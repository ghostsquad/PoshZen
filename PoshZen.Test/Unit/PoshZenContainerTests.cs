namespace PoshZen.Test.Unit
{
    using System;
    using System.IO;

    using FluentAssertions;

    using Moq;

    using Ploeh.AutoFixture;
    using Ploeh.AutoFixture.AutoMoq;

    using Xunit;

    public class PoshZenContainerTests : IUseFixture<ContainerFixture>
    {
        private IFixture fixture = new Fixture().Customize(new AutoMoqCustomization());

        private ContainerFixture containerFixture;

        [Fact]
        public void CanWriteJsonSettings()
        {
            var environmentMock = new Mock<IEnvironment>();
            environmentMock.Setup(x => x.ApplicationDataFolder).Returns(string.Empty);

            string actualSettings = null;
            string actualPathToSaveTo = null;
            environmentMock.Setup(x => x.WriteContents(It.IsAny<string>(), It.IsAny<string>()))
                .Callback<string, string>((x, y) =>
                {
                    actualPathToSaveTo = x; 
                    actualSettings = y;                                   
                });

            var poshZenContainer = PoshZenContainer.Create(environmentMock.Object, null);
            poshZenContainer.WriteJsonSettings(this.containerFixture.Settings);

            actualSettings.Should().Be(this.containerFixture.ExpectedSettings);
            actualPathToSaveTo.Should().Be(@"PoshZen\RegisteredAccounts.json");
        }

        [Fact]
        public void CanGetJsonSettings()
        {
            var environmentMock = new Mock<IEnvironment>();
            environmentMock.Setup(x => x.ApplicationDataFolder).Returns(AppDomain.CurrentDomain.BaseDirectory);
            environmentMock.Setup(x => x.FileExists(It.IsAny<string>())).Returns(true).Verifiable();
            environmentMock.Setup(x => x.ReadFile(It.IsAny<string>())).Returns(this.containerFixture.ExpectedSettings).Verifiable();

            var expectedPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"PoshZen\RegisteredAccounts.json");

            var poshZenContainer = PoshZenContainer.Create(environmentMock.Object, null);


            environmentMock.Verify(x => x.FileExists(expectedPath), Times.Once());
            environmentMock.Verify(x => x.ReadFile(expectedPath), Times.Once());

            poshZenContainer.Settings.CredentialType.Should().Be(this.containerFixture.Settings.CredentialType);
            poshZenContainer.Settings.DisplayName.Should().Be(this.containerFixture.Settings.DisplayName);
            poshZenContainer.Settings.Domain.Should().Be(this.containerFixture.Settings.Domain);
            poshZenContainer.Settings.ZendeskUser.Should().Be(this.containerFixture.Settings.ZendeskUser);
            poshZenContainer.Settings.ZendeskSecret.Should().Be(this.containerFixture.Settings.ZendeskSecret);
        }

        public void SetFixture(ContainerFixture data)
        {
            this.containerFixture = data;
        }
    }
}
