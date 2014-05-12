namespace PoshZen.Cmdlets.Tickets
{
    using System.Collections.Generic;
    using System.Management.Automation;

    using SharpZendeskApi;
    using SharpZendeskApi.Management;
    using SharpZendeskApi.Models;

    [Cmdlet(VerbsCommon.Get, CmdletNamingConstants.Tickets, DefaultParameterSetName = ParamSetDefault)]
    public class GetTicketsCmdlet : PoshZenCmdletBase<ITicket>
    {
        #region Constants

        private const string ParamSetDefault = "default";

        private const string ParamSetView = "View";

        private const string ParamSetViewId = "ViewId";
        
        #endregion

        #region Public Properties

        [Parameter(Position = 1, ParameterSetName = ParamSetDefault)]
        [Parameter(Position = 1, ParameterSetName = ParamSetView)]
        [Parameter(Position = 1, ParameterSetName = ParamSetViewId)]
        [ValidateNotNull]
        public override IZendeskClient Client { get; set; }

        [Parameter(Position = 0, ParameterSetName = ParamSetDefault, Mandatory = true)]
        [ValidateNotNullOrEmpty]
        public int[] Ids { get; set; }

        [Parameter(Position = 0, ParameterSetName = ParamSetView, Mandatory = true)]
        [ValidateNotNull]
        public View View { get; set; }

        [Parameter(Position = 0, ParameterSetName = ParamSetViewId, Mandatory = true)]
        [ValidateNotNull]
        public int ViewId { get; set; }

        #endregion

        #region Methods

        protected override void BeginProcessing()
        {
            this.ResolveManager();
        }

        protected override void ProcessRecord()
        {
            IEnumerable<ITicket> tickets = null;
            var managerAsTicketManager = this.Manager as TicketManager;            

            switch (this.ParameterSetName)
            {
                case ParamSetDefault:
                    {
                        tickets = this.Manager.GetMany(this.Ids);
                        break;
                    }

                case ParamSetView:
                    {
                        tickets = managerAsTicketManager.FromView(this.ViewId);
                        break;
                    }

                case ParamSetViewId:
                    {
                        tickets = managerAsTicketManager.FromView(this.View);
                        break;
                    }
            }

            if (tickets == null)
            {
                tickets = new ITicket[0];
            }

            this.WriteObject(tickets);
        }

        #endregion
    }
}