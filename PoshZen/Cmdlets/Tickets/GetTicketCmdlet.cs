namespace PoshZen.Cmdlets.Tickets
{
    using System.Management.Automation;

    using SharpZendeskApi;
    using SharpZendeskApi.Management;
    using SharpZendeskApi.Models;

    [Cmdlet(VerbsCommon.Get, CmdletNamingConstants.Ticket, DefaultParameterSetName = ParamSetFromId)]
    public class GetTicketCmdlet : PoshZenCmdletBase<ITicket, ITicketManager>
    {
        private const string ParamSetFromId = "FromId";

        #region Public Properties

        [Parameter(Position = 1, ValueFromPipeline = true)]
        public override ZendeskClientBase Client { get; set; }

        [Parameter(Position = 0, Mandatory = true)]      
        [ValidateNotNull]
        public int Id { get; set; }

        #endregion

        #region Methods        

        protected override void BeginProcessing()
        {            
            this.ResolveManager();
        }

        protected override void ProcessRecord()
        {            
            this.WriteObject(this.Manager.Get(this.Id));
        }

        #endregion
    }
}