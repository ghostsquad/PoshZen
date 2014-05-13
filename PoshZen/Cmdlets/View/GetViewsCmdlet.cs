using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoshZen.Cmdlets.View
{
    using System.Management.Automation;

    using SharpZendeskApi;
    using SharpZendeskApi.Management;
    using SharpZendeskApi.Models;

    [Cmdlet(VerbsCommon.Get, CmdletNamingConstants.Views, DefaultParameterSetName = ParamSetDefault)]
    public class GetViewsCmdlet : PoshZenCmdletBase<IView>
    {
        private const string ParamSetDefault = "default";

        private const string ParamSetActive = "active";        

        [Parameter(ValueFromPipeline = true, ParameterSetName = ParamSetDefault)]
        [Parameter(ValueFromPipeline = true, ParameterSetName = ParamSetActive)]
        public override IZendeskClient Client { get; set; }

        [Parameter(ParameterSetName = ParamSetDefault)]
        public SwitchParameter Full { get; set; }

        [Parameter(ParameterSetName = ParamSetActive)]        
        public SwitchParameter Active { get; set; }

        protected override void BeginProcessing()
        {
            this.ResolveClient();
        }

        protected override void ProcessRecord()
        {
            IEnumerable<IView> views = null;
            var viewManager = PoshZenContainer.Default.ResolveManager<IView>(this.Client) as ViewManager;            

            switch (this.ParameterSetName)
            {
                case ParamSetDefault:
                    {
                        views = viewManager.GetAvailableViews(this.Full);
                        break;
                    }
                case ParamSetActive:
                    {
                        views = viewManager.GetActiveViews();
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
