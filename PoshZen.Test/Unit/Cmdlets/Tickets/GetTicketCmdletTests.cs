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
        public GetTicketCmdletTests()
        {
            this.environmentMock.Setup(x => x.ApplicationDataFolder).Returns(AppDomain.CurrentDomain.BaseDirectory);
        }

        [Fact]
        public void CanGetTicket()
        {
            var expectedTicket = this.fixture.Create<ITicket>();            

            int? actualId;
            var managerMock = new Mock<ITicketManager>();
            managerMock.Setup(x => x.Get(It.IsAny<int>()))
                .Returns(expectedTicket)
                .Callback<int>(x => actualId = x);

            this.Glue(managerMock);

            var invocationData = this.Invoke("Get-Ticket 1");

            invocationData.Results.Should().HaveCount(1);
            invocationData.ErrorRecords.Should().BeEmpty();
            invocationData.Results[0].BaseObject.Should().Be(expectedTicket);
        }
    }
}
