namespace PoshZen.Test
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;   
    using System.Management.Automation;
    using System.Management.Automation.Runspaces;

    using FluentAssertions;

    using PoshZen.Exceptions;

    using Xunit;

    public class GetTicketCmdletTests
    {
        private RunspaceConfiguration config;

        [Fact]
        public void ShouldCreateCmdLet()
        {
            var cmd = new GetTicketCmdlet();
            cmd.Should().BeAssignableTo<Cmdlet>();
        }

        [Fact]
        public void WhenClientNotProvidedExpectPoshZenException()
        {
            var cmd = new GetTicketCmdlet { Id = 1 };
            var cmdEnumerator = cmd.Invoke().GetEnumerator();

            cmdEnumerator.Invoking(x => x.MoveNext()).ShouldThrow<PoshZenException>();
        }
    }
}
