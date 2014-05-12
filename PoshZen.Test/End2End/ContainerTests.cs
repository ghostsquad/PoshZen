using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoshZen.Test.End2End
{
    using System.IO;

    using FluentAssertions;

    using Newtonsoft.Json;

    using Ploeh.AutoFixture;
    using Ploeh.AutoFixture.AutoMoq;

    using RestSharp;

    using Xunit;

    public class ContainerTests
    {
        IFixture fixture = new Fixture().Customize(new AutoMoqCustomization());

        [Fact]
        public void CanResolveClientWithRegisteredSettingsFile()
        {                        
            var actualSettingsPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "PoshZen/RegisteredAccounts.json");

            if (File.Exists(actualSettingsPath))
            {
                File.Delete(actualSettingsPath);
            }

            var settings = this.fixture.Create<ClientSettings>();
            settings.ZendeskUser = settings.ZendeskUser.Protect();
            settings.ZendeskSecret = settings.ZendeskSecret.Protect();
            var serialized = JsonConvert.SerializeObject(settings);
            File.WriteAllText(actualSettingsPath, serialized);            

            // act            
            var client = PoshZenContainer.Default.ResolveClient();

            client.Should().NotBeNull();
        }
    }
}
