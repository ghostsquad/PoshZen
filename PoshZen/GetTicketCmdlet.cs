namespace PoshZen
{
    using System.Management.Automation;

    using SharpZendeskApi;
    using SharpZendeskApi.Models;

    [Cmdlet(VerbsCommon.Get, "Ticket")]
    public class GetTicketCmdlet : PoshZenCmdletBase<ITicket>
    {        
        #region Public Properties

        [Parameter(Position = 1, ValueFromPipeline = true)]
        public override IZendeskClient Client { get; set; }

        [Parameter(Position = 0)]
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