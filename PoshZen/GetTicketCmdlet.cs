namespace PoshZen
{
    using System.Management.Automation;

    using SharpZendeskApi.Core;

    [Cmdlet(VerbsCommon.Get, "Ticket")]
    public class GetTicketCmdlet : PoshZenCmdletBase
    {
        #region Fields

        private bool force;

        #endregion

        #region Constructors and Destructors

        public GetTicketCmdlet()
        {
            this.ThrowIfUnableToObtainClient();
        }

        #endregion

        #region Public Properties

        [Parameter(Position = 1, ValueFromPipeline = true)]
        public override IZendeskClient Client { get; set; }

        [Parameter(Position = 2)]
        public SwitchParameter Force
        {
            get
            {
                return this.force;
            }

            set
            {
                this.force = value;
            }
        }

        [Parameter(Position = 0, Mandatory = true)]
        public int Id { get; set; }

        #endregion

        #region Methods

        protected override void ProcessRecord()
        {
            this.WriteObject(PoshZenContainer.Default.ResolveTicketManager().Get(this.Id, this.Force));
        }

        #endregion
    }
}