namespace PoshZen.Cmdlets.Tickets
{
    using System.Management.Automation;

    using SharpZendeskApi;
    using SharpZendeskApi.Management;
    using SharpZendeskApi.Models;

    [Cmdlet("Update", CmdletNamingConstants.Ticket, DefaultParameterSetName = ParamSetDefault)]
    public class UpdateTicketCmdlet : PoshZenCmdletBase<ITicket, ITicketManager>
    {
        private const string ParamSetDefault = "default";

        [Parameter(Position = 1)]
        public override ZendeskClientBase Client { get; set; }

        [Parameter(Position = 0, ValueFromPipeline = true, ParameterSetName = ParamSetDefault, Mandatory = true)]       
        public ITicket Ticket { get; set; }

        #region Methods        

        protected override void BeginProcessing() {            
            this.ResolveManager();
        }

        protected override void ProcessRecord() {
            this.Manager.SubmitUpdatesFor(this.Ticket);
        }

        #endregion
    }
}