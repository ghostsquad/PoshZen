namespace PoshZen.Test
{
    using System;

    using FluentAssertions;

    using Microsoft.Practices.Unity;

    using Moq;

    using Ploeh.AutoFixture;

    using SharpZendeskApi;
    using SharpZendeskApi.Management;
    using SharpZendeskApi.Models;   

    using Xunit;

    public class GetTicketCmdletTests : ScriptCsCmdletTestBase, IUseFixture<ManagementFixture>
    {
        private readonly Mock<IEnvironment> environmentMock = new Mock<IEnvironment>();

        private ManagementFixture managementFixture;
        
        [Fact]
        public void CanGetTicket()
        {
            var expectedTicket = this.managementFixture.Fixture.Create<ITicket>();

            this.environmentMock.Setup(x => x.ApplicationDataFolder).Returns(AppDomain.CurrentDomain.BaseDirectory);

            int? actualId;
            var managerMock = new Mock<IManager<ITicket>>();
            managerMock.Setup(x => x.Get(It.IsAny<int>()))
                .Returns(expectedTicket)
                .Callback<int>(x => actualId = x);            

            this.managementFixture.UnityContainer.RegisterInstance(managerMock.Object);

            new PoshZenContainer(this.environmentMock.Object, this.managementFixture.UnityContainer);
            PoshZenContainer.Default.Client = Mock.Of<IZendeskClient>();

            var invocationData = Invoke("Get-Ticket 1");

            invocationData.Results.Should().HaveCount(1);
            invocationData.Results[0].BaseObject.Should().Be(expectedTicket);
        }

        public void SetFixture(ManagementFixture data)
        {
            this.managementFixture = data;
        }
    }
}
