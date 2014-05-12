namespace PoshZen.Test.End2End.Cmdlets
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Management.Automation;
    using System.Text;
    using System.Threading.Tasks;

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
            var ExpectedFilePath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "PoshZen/RegisteredAccounts.json");

            if (File.Exists(ExpectedFilePath))
            {
                File.Delete(ExpectedFilePath);
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
            var results = this.Invoke(cmd);

            // assert
            File.Exists(ExpectedFilePath).Should().BeTrue();
            var contents = File.ReadAllText(ExpectedFilePath);

            var settings = JsonConvert.DeserializeObject<ClientSettings>(contents);
            settings.ZendeskUser.Unprotect().Should().Be(ExpectedUsername);
            settings.ZendeskSecret.Unprotect().Should().Be(ExpectedPassword);
            settings.CredentialType.Should().Be(ZendeskAuthenticationMethod.Basic);
            settings.DisplayName.Should().NotBeNull();
            settings.Domain.Should().Be(ExpectedDomain);
        }
    }
}
