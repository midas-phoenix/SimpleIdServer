﻿// Copyright (c) SimpleIdServer. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using SimpleIdServer.IdServer.Domains;
using SqlSugar;

namespace SimpleIdServer.IdServer.Store.SqlSugar.Models;

[SugarTable("IdentityProvisioningMappingRule")]
public class SugarIdentityProvisioningMappingRule
{
    [SugarColumn(IsPrimaryKey = true)]  
    public string Id { get; set; } = null!;
    public string From { get; set; } = null!;
    public MappingRuleTypes MapperType { get; set; }
    public string? TargetUserAttribute { get; set; } = null;
    public string? TargetUserProperty { get; set; } = null;
    public bool HasMultipleAttribute { get; set; } = false;
    public string IdentityProvisioningDefinitionName { get; set; } = null!;
    public IdentityProvisioningMappingUsage Usage { get; set; } = IdentityProvisioningMappingUsage.USER;
    [Navigate(NavigateType.ManyToOne, nameof(IdentityProvisioningDefinitionName))]
    public SugarIdentityProvisioningDefinition IdentityProvisioningDefinition { get; set; } = null!;

    public IdentityProvisioningMappingRule ToDomain()
    {
        return new IdentityProvisioningMappingRule
        {
            Id = Id,
            From = From,
            MapperType = MapperType,
            TargetUserAttribute = TargetUserAttribute,
            TargetUserProperty = TargetUserProperty,
            HasMultipleAttribute = HasMultipleAttribute,
            Usage = Usage,
            IdentityProvisioningDefinition = IdentityProvisioningDefinition?.ToDomain()
        };
    }
}
