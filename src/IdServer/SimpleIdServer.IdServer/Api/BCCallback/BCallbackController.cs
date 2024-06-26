﻿// Copyright (c) SimpleIdServer. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.
using Hangfire;
using Microsoft.AspNetCore.Mvc;
using SimpleIdServer.IdServer.Api.BCCallback;
using SimpleIdServer.IdServer.Exceptions;
using SimpleIdServer.IdServer.Jobs;
using SimpleIdServer.IdServer.Jwt;
using SimpleIdServer.IdServer.Resources;
using SimpleIdServer.IdServer.Stores;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleIdServer.IdServer.Api.BCAuthorize
{
    public class BCCallbackController : BaseController
    {
        private readonly IBCAuthorizeRepository _bcAuthorizeRepository;
        private readonly IRecurringJobManager _recurringJobManager;
        private readonly ITransactionBuilder _transactionBuilder;

        public BCCallbackController(ITokenRepository tokenRepository,
            IJwtBuilder jwtBuilder, 
            IBCAuthorizeRepository bCAuthorizeRepository, 
            IRecurringJobManager recurringJobManager,
            ITransactionBuilder transactionBuilder) : base(tokenRepository, jwtBuilder)
        {
            _bcAuthorizeRepository = bCAuthorizeRepository;
            _recurringJobManager = recurringJobManager;
            _transactionBuilder = transactionBuilder;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromRoute] string prefix, [FromBody] BCCallbackParameter parameter, CancellationToken cancellationToken)
        {
            try
            {
                using (var transaction = _transactionBuilder.Build())
                {
                    prefix = prefix ?? Constants.DefaultRealm;
                    var bcAuthorize = await _bcAuthorizeRepository.GetById(parameter.AuthReqId, cancellationToken);
                    if (bcAuthorize == null) return BuildError(HttpStatusCode.NotFound, ErrorCodes.INVALID_REQUEST, string.Format(Global.UnknownBcAuthorize, parameter.AuthReqId));
                    if (!bcAuthorize.IsActive) return BuildError(HttpStatusCode.BadRequest, ErrorCodes.INVALID_REQUEST, Global.ExpiredBcAuthorize);
                    if (bcAuthorize.LastStatus != Domains.BCAuthorizeStatus.Pending) return BuildError(HttpStatusCode.BadRequest, ErrorCodes.INVALID_REQUEST, Global.BcAuthorizeNotPending);
                    switch (parameter.ActionEnum)
                    {
                        case BCCallbackActions.CONFIRM:
                            bcAuthorize.Confirm();
                            break;
                        case BCCallbackActions.REJECT:
                            bcAuthorize.Reject();
                            break;
                    }

                    _bcAuthorizeRepository.Update(bcAuthorize);
                    await transaction.Commit(cancellationToken);
                    _recurringJobManager.Trigger(nameof(BCNotificationJob));
                    return new NoContentResult();
                }
            }
            catch (OAuthException ex)
            {
                return BuildError(ex);
            }
        }
    }
}
