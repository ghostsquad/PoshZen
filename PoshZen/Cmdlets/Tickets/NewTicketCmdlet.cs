namespace PoshZen.Cmdlets.Tickets
{
    using System;
    using System.Management.Automation;

    using SharpZendeskApi;
    using SharpZendeskApi.Models;

    [Cmdlet(VerbsCommon.New, CmdletNamingConstants.Ticket, DefaultParameterSetName = ParamSetDefault)]
    public class NewTicketCmdlet : PoshZenCmdletBase<ITicket>
    {
        private const string ParamSetDefault = "default";

        [Parameter(Position = 1)]
        public override IZendeskClient Client { get; set; }

        [Parameter(Position = 0, ValueFromPipeline = true, ParameterSetName = ParamSetDefault, Mandatory = true)]       
        public ITicket Ticket { get; set; }

        protected override void BeginProcessing()
        {
            this.ResolveManager();
        }

        protected override void ProcessRecord()
        {
            if (this.Ticket == null)
            {
                throw new ArgumentNullException("Ticket");
            }

            this.WriteObject(this.Manager.SubmitNew(this.Ticket));
        }
    }
}
