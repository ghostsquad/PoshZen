namespace PoshZen.Test.Unit.Cmdlets.Tickets
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Management.Automation;
    using System.Text;
    using System.Threading.Tasks;

    using FluentAssertions;

    using Moq;

    using Ploeh.AutoFixture;

    using SharpZendeskApi.Management;
    using SharpZendeskApi.Models;

    using Xunit;

    public class NewTicketCmdletTests : CmdletTestBase<Ticket, ITicket, ITicketManager>
    {
        [Fact]
        public void NewTicket_NoParams_ExpectSubmitNewCalled()
        {
            var ticketInput = new Ticket(1, "foo");

            var expectedTicket = this.Fixture.Create<ITicket>();
            this.ManagerMock.Setup(x => x.SubmitNew(It.IsAny<ITicket>()))
                .Returns(expectedTicket)
                .Verifiable();

            var psCommand = new PSCommand();
            psCommand.AddCommand("New-Ticket");
            psCommand.AddArgument(ticketInput);

            // act
            var invocationData = this.Invoke(psCommand);

            invocationData.Results.Should().HaveCount(1);
            invocationData.ErrorRecords.Should().BeEmpty();
            this.ManagerMock.Verify(x => x.SubmitNew(ticketInput), Times.Once());
            invocationData.Results[0].BaseObject.Should().BeSameAs(expectedTicket);
        }                
    }
}
