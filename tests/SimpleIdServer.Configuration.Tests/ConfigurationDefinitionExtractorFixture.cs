﻿// Copyright (c) SimpleIdServer. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.
using NUnit.Framework;
using SimpleIdServer.IdServer.Domains;

namespace SimpleIdServer.Configuration.Tests;

public class ConfigurationDefinitionExtractorFixture
{
    [Test]
    public void When_Extract_UserOptions_To_ConfigurationDefinition_Then_ExtractionIsCorrect()
    {
        // ACT
        var configurationDefinition = ConfigurationDefinitionExtractor.Extract<UserOptions>();

        // ASSERT
        var firstNameDefinition = configurationDefinition.Records.Single(r => r.Name == nameof(UserOptions.FirstName));
        var genderDefinition = configurationDefinition.Records.Single(r => r.Name == nameof(UserOptions.Gender));
        var statusDefinition = configurationDefinition.Records.Single(r => r.Name == nameof(UserOptions.Status));
        var isActiveDefinition = configurationDefinition.Records.Single(r => r.Name == nameof(UserOptions.IsActive));
        var ageDefinition = configurationDefinition.Records.Single(r => r.Name == nameof(UserOptions.Age));
        var genderValues = genderDefinition.Values;
        var statusValues = statusDefinition.Values;

        Assert.That(firstNameDefinition.Name, Is.EqualTo(nameof(UserOptions.FirstName)));
        Assert.That(genderDefinition.Name, Is.EqualTo(nameof(UserOptions.Gender)));
        Assert.That(statusDefinition.Name, Is.EqualTo(nameof(UserOptions.Status)));
        Assert.That(isActiveDefinition.Name, Is.EqualTo(nameof(UserOptions.IsActive)));

        Assert.That(firstNameDefinition.Type, Is.EqualTo(ConfigurationDefinitionRecordTypes.INPUT));
        Assert.That(genderDefinition.Type, Is.EqualTo(ConfigurationDefinitionRecordTypes.SELECT));
        Assert.That(statusDefinition.Type, Is.EqualTo(ConfigurationDefinitionRecordTypes.MULTISELECT));
        Assert.That(isActiveDefinition.Type, Is.EqualTo(ConfigurationDefinitionRecordTypes.CHECKBOX));
        Assert.That(ageDefinition.Type, Is.EqualTo(ConfigurationDefinitionRecordTypes.NUMBER));

        Assert.That(firstNameDefinition.DisplayName, Is.EqualTo("FirstName"));
        Assert.That(firstNameDefinition.Description, Is.EqualTo("Description"));
        Assert.That(genderDefinition.DisplayName, Is.EqualTo("Gender"));
        Assert.That(genderDefinition.Description, Is.EqualTo("Gender"));
        Assert.That(statusDefinition.DisplayName, Is.EqualTo("Status"));
        Assert.That(statusDefinition.Description, Is.EqualTo("Status"));
        Assert.That(isActiveDefinition.DisplayName, Is.EqualTo("IsActive"));
        Assert.That(isActiveDefinition.Description, Is.EqualTo("IsActive"));
        Assert.That(ageDefinition.DisplayName, Is.EqualTo("Age"));
        Assert.That(ageDefinition.Description, Is.EqualTo("Age"));

        Assert.That(genderValues.Count(), Is.EqualTo(2));
        Assert.That(genderValues.First().Name, Is.EqualTo("Male"));
        Assert.That(genderValues.Last().Name, Is.EqualTo("Female"));
        Assert.That(genderValues.First().Value, Is.EqualTo("0"));
        Assert.That(genderValues.Last().Value, Is.EqualTo("1"));

        Assert.That(statusValues.Count(), Is.EqualTo(2));
        Assert.That(statusValues.First().Name, Is.EqualTo("Freelance"));
        Assert.That(statusValues.Last().Name, Is.EqualTo("Employee"));
        Assert.That(statusValues.First().Value, Is.EqualTo("0"));
        Assert.That(statusValues.Last().Value, Is.EqualTo("1"));
    }
}
