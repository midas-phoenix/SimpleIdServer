﻿// Copyright (c) SimpleIdServer. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SimpleIdServer.IdServer.Domains;
using SimpleIdServer.IdServer.Authenticate.AssertionParsers;
using SimpleIdServer.IdServer.Exceptions;
using SimpleIdServer.IdServer.Options;
using SimpleIdServer.IdServer.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleIdServer.IdServer.Authenticate
{
    public interface IAuthenticateClient
    {
        Task<Client> Authenticate(AuthenticateInstruction authenticateIsnstruction, string issuerName, CancellationToken cancellationToken, bool isAuthorizationCodeGrantType = false, string errorCode = ErrorCodes.INVALID_CLIENT);
    }

    public class AuthenticateClient : IAuthenticateClient
    {
        private readonly IClientRepository _clientRepository;
        private readonly IEnumerable<IOAuthClientAuthenticationHandler> _handlers;
        private readonly IEnumerable<IClientAssertionParser> _clientAssertionParsers;
        private readonly IdServerHostOptions _options;

        public AuthenticateClient(IClientRepository clientRepository, IEnumerable<IOAuthClientAuthenticationHandler> handlers, IEnumerable<IClientAssertionParser> clientAssertionParsers, IOptions<IdServerHostOptions> options)
        {
            _clientRepository = clientRepository;
            _handlers = handlers;
            _clientAssertionParsers = clientAssertionParsers;
            _options = options.Value;
        }

        public async Task<Client> Authenticate(AuthenticateInstruction authenticateInstruction, string issuerName, CancellationToken cancellationToken, bool isAuthorizationCodeGrantType = false, string errorCode = ErrorCodes.INVALID_CLIENT)
        {
            if (authenticateInstruction == null) throw new ArgumentNullException(nameof(authenticateInstruction));

            string clientId;
            if (!TryGetClientId(authenticateInstruction, out clientId)) throw new OAuthException(errorCode, ErrorMessages.MISSING_CLIENT_ID);

            var client = await _clientRepository.Query()
                .AsNoTracking()
                .Include(c => c.SerializedJsonWebKeys)
                .Include(c => c.Scopes)
                .FirstOrDefaultAsync(c => c.ClientId == clientId, cancellationToken);
            if (client == null) throw new OAuthException(errorCode, string.Format(ErrorMessages.UNKNOWN_CLIENT, clientId));
            if (isAuthorizationCodeGrantType) return client;

            var tokenEndPointAuthMethod = client.TokenEndPointAuthMethod ?? _options.DefaultTokenEndPointAuthMethod;
            var handler = _handlers.FirstOrDefault(h => h.AuthMethod == tokenEndPointAuthMethod);
            if (handler == null) throw new OAuthException(errorCode, string.Format(ErrorMessages.UNKNOWN_AUTH_METHOD, tokenEndPointAuthMethod));

            if (!await handler.Handle(authenticateInstruction, client, issuerName, cancellationToken, errorCode)) throw new OAuthException(errorCode, ErrorMessages.BAD_CLIENT_CREDENTIAL);

            return client;
        }

        private bool TryGetClientId(AuthenticateInstruction instruction, out string clientId)
        {
            clientId = null;
            if (TryExtractClientIdFromClientAssertion(instruction, out clientId)) return true;

            clientId = instruction.ClientIdFromAuthorizationHeader;
            if (!string.IsNullOrWhiteSpace(clientId)) return true;
            
            clientId = instruction.ClientIdFromHttpRequestBody;
            if (!string.IsNullOrWhiteSpace(clientId)) return true;
            return false;
        }

        public bool TryExtractClientIdFromClientAssertion(AuthenticateInstruction instruction, out string clientId)
        {
            clientId = null;
            if (string.IsNullOrWhiteSpace(instruction.ClientAssertionType)) return false;
            var type = instruction.ClientAssertionType;
            var parser = _clientAssertionParsers.FirstOrDefault(c => c.Type == type);
            if (parser == null) throw new OAuthException(ErrorCodes.INVALID_REQUEST, string.Format(ErrorMessages.CLIENT_ASSERTION_TYPE_NOT_SUPPORTED, type));
            var clientAssertion = instruction.ClientAssertion;
            if (string.IsNullOrWhiteSpace(clientAssertion)) throw new OAuthException(ErrorCodes.INVALID_REQUEST, ErrorMessages.CLIENT_ASSERTION_IS_MISSING);
            if (!parser.TryExtractClientId(clientAssertion, out clientId)) return false;
            if (string.IsNullOrWhiteSpace(clientId)) throw new OAuthException(ErrorCodes.INVALID_REQUEST, ErrorMessages.CLIENT_ID_CANNOT_BE_EXTRACTED_FROM_CLIENT_ASSERTION);
            return true;
        }
    }
}