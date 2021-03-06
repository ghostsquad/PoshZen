﻿namespace PoshZen.Test.Integration.Cmdlets
{
    using System;
    using System.IO;
    using System.Management.Automation;

    using FluentAssertions;

    using Newtonsoft.Json;

    using SharpZendeskApi;

    using Xunit;

    public class SetZendeskCredentialsCmdletTests : ScriptCsCmdletTestBase
    {
        [Fact]
        public void GivenCredsExpectJsonFilePopulated()
        {
            // arrange
            var expectedFilePath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "PoshZen/RegisteredAccounts.json");

            if (File.Exists(expectedFilePath))
            {
                File.Delete(expectedFilePath);
            }            

            const string ExpectedDomain = "domain123";
            const string ExpectedUsername = "user123";
            const string ExpectedPassword = "password123";

            var cmd = new PSCommand();
            cmd.AddCommand("Set-ZendeskCredentials");
            cmd.AddParameter("Domain", ExpectedDomain);
            cmd.AddParameter("UserName", ExpectedUsername);
            cmd.AddParameter("Password", ExpectedPassword);            

            // act           
            this.Invoke(cmd);

            // assert
            File.Exists(expectedFilePath).Should().BeTrue();
            var contents = File.ReadAllText(expectedFilePath);

            var settings = JsonConvert.DeserializeObject<ClientSettings>(contents);
            settings.ZendeskUser.Unprotect().Should().Be(ExpectedUsername);
            settings.ZendeskSecret.Unprotect().Should().Be(ExpectedPassword);
            settings.CredentialType.Should().Be(ZendeskAuthenticationMethod.Basic);
            settings.DisplayName.Should().NotBeNull();
            settings.Domain.Should().Be(ExpectedDomain);
        }
    }
}
