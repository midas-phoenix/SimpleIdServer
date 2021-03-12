﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (https://www.specflow.org/).
//      SpecFlow Version:3.7.0.0
//      SpecFlow Generator Version:3.7.0.0
// 
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
#region Designer generated code
#pragma warning disable
namespace SimpleIdServer.OpenBankingApi.Host.Acceptance.Tests.Features
{
    using TechTalk.SpecFlow;
    using System;
    using System.Linq;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "3.7.0.0")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public partial class AuthorizationFeature : object, Xunit.IClassFixture<AuthorizationFeature.FixtureData>, System.IDisposable
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
        private string[] _featureTags = ((string[])(null));
        
        private Xunit.Abstractions.ITestOutputHelper _testOutputHelper;
        
#line 1 "Authorization.feature"
#line hidden
        
        public AuthorizationFeature(AuthorizationFeature.FixtureData fixtureData, SimpleIdServer_OpenBankingApi_Host_Acceptance_Tests_XUnitAssemblyFixture assemblyFixture, Xunit.Abstractions.ITestOutputHelper testOutputHelper)
        {
            this._testOutputHelper = testOutputHelper;
            this.TestInitialize();
        }
        
        public static void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "Features", "Authorization", "\tCheck /authorization endpoint", ProgrammingLanguage.CSharp, ((string[])(null)));
            testRunner.OnFeatureStart(featureInfo);
        }
        
        public static void FeatureTearDown()
        {
            testRunner.OnFeatureEnd();
            testRunner = null;
        }
        
        public virtual void TestInitialize()
        {
        }
        
        public virtual void TestTearDown()
        {
            testRunner.OnScenarioEnd();
        }
        
        public virtual void ScenarioInitialize(TechTalk.SpecFlow.ScenarioInfo scenarioInfo)
        {
            testRunner.OnScenarioInitialize(scenarioInfo);
            testRunner.ScenarioContext.ScenarioContainer.RegisterInstanceAs<Xunit.Abstractions.ITestOutputHelper>(_testOutputHelper);
        }
        
        public virtual void ScenarioStart()
        {
            testRunner.OnScenarioStart();
        }
        
        public virtual void ScenarioCleanup()
        {
            testRunner.CollectScenarioErrors();
        }
        
        void System.IDisposable.Dispose()
        {
            this.TestTearDown();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="Check s_hash & sub is returned in id_token")]
        [Xunit.TraitAttribute("FeatureTitle", "Authorization")]
        [Xunit.TraitAttribute("Description", "Check s_hash & sub is returned in id_token")]
        public virtual void CheckS_HashSubIsReturnedInId_Token()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Check s_hash & sub is returned in id_token", null, tagsOfScenario, argumentsOfScenario, this._featureTags);
#line 4
this.ScenarioInitialize(scenarioInfo);
#line hidden
            bool isScenarioIgnored = default(bool);
            bool isFeatureIgnored = default(bool);
            if ((tagsOfScenario != null))
            {
                isScenarioIgnored = tagsOfScenario.Where(__entry => __entry != null).Where(__entry => String.Equals(__entry, "ignore", StringComparison.CurrentCultureIgnoreCase)).Any();
            }
            if ((this._featureTags != null))
            {
                isFeatureIgnored = this._featureTags.Where(__entry => __entry != null).Where(__entry => String.Equals(__entry, "ignore", StringComparison.CurrentCultureIgnoreCase)).Any();
            }
            if ((isScenarioIgnored || isFeatureIgnored))
            {
                testRunner.SkipScenario();
            }
            else
            {
                this.ScenarioStart();
                TechTalk.SpecFlow.Table table20 = new TechTalk.SpecFlow.Table(new string[] {
                            "Type",
                            "Kid",
                            "AlgName"});
                table20.AddRow(new string[] {
                            "SIG",
                            "1",
                            "RS256"});
#line 5
 testRunner.When("build JSON Web Keys, store JWKS into \'jwks\' and store the public keys into \'jwks_" +
                        "json\'", ((string)(null)), table20, "When ");
#line hidden
                TechTalk.SpecFlow.Table table21 = new TechTalk.SpecFlow.Table(new string[] {
                            "Key",
                            "Value"});
                table21.AddRow(new string[] {
                            "token_endpoint_auth_method",
                            "tls_client_auth"});
                table21.AddRow(new string[] {
                            "response_types",
                            "[token,code,id_token]"});
                table21.AddRow(new string[] {
                            "grant_types",
                            "[client_credentials]"});
                table21.AddRow(new string[] {
                            "scope",
                            "accounts"});
                table21.AddRow(new string[] {
                            "redirect_uris",
                            "[https://localhost:8080/callback]"});
                table21.AddRow(new string[] {
                            "tls_client_auth_san_dns",
                            "firstMtlsClient"});
                table21.AddRow(new string[] {
                            "id_token_signed_response_alg",
                            "PS256"});
                table21.AddRow(new string[] {
                            "token_signed_response_alg",
                            "PS256"});
                table21.AddRow(new string[] {
                            "request_object_signing_alg",
                            "RS256"});
                table21.AddRow(new string[] {
                            "jwks",
                            "$jwks_json$"});
#line 9
 testRunner.When("execute HTTP POST JSON request \'https://localhost:8080/register\'", ((string)(null)), table21, "When ");
#line hidden
#line 22
 testRunner.And("extract JSON from body", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 23
 testRunner.And("extract parameter \'client_id\' from JSON body", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 24
 testRunner.And("add authorized Account Access Consent \'$client_id$\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
                TechTalk.SpecFlow.Table table22 = new TechTalk.SpecFlow.Table(new string[] {
                            "Key",
                            "Value"});
                table22.AddRow(new string[] {
                            "response_type",
                            "code id_token"});
                table22.AddRow(new string[] {
                            "client_id",
                            "$client_id$"});
                table22.AddRow(new string[] {
                            "state",
                            "state"});
                table22.AddRow(new string[] {
                            "response_mode",
                            "fragment"});
                table22.AddRow(new string[] {
                            "scope",
                            "accounts"});
                table22.AddRow(new string[] {
                            "redirect_uri",
                            "https://localhost:8080/callback"});
                table22.AddRow(new string[] {
                            "nonce",
                            "nonce"});
                table22.AddRow(new string[] {
                            "exp",
                            "$tomorrow$"});
                table22.AddRow(new string[] {
                            "aud",
                            "https://localhost:8080"});
                table22.AddRow(new string[] {
                            "claims",
                            "{ id_token: { openbanking_intent_id: { value: \"$consentId$\", essential : true } }" +
                                " }"});
#line 26
 testRunner.And("use \'1\' JWK from \'jwks\' to build JWS and store into \'request\'", ((string)(null)), table22, "And ");
#line hidden
                TechTalk.SpecFlow.Table table23 = new TechTalk.SpecFlow.Table(new string[] {
                            "Key",
                            "Value"});
                table23.AddRow(new string[] {
                            "response_type",
                            "code id_token"});
                table23.AddRow(new string[] {
                            "client_id",
                            "$client_id$"});
                table23.AddRow(new string[] {
                            "request",
                            "$request$"});
                table23.AddRow(new string[] {
                            "scope",
                            "accounts"});
#line 39
 testRunner.And("execute HTTP GET request \'https://localhost:8080/authorization\'", ((string)(null)), table23, "And ");
#line hidden
#line 46
 testRunner.And("extract \'id_token\' from callback", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 47
 testRunner.And("extract payload from JWS \'$id_token$\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 49
 testRunner.Then("token claim \'s_hash\'=\'S6aXNcpTdl7WpwnttWxuog\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
#line 50
 testRunner.Then("token claim \'sub\'=\'$consentId$\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "3.7.0.0")]
        [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
        public class FixtureData : System.IDisposable
        {
            
            public FixtureData()
            {
                AuthorizationFeature.FeatureSetup();
            }
            
            void System.IDisposable.Dispose()
            {
                AuthorizationFeature.FeatureTearDown();
            }
        }
    }
}
#pragma warning restore
#endregion