namespace PoshZen.Test
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;   
    using System.Management.Automation;
    using System.Management.Automation.Runspaces;

    using FluentAssertions;

    using Xunit;

    public class GetTicketCmdletTests
    {
        private RunspaceConfiguration config;

        public GetTicketCmdletTests()
        {
            config = RunspaceConfiguration.Create();
            config.Cmdlets.Append(new CmdletConfigurationEntry(
                "Get-MSIFileHash",
                typeof(GetTicketCmdlet),
                "Microsoft.Windows.Installer.PowerShell.dll-Help.xml"));
        }

        [Fact]
        public void CanGetTicket()
        {
            using (Runspace rs = RunspaceFactory.CreateRunspace(config))
            {
                rs.Open();
                using (Pipeline p = rs.CreatePipeline(@"get-ticket -Client example.txt"))
                {
                    Collection<PSObject> objs = p.Invoke();
                    objs.Count.Should().Be(1);
                }
            }
        }
    }
}
