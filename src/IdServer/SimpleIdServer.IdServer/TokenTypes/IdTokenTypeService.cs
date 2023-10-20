﻿// Copyright (c) SimpleIdServer. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using SimpleIdServer.IdServer.Domains;
using SimpleIdServer.IdServer.DTOs;
using SimpleIdServer.IdServer.Exceptions;
using SimpleIdServer.IdServer.Jwt;
using SimpleIdServer.IdServer.Options;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleIdServer.IdServer.TokenTypes;

public class IdTokenTypeService : ITokenTypeService
{
    private readonly IJwtBuilder _jwtBuilder;
    private readonly IdServerHostOptions _options;

    public IdTokenTypeService(IJwtBuilder jwtBuilder, IOptions<IdServerHostOptions> options)
    {
        _jwtBuilder = jwtBuilder;
        _options = options.Value;
    }

    public const string NAME = "urn:ietf:params:oauth:token-type:id_token";
    public string Name => NAME;
    public string TokenType => TokenResponseParameters.IdToken;

    public TokenResult Parse(string realm, string token)
    {
        var extractionResult = _jwtBuilder.ReadSelfIssuedJsonWebToken(realm, token);
        if (extractionResult.Error != null) throw new OAuthException(ErrorCodes.INVALID_REQUEST, extractionResult.Error);
        return new TokenResult
        {
            Claims = extractionResult.Jwt.GetClaimsDic(),
            Subject = extractionResult.Jwt.Subject
        };
    }

    public async Task<string> Build(string realm, string issuer, Client client, Dictionary<string, object> claims, CancellationToken cancellationToken)
    {
        var securityTokenDescriptor = new SecurityTokenDescriptor
        {
            Issuer = issuer,
            IssuedAt = DateTime.UtcNow,
            Expires = DateTime.UtcNow.AddSeconds(client.TokenExpirationTimeInSeconds ?? _options.DefaultTokenExpirationTimeInSeconds),
            Claims = claims
        };
        var idToken = await _jwtBuilder.BuildClientToken(realm, client, securityTokenDescriptor, (client.IdTokenSignedResponseAlg ?? _options.DefaultTokenSignedResponseAlg), client.IdTokenEncryptedResponseAlg, client.IdTokenEncryptedResponseEnc, cancellationToken);
        return idToken;
    }
}
