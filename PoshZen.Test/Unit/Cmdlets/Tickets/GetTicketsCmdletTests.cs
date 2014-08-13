using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FluentAssertions;

using Microsoft.Practices.Unity;

using Moq;

using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;

using SharpZendeskApi;
using SharpZendeskApi.Management;
using SharpZendeskApi.Models;

using Xunit.Should;

namespace PoshZen.Test.Unit.Cmdlets.Tickets {
    using Xunit;

    public class GetTicketsCmdletTests : ScriptCsCmdletTestBase {
        public GetTicketsCmdletTests() {
            this.environmentMock.Setup(x => x.ApplicationDataFolder).Returns(AppDomain.CurrentDomain.BaseDirectory);
        }

        [Fact]
        public void GivenViewIdExpectIdPassedToTicketManager() {
            const int ExpectedViewId = 1;

            int? actualViewId = null;
            var managerMock = new Mock<ITicketManager>();
            managerMock.Setup(x => x.FromView(It.IsAny<int>())).Callback<int>(x => actualViewId = x);

            this.Glue(managerMock);

            var invocationData = this.Invoke("Get-Tickets -ViewId " + ExpectedViewId);

            invocationData.ErrorRecords.Should().BeEmpty();
            actualViewId.Should().Be(ExpectedViewId);
        }

        [Fact]
        public void CanGetAllTickets() {
            const int ExpectedTicketCount = 2;
            var expectedTickets = this.fixture.CreateMany<ITicket>(ExpectedTicketCount).ToList();

            var listingMock = new Mock<IListing<ITicket>>();
            listingMock.Setup(x => x.GetEnumerator()).Returns(expectedTickets.GetEnumerator());

            var managerMock = new Mock<ITicketManager>();
            managerMock.Setup(x => x.FromView(It.IsAny<int>())).Returns(listingMock.Object);

            this.Glue(managerMock);

            var invocationData = this.Invoke("Get-Tickets -ViewId 1");

            invocationData.Results.Count.Should().Be(ExpectedTicketCount);
            invocationData.ErrorRecords.Should().BeEmpty();
            for (var i = 0; i < invocationData.Results.Count; i++) {
                invocationData.Results[i].BaseObject.Should().Be(expectedTickets[i]);
            }
        }        
    }
}
