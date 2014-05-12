namespace PoshZen.Test.Unit.Cmdlets.Tickets
{
    using System;

    using FluentAssertions;

    using Microsoft.Practices.Unity;

    using Moq;

    using Ploeh.AutoFixture;
    using Ploeh.AutoFixture.AutoMoq;

    using SharpZendeskApi;
    using SharpZendeskApi.Management;
    using SharpZendeskApi.Models;

    using Xunit;

    public class GetTicketCmdletTests : ScriptCsCmdletTestBase
    {
        private readonly Mock<IEnvironment> environmentMock = new Mock<IEnvironment>();

        private IUnityContainer container = new UnityContainer();

        private IFixture fixture = new Fixture().Customize(new AutoMoqCustomization());
        
        [Fact]
        public void CanGetTicket()
        {
            var expectedTicket = this.fixture.Create<ITicket>();

            this.environmentMock.Setup(x => x.ApplicationDataFolder).Returns(AppDomain.CurrentDomain.BaseDirectory);

            int? actualId;
            var managerMock = new Mock<IManager<ITicket>>();
            managerMock.Setup(x => x.Get(It.IsAny<int>()))
                .Returns(expectedTicket)
                .Callback<int>(x => actualId = x);

            this.container.RegisterType<ITicket, Ticket>();
            this.container.RegisterInstance<IManager<ITicket>>(managerMock.Object);

            var poshZenContainer = PoshZenContainer.Create(this.environmentMock.Object, this.container);
            poshZenContainer.Client = Mock.Of<IZendeskClient>();

            var invocationData = this.Invoke("Get-Ticket 1");

            invocationData.Results.Should().HaveCount(1);
            invocationData.ErrorRecords.Should().HaveCount(0);
            invocationData.Results[0].BaseObject.Should().Be(expectedTicket);
        }
    }
}
