namespace PoshZen
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Management.Automation;
    using System.Text;
    using System.Threading.Tasks;

    using SharpZendeskApi.Core;
    using SharpZendeskApi.Core.Management;
    using SharpZendeskApi.Core.Models;

    [Cmdlet("Get", "Tickets")]
    public class GetTicketsCmdlet : Cmdlet
    {

        #region Fields

        private TicketManager manager;

        #endregion

        #region Public Properties

        [Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true, ParameterSetName = "Ids")]
        [Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true, ParameterSetName = "FromViewId")]
        [Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true, ParameterSetName = "FromView")]
        public IZendeskClient Client { get; set; }

        [Parameter(Position = 1, Mandatory = true, ParameterSetName = "Ids")]
        public int[] Ids { get; set; }

        [Parameter(Position = 1, Mandatory = true, ParameterSetName = "FromViewId")]
        public int? ViewId { get; set; }

        [Parameter(Position = 1, Mandatory = true, ParameterSetName = "FromView")]
        public View View { get; set; }

        #endregion

        #region Methods

        protected override void BeginProcessing()
        {
            this.manager = new TicketManager(this.Client);
        }

        protected override void ProcessRecord()
        {
            IEnumerable<ITicket> tickets = null;

            if (this.Ids != null && this.Ids.Count() > 0)
            {
                tickets = this.manager.GetMany(this.Ids);
            }
            else if (this.ViewId.HasValue)
            {
                tickets = this.manager.FromView(this.ViewId.Value);
            }
            else if (this.View != null)
            {
                tickets = this.manager.FromView(this.View);                
            }

            this.WriteObject(tickets ?? new ITicket[0]);
        }

        #endregion
    }
}
