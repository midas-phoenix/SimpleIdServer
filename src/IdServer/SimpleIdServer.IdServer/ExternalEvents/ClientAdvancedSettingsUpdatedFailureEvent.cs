﻿// Copyright (c) SimpleIdServer. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using SimpleIdServer.IdServer.Api.Clients;

namespace SimpleIdServer.IdServer.ExternalEvents
{
    internal class ClientAdvancedSettingsUpdatedFailureEvent : IExternalEvent
    {
        public string EventName => nameof(ClientAdvancedSettingsUpdatedFailureEvent);
        public string Realm { get; set; }
        public UpdateAdvancedClientSettingsRequest Request { get; set; }
        public string Id { get; set; }
    }
}
