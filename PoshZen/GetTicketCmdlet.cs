namespace PoshZen
{
    using System.Management.Automation;

    using SharpZendeskApi.Core;
    using SharpZendeskApi.Core.Management;

    [Cmdlet("Get", "Ticket")]
    public class GetTicketCmdlet : Cmdlet
    {
        #region Fields

        private TicketManager manager;

        #endregion

        #region Public Properties

        [Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true)]
        public IZendeskClient Client { get; set; }

        [Parameter(Position = 1, Mandatory = true)]
        public int Id { get; set; }

        #endregion

        #region Methods

        protected override void BeginProcessing()
        {
            this.manager = new TicketManager(this.Client);
        }

        protected override void ProcessRecord()
        {
            this.WriteObject(this.manager.Get(this.Id));
        }

        #endregion
    }
}