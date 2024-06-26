﻿// Copyright (c) SimpleIdServer. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using SimpleIdServer.CredentialIssuer.Api.Credential.Validators;
using SimpleIdServer.CredentialIssuer.CredentialFormats;
using SimpleIdServer.CredentialIssuer.Domains;
using SimpleIdServer.CredentialIssuer.Store;
using SimpleIdServer.IdServer.CredentialIssuer;
using SimpleIdServer.IdServer.CredentialIssuer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleIdServer.CredentialIssuer.Api.Credential
{
    [Route(Constants.EndPoints.Credential)]
    [Authorize("ApiAuthenticated")]
    public class CredentialController : BaseController
    {
        private readonly IEnumerable<ICredentialFormatter> _formatters;
        private readonly ICredentialStore _credentialStore;
        private readonly ICredentialConfigurationStore _credentialConfigurationStore;
        private readonly IUserCredentialClaimStore _userCredentialClaimStore;
        private readonly IEnumerable<IKeyProofTypeValidator> _keyProofTypeValidators;
        private readonly CredentialIssuerOptions _options;

        public CredentialController(
            IEnumerable<ICredentialFormatter> formatters,
            ICredentialStore credentialStore,
            ICredentialConfigurationStore credentialConfigurationStore,
            IUserCredentialClaimStore userCredentialClaimStore,
            IEnumerable<IKeyProofTypeValidator> keyProofTypeValidators,
            IOptions<CredentialIssuerOptions> options)
        {
            _formatters = formatters;
            _credentialStore = credentialStore;
            _credentialConfigurationStore = credentialConfigurationStore;
            _userCredentialClaimStore = userCredentialClaimStore;
            _keyProofTypeValidators = keyProofTypeValidators;
            _options = options.Value;
        }

        [HttpPost]
        public async Task<IActionResult> Get([FromBody] CredentialRequest request, CancellationToken cancellationToken)
        {
            var subject = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var scope = User.Claims.SingleOrDefault(c => c.Type == "scope")?.Value;
            var authorizedScopes = new List<string>();
            if (!string.IsNullOrWhiteSpace(scope))
            {
                authorizedScopes = scope.Split(" ", StringSplitOptions.RemoveEmptyEntries).ToList();
            }

            var validationResult = await Validate(request, authorizedScopes, cancellationToken);
            if (validationResult.ErrorResult != null) return Build(validationResult.ErrorResult.Value);
            if (!string.IsNullOrWhiteSpace(validationResult.Subject))
                subject = validationResult.Subject;
            var buildRequest = new BuildCredentialRequest
            {
                Subject = subject,
                Issuer = _options.DidDocument.Id
            };
            var claims = new List<CredentialUserClaimNode>();
            List<CredentialUserClaimNode> userClaims = null;
            if (validationResult.Credential != null)
            {
                buildRequest.Id = $"{validationResult.Credential.Configuration.BaseUrl}/{validationResult.Credential.CredentialId}";
                buildRequest.JsonLdContext = validationResult.Credential.Configuration.JsonLdContext;
                buildRequest.Type = validationResult.Credential.Configuration.Type;
                buildRequest.ValidFrom = validationResult.Credential.IssueDateTime;
                buildRequest.ValidUntil = validationResult.Credential.ExpirationDateTime;
                buildRequest.CredentialConfiguration = validationResult.Credential.Configuration;
                userClaims = validationResult.Credential.Claims.Select(c =>
                {
                    var cl = validationResult.CredentialTemplate.Claims.Single(cl => cl.SourceUserClaimName == c.Name);
                    return new CredentialUserClaimNode
                    {
                        Level = cl.Name.Split('.').Count(),
                        Name = cl.Name,
                        Value = c.Value
                    };
                }).ToList();
            }
            else
            {
                buildRequest.Id = $"{validationResult.CredentialTemplate.BaseUrl}/{Guid.NewGuid()}";
                buildRequest.JsonLdContext = validationResult.CredentialTemplate.JsonLdContext;
                buildRequest.Type = validationResult.CredentialTemplate.Type;
                buildRequest.CredentialConfiguration = validationResult.CredentialTemplate;
                if (_options.CredentialExpirationTimeInSeconds != null)
                {
                    buildRequest.ValidFrom = DateTime.UtcNow;
                    buildRequest.ValidUntil = DateTime.UtcNow.AddSeconds(_options.CredentialExpirationTimeInSeconds.Value);
                }

                var userCredentials = await _userCredentialClaimStore.Resolve(subject, validationResult.CredentialTemplate.Claims, cancellationToken);
                userClaims = userCredentials.Select(c =>
                {
                    var cl = validationResult.CredentialTemplate.Claims.Single(cl => cl.SourceUserClaimName == c.Name);
                    return new CredentialUserClaimNode
                    {
                        Level = cl.Name.Split('.').Count(),
                        Name = cl.Name,
                        Value = c.Value
                    };
                }).ToList();
            }

            buildRequest.UserClaims = userClaims;
            var formatter = validationResult.Formatter;
            var credentialResult = formatter.Build(buildRequest, 
                _options.DidDocument, 
                _options.VerificationMethodId, 
                _options.AsymmKey);
            if(request.CredentialResponseEncryption != null)
            {
                var handler = new JsonWebTokenHandler();
                var encKey = request.CredentialResponseEncryption.Jwk;
                var encryptedCredential = handler.EncryptToken(credentialResult.ToJsonString(), new Microsoft.IdentityModel.Tokens.EncryptingCredentials(
                    encKey,
                    request.CredentialResponseEncryption.Alg,
                    request.CredentialResponseEncryption.Enc));
                credentialResult = encryptedCredential;
            }

            return new OkObjectResult(new CredentialResult
            {
                Credential = credentialResult,
                CNonce = validationResult.Nonce
            });
        }

        private async Task<CredentialValidationResult> Validate(CredentialRequest credentialRequest, List<string> authorizedScopes, CancellationToken cancellationToken)
        {
            string subject = null;
            string nonce = null;
            if (credentialRequest == null)
                return CredentialValidationResult.Error(new ErrorResult(HttpStatusCode.BadRequest, ErrorCodes.INVALID_CREDENTIAL_REQUEST, ErrorMessages.INVALID_INCOMING_REQUEST));

            var atCredentialIdentifiers = GetCredentialIdentifiers();
            if(atCredentialIdentifiers == null && string.IsNullOrWhiteSpace(credentialRequest.Format))
                return CredentialValidationResult.Error(new ErrorResult(HttpStatusCode.BadRequest, ErrorCodes.INVALID_CREDENTIAL_REQUEST, string.Format(ErrorMessages.MISSING_PARAMETER, CredentialRequestNames.Format)));

            if(credentialRequest.Proof != null)
            {
                if (string.IsNullOrWhiteSpace(credentialRequest.Proof.ProofType)) return CredentialValidationResult.Error(new ErrorResult(HttpStatusCode.BadRequest, ErrorCodes.INVALID_CREDENTIAL_REQUEST, string.Format(ErrorMessages.MISSING_PARAMETER, CredentialRequestNames.ProofType)));
                var proofType = _keyProofTypeValidators.SingleOrDefault(v => v.Type == credentialRequest.Proof.ProofType);
                if (proofType == null) return CredentialValidationResult.Error(new ErrorResult(HttpStatusCode.BadRequest, ErrorCodes.INVALID_CREDENTIAL_REQUEST, string.Format(ErrorMessages.INVALID_PROOF_FORMAT, credentialRequest.Proof.ProofType)));
                var proofTypeValidationResult = await proofType.Validate(credentialRequest.Proof, cancellationToken);
                if (!proofTypeValidationResult.IsValid) return CredentialValidationResult.Error(new ErrorResult(HttpStatusCode.BadRequest, ErrorCodes.INVALID_PROOF, proofTypeValidationResult.ErrorMessage));
                subject = proofTypeValidationResult.Subject;
                nonce = proofTypeValidationResult.CNonce;
            }

            if (atCredentialIdentifiers != null && string.IsNullOrWhiteSpace(credentialRequest.Credentialidentifier))
                return CredentialValidationResult.Error(new ErrorResult(HttpStatusCode.BadRequest, ErrorCodes.INVALID_CREDENTIAL_REQUEST, string.Format(ErrorMessages.MISSING_PARAMETER, CredentialRequestNames.CredentialIdentifier)));

            if(!string.IsNullOrWhiteSpace(credentialRequest.Format) && !string.IsNullOrWhiteSpace(credentialRequest.Credentialidentifier))
                return CredentialValidationResult.Error(new ErrorResult(HttpStatusCode.BadRequest, ErrorCodes.INVALID_CREDENTIAL_REQUEST, ErrorMessages.CANNOT_USER_CREDENTIAL_IDENTIFIER_WITH_FORMAT));

            if(!string.IsNullOrWhiteSpace(credentialRequest.Credentialidentifier) && !atCredentialIdentifiers.Contains(credentialRequest.Credentialidentifier))
                return CredentialValidationResult.Error(new ErrorResult(HttpStatusCode.BadRequest, ErrorCodes.INVALID_CREDENTIAL_REQUEST, ErrorMessages.INVALID_CREDENTIAL_IDENTIFIER));

            if (credentialRequest.CredentialResponseEncryption != null)
            {
                if (string.IsNullOrWhiteSpace(credentialRequest.CredentialResponseEncryption.Alg))
                    return CredentialValidationResult.Error(new ErrorResult(HttpStatusCode.BadRequest, ErrorCodes.INVALID_ENCRYPTION_PARAMETERS, string.Format(ErrorMessages.MISSING_PARAMETER, CredentialRequestNames.Alg)));
                if (string.IsNullOrWhiteSpace(credentialRequest.CredentialResponseEncryption.Enc))
                    return CredentialValidationResult.Error(new ErrorResult(HttpStatusCode.BadRequest, ErrorCodes.INVALID_ENCRYPTION_PARAMETERS, string.Format(ErrorMessages.MISSING_PARAMETER, CredentialRequestNames.Enc)));
                if (credentialRequest.CredentialResponseEncryption.Jwk == null)
                    return CredentialValidationResult.Error(new ErrorResult(HttpStatusCode.BadRequest, ErrorCodes.INVALID_ENCRYPTION_PARAMETERS, string.Format(ErrorMessages.MISSING_PARAMETER, CredentialRequestNames.Jwk)));
            }

            if (!string.IsNullOrWhiteSpace(credentialRequest.Format))
            {
                var formatter = _formatters.SingleOrDefault(f => f.Format == credentialRequest.Format);
                if (formatter == null) return CredentialValidationResult.Error(new ErrorResult(HttpStatusCode.BadRequest, ErrorCodes.UNSUPPORTED_CREDENTIAL_FORMAT, string.Format(ErrorMessages.UNSUPPORTED_CREDENTIAL_FORMAT, credentialRequest.Format)));
                var header = formatter.ExtractHeader(credentialRequest.Data);
                if (header == null) return CredentialValidationResult.Error(new ErrorResult(HttpStatusCode.BadRequest, ErrorCodes.INVALID_CREDENTIAL_REQUEST, ErrorMessages.CREDENTIAL_TYPE_CANNOT_BE_EXTRACTED));
                var credentialConfiguration = await _credentialConfigurationStore.GetByTypeAndFormat(header.Type, credentialRequest.Format, cancellationToken);
                if (credentialConfiguration == null) return CredentialValidationResult.Error(new ErrorResult(HttpStatusCode.BadRequest, ErrorCodes.UNSUPPORTED_CREDENTIAL_TYPE, string.Format(ErrorMessages.UNSUPPORTED_CREDENTIAL_TYPE, header.Type)));
                if (!authorizedScopes.Any(s => credentialConfiguration.Scope == s)) return CredentialValidationResult.Error(new ErrorResult(HttpStatusCode.Unauthorized, ErrorCodes.UNAUTHORIZED, string.Format(ErrorMessages.UNAUTHORIZED_TO_ACCESS, header.Type))); 
                return CredentialValidationResult.Ok(formatter, credentialConfiguration, subject, nonce);
            }

            var credential = await _credentialStore.GetByCredentialId(credentialRequest.Credentialidentifier, cancellationToken);
            if (credential == null) return CredentialValidationResult.Error(new ErrorResult(HttpStatusCode.BadRequest, ErrorCodes.INVALID_CREDENTIAL_REQUEST, string.Format(ErrorMessages.UNKNOWN_CREDENTIAL_ID, credentialRequest.Credentialidentifier)));
            return CredentialValidationResult.Ok(_formatters.Single(f => f.Format == credential.Configuration.Format), credential, subject, nonce);
        }

        private class CredentialValidationResult : BaseValidationResult
        {
            private CredentialValidationResult(ErrorResult error) : base(error)
            {
            }

            private CredentialValidationResult(ICredentialFormatter formatter, CredentialConfiguration credentialTemplate, string subject, string nonce)
            {
                Formatter = formatter;
                CredentialTemplate = credentialTemplate;
                Subject = subject;
                Nonce = nonce;
            }

            private CredentialValidationResult(ICredentialFormatter formatter, Domains.Credential credential, string subject, string nonce)
            {
                Formatter = formatter;
                Credential = credential;
                Subject = subject;
                Nonce = nonce;
            }

            public ICredentialFormatter Formatter { get; private set; }

            public CredentialConfiguration CredentialTemplate { get; private set; }

            public string Subject { get; private set; }

            public string Nonce { get; private set; }

            public Domains.Credential Credential { get; private set; }

            public static CredentialValidationResult Ok(ICredentialFormatter formatter, CredentialConfiguration credentialTemplate, string subject, string nonce) => new CredentialValidationResult(formatter, credentialTemplate, subject, nonce);

            public static CredentialValidationResult Ok(ICredentialFormatter formatter, Domains.Credential credential, string subject, string nonce) => new CredentialValidationResult(formatter, credential, subject, nonce);

            public static CredentialValidationResult Error(ErrorResult error) => new CredentialValidationResult(error);
        }

        private List<string> GetCredentialIdentifiers()
        {
            var claim = User.Claims.SingleOrDefault(c => c.Type == "authorization_details");
            if (claim == null) return null;
            var jsonObj = JsonObject.Parse(claim.Value).AsObject();
            if (!jsonObj.ContainsKey("credential_identifiers")) return null;
            return (jsonObj["credential_identifiers"] as JsonArray).Select(c => c.ToString()).ToList();
        }
    }
}
