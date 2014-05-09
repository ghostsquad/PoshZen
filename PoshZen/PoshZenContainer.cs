namespace PoshZen
{
    using System;
    using System.IO;

    using Microsoft.Practices.Unity;

    using Newtonsoft.Json;

    using SharpZendeskApi;
    using SharpZendeskApi.Management;
    using SharpZendeskApi.Models;   

    internal sealed class PoshZenContainer
    {
        #region Static Fields

        private static readonly object syncRoot = new object();

        private static volatile PoshZenContainer instance;

        #endregion

        #region Constructors and Destructors

        public PoshZenContainer(IEnvironment environment, IUnityContainer container)
        {
            this.Environment = environment;
            this.Container = container;

            var poshZenAppDataFolder = Path.Combine(environment.ApplicationDataFolder, "PoshZen");
            environment.CreateDirectory(poshZenAppDataFolder);
            this.SettingsJsonPath = Path.Combine(poshZenAppDataFolder, "RegisteredAccounts.json");

            if (!environment.FileExists(this.SettingsJsonPath))
            {
                return;
            }

            string settingsJson = environment.ReadFile(this.SettingsJsonPath);
            if (settingsJson != null)
            {
                this.Settings = JsonConvert.DeserializeObject<ClientSettings>(settingsJson);
            }
            else
            {
                throw new InvalidOperationException(this.SettingsJsonPath + " file is corrupt!");
            }

            instance = this;
        }

        #endregion

        #region Public Properties

        public static PoshZenContainer Default
        {
            get
            {
                if (instance != null)
                {
                    return instance;
                }

                lock (syncRoot)
                {
                    if (instance != null)
                    {
                        return instance;
                    }

                    var container = new UnityContainer();
                    container.RegisterType(typeof(TicketManager));

                    instance = new PoshZenContainer(WindowsEnvironment.Default, container);
                }

                return instance;
            }
        }

        public IZendeskClient Client { get; set; }

        public IUnityContainer Container { get; private set; }

        public IEnvironment Environment { get; private set; }

        public ClientSettings Settings { get; set; }

        public string SettingsJsonPath { get; set; }

        #endregion

        #region Public Methods and Operators

        public IZendeskClient ResolveClient()
        {
            if (this.Client != null)
            {
                return this.Client;
            }

            if (this.Settings == null)
            {
                return null;
            }

            return this.Client =
                       new ZendeskClient(
                           this.Settings.Domain, 
                           this.Settings.ZendeskUser.Unprotect(), 
                           this.Settings.ZendeskSecret.Unprotect(), 
                           this.Settings.CredentialType);
        }

        public IManager<TInterface> ResolveManager<TInterface>(IZendeskClient client)
            where TInterface : IZendeskThing, ITrackable
        {

            return this.Container.Resolve<IManager<TInterface>>(new ResolverOverride[] { new ParameterOverride("client", client) });            
        }

        public void WriteJsonSettings(ClientSettings settings)
        {
            this.Settings = settings;
            var serializedSettings = JsonConvert.SerializeObject(
                this.Settings, 
                new JsonSerializerSettings { Formatting = Formatting.Indented });
            this.Environment.WriteContents(this.SettingsJsonPath, serializedSettings);
        }

        #endregion
    }
}