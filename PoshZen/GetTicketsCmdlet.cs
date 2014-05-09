namespace PoshZen
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Management.Automation;

    using SharpZendeskApi;
    using SharpZendeskApi.Management;
    using SharpZendeskApi.Models;

    [Cmdlet(VerbsCommon.Get, "Tickets", DefaultParameterSetName = ParamSetIdList)]
    public class GetTicketsCmdlet : PoshZenCmdletBase<ITicket>
    {
        #region Constants

        private const string ParamSetIdList = "IdList";

        private const string ParamSetView = "View";

        private const string ParamSetViewId = "ViewId";
        
        #endregion

        #region Fields       

        #endregion

        #region Public Properties

        [Parameter(Position = 1)]
        [ValidateNotNull]
        public override IZendeskClient Client { get; set; }

        [Parameter(Position = 0, ParameterSetName = ParamSetIdList)]
        [ValidateNotNullOrEmpty]
        public int[] Ids { get; set; }

        [Parameter(Position = 0, ParameterSetName = ParamSetView)]
        [ValidateNotNull]
        public View View { get; set; }

        [Parameter(Position = 0, ParameterSetName = ParamSetViewId)]
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
                case ParamSetIdList:
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