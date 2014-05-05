namespace PoshZen
{
    using System;
    using System.IO;

    using Newtonsoft.Json;

    using SharpZendeskApi.Core;
    using SharpZendeskApi.Core.Management;
    using SharpZendeskApi.Core.Models;

    internal class PoshZenContainer
    {
        #region Static Fields

        private static readonly object syncRoot = new object();

        private static volatile PoshZenContainer instance;

        #endregion

        #region Fields

        private IEnvironment environment;

        private string settingsJsonPath;

        #endregion

        #region Constructors and Destructors

        public PoshZenContainer(IEnvironment environment)
        {
            this.environment = environment;

            var poshZenAppDataFolder = Path.Combine(environment.ApplicationDataFolder, "PoshZen");
            environment.CreateDirectoryIfNotExists(poshZenAppDataFolder);
            this.settingsJsonPath = Path.Combine(poshZenAppDataFolder, "RegisteredAccounts.json");

            string settingsJson = null;

            if (File.Exists(this.settingsJsonPath))
            {
                settingsJson = environment.ReadFile(this.settingsJsonPath);
            }

            if (settingsJson != null)
            {
                this.Settings = JsonConvert.DeserializeObject<ClientSettings>(settingsJson);
            }
            else
            {
                throw new InvalidOperationException(this.settingsJsonPath + " file is corrupt!");
            }
        }

        #endregion

        #region Public Properties

        public static PoshZenContainer Default
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new PoshZenContainer(WindowsEnvironment.Default);
                        }
                    }
                }

                return instance;
            }
        }

        public IZendeskClient Client { get; set; }

        public ClientSettings Settings { get; set; }

        #endregion

        #region Public Methods and Operators

        public IZendeskClient ResolveClient()
        {
            return this.Client
                   ?? (this.Client =
                       new ZendeskClient(
                           this.Settings.Domain, 
                           this.Settings.ZendeskUser.Unprotect(), 
                           this.Settings.ZendeskSecret.Unprotect(), 
                           this.Settings.CredentialType));
        }

        public IManager<ITicket> ResolveTicketManager()
        {
            return new TicketManager(this.Client);
        }

        public void WriteJsonSettings(ClientSettings settings)
        {
            this.Settings = settings;
            var serializedSettings = JsonConvert.SerializeObject(this.Settings);
            this.environment.WriteContents(serializedSettings);
        }

        #endregion
    }
}