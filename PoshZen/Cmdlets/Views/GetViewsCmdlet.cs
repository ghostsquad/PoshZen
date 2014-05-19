namespace PoshZen.Cmdlets.Views
{
    using System.Collections.Generic;
    using System.Management.Automation;

    using SharpZendeskApi;
    using SharpZendeskApi.Management;
    using SharpZendeskApi.Models;

    [Cmdlet(VerbsCommon.Get, CmdletNamingConstants.Views, DefaultParameterSetName = ParamSetDefault)]
    public class GetViewsCmdlet : PoshZenCmdletBase<IView, ViewManager>
    {
        private const string ParamSetDefault = "default";

        private const string ParamSetActive = "active";        

        [Parameter(ValueFromPipeline = true, ParameterSetName = ParamSetDefault)]
        [Parameter(ValueFromPipeline = true, ParameterSetName = ParamSetActive)]
        public override ZendeskClientBase Client { get; set; }

        [Parameter(ParameterSetName = ParamSetDefault)]
        public SwitchParameter Full { get; set; }

        [Parameter(ParameterSetName = ParamSetActive)]        
        public SwitchParameter Active { get; set; }

        protected override void BeginProcessing()
        {
            this.ResolveManager();
        }

        protected override void ProcessRecord()
        {
            IEnumerable<IView> views = null;            

            switch (this.ParameterSetName)
            {
                case ParamSetDefault:
                    {
                        views = this.Manager.GetAvailableViews(this.Full);
                        break;
                    }
                case ParamSetActive:
                    {
                        views = this.Manager.GetActiveViews();
                        break;
                    }
            }

            if (views == null)
            {
                views = new IView[0];
            }

            this.WriteObject(views);
        }
    }
}
