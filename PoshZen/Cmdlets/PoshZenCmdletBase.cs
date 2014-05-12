namespace PoshZen.Cmdlets
{
    using System.Management.Automation;

    using PoshZen.Exceptions;

    using SharpZendeskApi;
    using SharpZendeskApi.Management;
    using SharpZendeskApi.Models;

    public abstract class PoshZenCmdletBase<TInterface> : PSCmdlet
        where TInterface : IZendeskThing, ITrackable
    {
        #region Public Properties

        public abstract IZendeskClient Client { get; set; }

        protected IManager<TInterface> Manager { get; set; }

        #endregion

        #region Methods

        internal void ResolveManager()
        {
            this.ResolveClient();
            this.Manager = PoshZenContainer.Default.ResolveManager<TInterface>(this.Client);
        }

        protected void ResolveClient()
        {
            if (this.Client != null)
            {
                return;
            }

            this.Client = PoshZenContainer.Default.ResolveClient();
            if (this.Client == null)
            {
                throw new PoshZenException("No client specified or obtained from persisted/shell defaults.");
            }
        }

        #endregion
    }
}