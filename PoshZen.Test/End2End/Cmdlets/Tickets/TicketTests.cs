namespace PoshZen.Test.End2End.Cmdlets.Tickets
{
    using System.Collections.Generic;
    using System.Management.Automation;

    using FluentAssertions;

    using SharpZendeskApi;
    using SharpZendeskApi.Models;

    using Xunit;
    using Xunit.Should;

    public class TicketTests : ScriptCsCmdletTestBase
    {
        [Fact]
        public void CanGetTicketFromKnownEndPoint()
        {
            // arrange
            var client = TestHelpers.GetClient(ZendeskAuthenticationMethod.Basic);

            var command = new PSCommand();
            command.AddCommand("Get-Ticket");
            command.AddParameter("Client", client);
            command.AddParameter("Id", 1);            

            // act
            var invocationData = this.Invoke(command);

            // assert
            invocationData.Results.Should().HaveCount(1);
            invocationData.ErrorRecords.Should().BeEmpty();
            var actualTicket = invocationData.Results[0].BaseObject.As<ITicket>();
            actualTicket.Id.Should().Be(1);
        }

        [Fact]
        public void CanCreateTicketToKnownEndPoint()
        {
            // arrange
            var client = TestHelpers.GetClient(ZendeskAuthenticationMethod.Basic);

            var ticket = new Ticket(684991558, "this is a test from PoshZen!");

            //var variables = new Dictionary<string, object> { { "ticket", ticket } };

            var command = new PSCommand();            
            command.AddCommand("New-Ticket");
            command.AddParameter("Ticket", ticket);
            command.AddParameter("Client", client);

            var invocationData = this.Invoke(command);

            // assert
            invocationData.Results.Should().HaveCount(1);
            invocationData.ErrorRecords.Should().BeEmpty();
            var actualTicket = invocationData.Results[0].BaseObject.As<ITicket>();
            actualTicket.Id.ShouldNotBeNull();
        }

        [Fact]
        public void CanCreateTicketToKnownEndPointUsingPipeline()
        {
            // arrange
            var client = TestHelpers.GetClient(ZendeskAuthenticationMethod.Basic);

            var ticket = new Ticket(684991558, "this is a test from PoshZen!");

            var variables = new Dictionary<string, object> { { "ticket", ticket } };

            var command = new PSCommand();            
            command.AddScript("$ticket");
            command.AddCommand("New-Ticket");            
            command.AddParameter("Client", client);

            var invocationData = this.Invoke(command, variables);

            // assert
            invocationData.Results.Should().HaveCount(1);
            invocationData.ErrorRecords.Should().BeEmpty();
            var actualTicket = invocationData.Results[0].BaseObject.As<ITicket>();
            actualTicket.Id.ShouldNotBeNull();
        }
    }
}
