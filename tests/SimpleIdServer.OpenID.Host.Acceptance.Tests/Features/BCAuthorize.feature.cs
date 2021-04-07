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
namespace SimpleIdServer.OpenID.Host.Acceptance.Tests.Features
{
    using TechTalk.SpecFlow;
    using System;
    using System.Linq;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "3.7.0.0")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public partial class BCAuthorizeFeature : object, Xunit.IClassFixture<BCAuthorizeFeature.FixtureData>, System.IDisposable
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
        private string[] _featureTags = ((string[])(null));
        
        private Xunit.Abstractions.ITestOutputHelper _testOutputHelper;
        
#line 1 "BCAuthorize.feature"
#line hidden
        
        public BCAuthorizeFeature(BCAuthorizeFeature.FixtureData fixtureData, SimpleIdServer_OpenID_Host_Acceptance_Tests_XUnitAssemblyFixture assemblyFixture, Xunit.Abstractions.ITestOutputHelper testOutputHelper)
        {
            this._testOutputHelper = testOutputHelper;
            this.TestInitialize();
        }
        
        public static void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "Features", "BCAuthorize", "\tCheck /mtls/bc-authorize endpoint", ProgrammingLanguage.CSharp, ((string[])(null)));
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
        
        [Xunit.SkippableFactAttribute(DisplayName="Use push notification mode")]
        [Xunit.TraitAttribute("FeatureTitle", "BCAuthorize")]
        [Xunit.TraitAttribute("Description", "Use push notification mode")]
        public virtual void UsePushNotificationMode()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Use push notification mode", null, tagsOfScenario, argumentsOfScenario, this._featureTags);
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
                TechTalk.SpecFlow.Table table168 = new TechTalk.SpecFlow.Table(new string[] {
                            "Type",
                            "Kid",
                            "AlgName"});
                table168.AddRow(new string[] {
                            "SIG",
                            "1",
                            "RS256"});
#line 5
 testRunner.When("add JSON web key to Authorization Server and store into \'jwks\'", ((string)(null)), table168, "When ");
#line hidden
                TechTalk.SpecFlow.Table table169 = new TechTalk.SpecFlow.Table(new string[] {
                            "Key",
                            "Value"});
                table169.AddRow(new string[] {
                            "token_endpoint_auth_method",
                            "tls_client_auth"});
                table169.AddRow(new string[] {
                            "response_types",
                            "[token]"});
                table169.AddRow(new string[] {
                            "grant_types",
                            "[client_credentials]"});
                table169.AddRow(new string[] {
                            "scope",
                            "openid profile"});
                table169.AddRow(new string[] {
                            "redirect_uris",
                            "[http://localhost:8080]"});
                table169.AddRow(new string[] {
                            "tls_client_auth_san_dns",
                            "firstMtlsClient"});
                table169.AddRow(new string[] {
                            "backchannel_token_delivery_mode",
                            "push"});
                table169.AddRow(new string[] {
                            "backchannel_client_notification_endpoint",
                            "https://localhost:8080/pushNotificationEdp"});
#line 9
 testRunner.And("execute HTTP POST JSON request \'https://localhost:8080/register\'", ((string)(null)), table169, "And ");
#line hidden
#line 20
 testRunner.And("extract JSON from body", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 21
 testRunner.And("extract parameter \'client_id\' from JSON body", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
                TechTalk.SpecFlow.Table table170 = new TechTalk.SpecFlow.Table(new string[] {
                            "Key",
                            "Value"});
                table170.AddRow(new string[] {
                            "X-Testing-ClientCert",
                            "mtlsClient.crt"});
                table170.AddRow(new string[] {
                            "client_id",
                            "$client_id$"});
                table170.AddRow(new string[] {
                            "login_hint",
                            "administrator"});
                table170.AddRow(new string[] {
                            "scope",
                            "openid profile"});
                table170.AddRow(new string[] {
                            "client_notification_token",
                            "7dc3061e-bad9-4817-bd33-8db789bfb516"});
#line 23
 testRunner.And("execute HTTP POST JSON request \'https://localhost:8080/mtls/bc-authorize\'", ((string)(null)), table170, "And ");
#line hidden
#line 31
 testRunner.And("extract JSON from body", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 32
 testRunner.And("extract parameter \'auth_req_id\' from JSON body", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 34
 testRunner.And("confirm authorization request \'$auth_req_id$\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 35
 testRunner.And("poll until \'callbackResponse\' is received", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 36
 testRunner.And("extract JSON from \'callbackResponse\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 38
 testRunner.Then("JSON contains \'auth_req_id\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
#line 39
 testRunner.Then("JSON contains \'id_token\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
#line 40
 testRunner.Then("JSON contains \'access_token\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
#line 41
 testRunner.Then("JSON contains \'refresh_token\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="Use ping notification mode")]
        [Xunit.TraitAttribute("FeatureTitle", "BCAuthorize")]
        [Xunit.TraitAttribute("Description", "Use ping notification mode")]
        public virtual void UsePingNotificationMode()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Use ping notification mode", null, tagsOfScenario, argumentsOfScenario, this._featureTags);
#line 43
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
                TechTalk.SpecFlow.Table table171 = new TechTalk.SpecFlow.Table(new string[] {
                            "Type",
                            "Kid",
                            "AlgName"});
                table171.AddRow(new string[] {
                            "SIG",
                            "1",
                            "RS256"});
#line 44
 testRunner.When("add JSON web key to Authorization Server and store into \'jwks\'", ((string)(null)), table171, "When ");
#line hidden
                TechTalk.SpecFlow.Table table172 = new TechTalk.SpecFlow.Table(new string[] {
                            "Key",
                            "Value"});
                table172.AddRow(new string[] {
                            "token_endpoint_auth_method",
                            "tls_client_auth"});
                table172.AddRow(new string[] {
                            "response_types",
                            "[token]"});
                table172.AddRow(new string[] {
                            "grant_types",
                            "[client_credentials]"});
                table172.AddRow(new string[] {
                            "scope",
                            "openid profile"});
                table172.AddRow(new string[] {
                            "redirect_uris",
                            "[http://localhost:8080]"});
                table172.AddRow(new string[] {
                            "tls_client_auth_san_dns",
                            "firstMtlsClient"});
                table172.AddRow(new string[] {
                            "backchannel_token_delivery_mode",
                            "ping"});
                table172.AddRow(new string[] {
                            "backchannel_client_notification_endpoint",
                            "https://localhost:8080/pushNotificationEdp"});
#line 48
 testRunner.And("execute HTTP POST JSON request \'https://localhost:8080/register\'", ((string)(null)), table172, "And ");
#line hidden
#line 59
 testRunner.And("extract JSON from body", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 60
 testRunner.And("extract parameter \'client_id\' from JSON body", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
                TechTalk.SpecFlow.Table table173 = new TechTalk.SpecFlow.Table(new string[] {
                            "Key",
                            "Value"});
                table173.AddRow(new string[] {
                            "X-Testing-ClientCert",
                            "mtlsClient.crt"});
                table173.AddRow(new string[] {
                            "client_id",
                            "$client_id$"});
                table173.AddRow(new string[] {
                            "login_hint",
                            "administrator"});
                table173.AddRow(new string[] {
                            "scope",
                            "openid profile"});
                table173.AddRow(new string[] {
                            "client_notification_token",
                            "7dc3061e-bad9-4817-bd33-8db789bfb516"});
#line 62
 testRunner.And("execute HTTP POST JSON request \'https://localhost:8080/mtls/bc-authorize\'", ((string)(null)), table173, "And ");
#line hidden
#line 70
 testRunner.And("extract JSON from body", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 71
 testRunner.And("extract parameter \'auth_req_id\' from JSON body", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 73
 testRunner.And("confirm authorization request \'$auth_req_id$\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 74
 testRunner.And("poll until \'callbackResponse\' is received", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 75
 testRunner.And("extract JSON from \'callbackResponse\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 77
 testRunner.Then("JSON contains \'auth_req_id\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
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
                BCAuthorizeFeature.FeatureSetup();
            }
            
            void System.IDisposable.Dispose()
            {
                BCAuthorizeFeature.FeatureTearDown();
            }
        }
    }
}
#pragma warning restore
#endregion