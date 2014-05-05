namespace PoshZen
{
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Management.Automation;
    using System.Management.Automation.Runspaces;

    [RunInstaller(true)]
    public class PoshZenPSSnapIn : CustomPSSnapIn
    {
        #region Fields

        private Collection<CmdletConfigurationEntry> cmdlets = new Collection<CmdletConfigurationEntry>();

        private Collection<FormatConfigurationEntry> formats = new Collection<FormatConfigurationEntry>();

        private Collection<ProviderConfigurationEntry> providers = new Collection<ProviderConfigurationEntry>();

        private Collection<TypeConfigurationEntry> types = new Collection<TypeConfigurationEntry>();

        #endregion

        #region Constructors and Destructors

        public PoshZenPSSnapIn()
        {
            this.cmdlets.Add(new CmdletConfigurationEntry("get-ticket", typeof(GetTicketCmdlet), null));
        }

        #endregion

        #region Public Properties

        public override Collection<CmdletConfigurationEntry> Cmdlets
        {
            get
            {
                return this.cmdlets;
            }
        }

        public override string Description
        {
            get
            {
                return "This snap-in contains the cmdlets for SharpZendeskApi functions.";
            }
        }

        public override Collection<FormatConfigurationEntry> Formats
        {
            get
            {
                return this.formats;
            }
        }

        public override string Name
        {
            get
            {
                return "PoshZenPSSnapIn";
            }
        }

        public override Collection<ProviderConfigurationEntry> Providers
        {
            get
            {
                return this.providers;
            }
        }

        public override Collection<TypeConfigurationEntry> Types
        {
            get
            {
                return this.types;
            }
        }

        public override string Vendor
        {
            get
            {
                return "Weston McNamee";
            }
        }

        #endregion
    }
}