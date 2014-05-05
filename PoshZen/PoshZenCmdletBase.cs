namespace PoshZen
{
    using System;
    using System.Management.Automation;

    using SharpZendeskApi.Core;

    public abstract class PoshZenCmdletBase : Cmdlet
    {
        #region Public Properties

        public abstract IZendeskClient Client { get; set; }

        #endregion

        #region Methods

        protected void ThrowIfUnableToObtainClient()
        {
            if (this.Client != null)
            {
                return;
            }

            this.Client = PoshZenContainer.Default.ResolveClient();
            if (this.Client == null)
            {
                throw new InvalidOperationException("No client specified or obtained from persisted/shell defaults.");
            }
        }

        #endregion
    }
}