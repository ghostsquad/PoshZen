namespace PoshZen.Cmdlets.Tickets
{
    using System.Collections.Generic;
    using System.Management.Automation;

    using SharpZendeskApi;
    using SharpZendeskApi.Management;
    using SharpZendeskApi.Models;

    [Cmdlet(VerbsCommon.Get, CmdletNamingConstants.Tickets, DefaultParameterSetName = ParamSetDefault)]
    public class GetTicketsCmdlet : PoshZenCmdletBase<ITicket, ITicketManager>
    {
        #region Constants

        private const string ParamSetDefault = "default";

        private const string ParamSetView = "View";

        private const string ParamSetViewId = "ViewId";
        
        #endregion

        #region Public Properties

        [Parameter(Position = 2, ParameterSetName = ParamSetDefault)]
        [Parameter(Position = 2, ParameterSetName = ParamSetView)]
        [Parameter(Position = 2, ParameterSetName = ParamSetViewId)]
        [ValidateNotNull]
        public override ZendeskClientBase Client { get; set; }

        [Parameter(Position = 0, ParameterSetName = ParamSetDefault, Mandatory = true)]
        [ValidateNotNullOrEmpty]
        public int[] Ids { get; set; }

        [Parameter(Position = 0, ParameterSetName = ParamSetView, Mandatory = true)]
        [ValidateNotNull]
        public View View { get; set; }

        [Parameter(Position = 0, ParameterSetName = ParamSetViewId, Mandatory = true)]
        [ValidateNotNull]
        public int ViewId { get; set; }

        [Parameter(Position = 1, ParameterSetName = ParamSetDefault)]
        [Parameter(Position = 1, ParameterSetName = ParamSetView)]
        [Parameter(Position = 1, ParameterSetName = ParamSetViewId)]
        public SwitchParameter Page { get; set; }

        #endregion

        #region Methods

        protected override void BeginProcessing()
        {
            this.ResolveManager();
        }

        protected override void ProcessRecord()
        {
            IEnumerable<ITicket> tickets = null;          

            switch (this.ParameterSetName)
            {
                case ParamSetDefault:
                    {
                        tickets = this.Manager.GetMany(this.Ids);
                        break;
                    }

                case ParamSetViewId:
                    {
                        tickets = this.Manager.FromView(this.ViewId);
                        break;
                    }

                case ParamSetView:
                    {
                        tickets = this.Manager.FromView(this.View.Id.Value);
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