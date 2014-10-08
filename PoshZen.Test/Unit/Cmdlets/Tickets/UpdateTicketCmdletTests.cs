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

    public class UpdateTicketCmdletTests : CmdletTestBase<Ticket, ITicket, ITicketManager>
    {
        [Fact]
        public void UpdateTicket_ExpectSubmitUpdatesCalledWithProvidedITicket()
        {
            var ticketInput = new Ticket(1, "foo");

            this.ManagerMock.Setup(x => x.SubmitUpdatesFor(It.IsAny<ITicket>()))
                .Verifiable();

            var psCommand = new PSCommand();
            psCommand.AddCommand("Update-Ticket");
            psCommand.AddArgument(ticketInput);

            // act
            var invocationData = this.Invoke(psCommand);

            invocationData.Results.Should().BeEmpty();
            invocationData.ErrorRecords.Should().BeEmpty();
            this.ManagerMock.Verify(x => x.SubmitUpdatesFor(ticketInput), Times.Once());
        }                
    }
}
