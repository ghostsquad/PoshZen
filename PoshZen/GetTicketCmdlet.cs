namespace PoshZen
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Management.Automation;
    using System.Text;
    using System.Threading.Tasks;
    using SharpZendeskApi.Core;

    [Cmdlet("Get", "Ticket")]
    public class GetTicketCmdlet : Cmdlet
    {
        protected override void EndProcessing()
        {
            base.EndProcessing();
        }

        protected override void ProcessRecord()
        {
            base.ProcessRecord();            
        }

        protected override void BeginProcessing()
        {
            base.BeginProcessing();
        }
    }
}
