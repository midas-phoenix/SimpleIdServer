﻿// Copyright (c) SimpleIdServer. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.
using Microsoft.AspNetCore.Mvc;
using SimpleIdServer.IdServer.Api.Token.Handlers;
using SimpleIdServer.IdServer.Api.Token.Helpers;
using SimpleIdServer.IdServer.Domains;
using SimpleIdServer.IdServer.DTOs;
using SimpleIdServer.IdServer.Exceptions;
using SimpleIdServer.IdServer.Store;
using System;
using System.Net;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;
using static SimpleIdServer.IdServer.Constants;

namespace SimpleIdServer.IdServer.Api.BCAuthorize
{
    public interface IBCAuthorizeHandler
    {
        Task<IActionResult> Create(HandlerContext context, CancellationToken cancellationToken);
    }

    public class BCAuthorizeHandler : IBCAuthorizeHandler
    {
        private readonly IClientAuthenticationHelper _clientAuthenticationHelper;
        private readonly IBCAuthorizeRequestValidator _bcAuthorizeRequestValidator;
        private readonly IBCNotificationService _bcNotificationService;
        private readonly IBCAuthorizeRepository _bcAuthorizeRepository;

        public BCAuthorizeHandler(
            IClientAuthenticationHelper clientAuthenticationHelper,
            IBCAuthorizeRequestValidator bcAuthorizeRequestValidator,
            IBCNotificationService bcNotificationService,
            IBCAuthorizeRepository bcAuthorizeRepository)
        {
            _clientAuthenticationHelper = clientAuthenticationHelper;
            _bcAuthorizeRequestValidator = bcAuthorizeRequestValidator;
            _bcNotificationService = bcNotificationService;
            _bcAuthorizeRepository = bcAuthorizeRepository;
        }

        public async Task<IActionResult> Create(HandlerContext context, CancellationToken cancellationToken)
        {
            try
            {
                Client oauthClient = await _clientAuthenticationHelper.AuthenticateClient(context.Request.HttpHeader, context.Request.RequestData, context.Request.Certificate, context.Request.IssuerName, cancellationToken, ErrorCodes.INVALID_REQUEST);
                context.SetClient(oauthClient);
                var user = await _bcAuthorizeRequestValidator.ValidateCreate(context, cancellationToken);
                context.SetUser(user);
                var requestedExpiry = context.Request.RequestData.GetRequestedExpiry() ?? context.Client.AuthReqIdExpirationTimeInSeconds;
                var currentDateTime = DateTime.UtcNow;
                var openidClient = oauthClient;
                var interval = oauthClient.BCIntervalSeconds;
                var bcAuthorize = Domains.BCAuthorize.Create(
                    currentDateTime.AddSeconds(requestedExpiry),
                    oauthClient.ClientId,
                    interval,
                    openidClient.BCClientNotificationEndpoint,
                    openidClient.BCTokenDeliveryMode,
                    context.Request.RequestData.GetScopesFromAuthorizationRequest(),
                    context.User.Id,
                    context.Request.RequestData.GetClientNotificationToken());
                bcAuthorize.IncrementNextFetchTime();
                _bcAuthorizeRepository.Add(bcAuthorize);
                await _bcAuthorizeRepository.SaveChanges(cancellationToken);

                var bindingMessage = context.Request.RequestData.GetBindingMessage();
                await _bcNotificationService.Notify(context, new BCNotificationMessage { ClientId = context.Client.ClientId, AuthReqId = bcAuthorize.Id, BindingMessage = bindingMessage, Scopes = bcAuthorize.Scopes }, cancellationToken);

                var res = new JsonObject
                {
                    { BCAuthenticationResponseParameters.AuthReqId, bcAuthorize.Id },
                    { BCAuthenticationResponseParameters.ExpiresIn, requestedExpiry },
                };
                if (oauthClient.BCTokenDeliveryMode == StandardNotificationModes.Ping ||
                    oauthClient.BCTokenDeliveryMode == StandardNotificationModes.Poll)
                    res.Add(BCAuthenticationResponseParameters.Interval, interval);

                return new OkObjectResult(res);
            }
            catch (OAuthUnauthorizedException ex)
            {
                return BaseCredentialsHandler.BuildError(HttpStatusCode.Unauthorized, ex.Code, ex.Message);
            }
            catch (OAuthException ex)
            {
                return BaseCredentialsHandler.BuildError(HttpStatusCode.BadRequest, ex.Code, ex.Message);
            }
        }
    }
}
