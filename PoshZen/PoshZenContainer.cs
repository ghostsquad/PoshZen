namespace PoshZen
{
    using System;
    using System.IO;

    using Microsoft.Practices.Unity;

    using Newtonsoft.Json;

    using SharpZendeskApi;
    using SharpZendeskApi.Management;
    using SharpZendeskApi.Models;

    /// <summary>
    /// The posh zen container.
    /// </summary>
    internal sealed class PoshZenContainer
    {
        #region Static Fields

        /// <summary>
        /// The sync root.
        /// </summary>
        private static readonly object syncRoot = new object();

        /// <summary>
        /// The instance.
        /// </summary>
        private static volatile PoshZenContainer instance;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PoshZenContainer"/> class.
        /// </summary>
        /// <param name="environment">
        /// The environment.
        /// </param>
        /// <param name="container">
        /// The container.
        /// </param>
        /// <exception cref="InvalidOperationException">
        /// </exception>
        private PoshZenContainer(IEnvironment environment, IUnityContainer container)
        {
            this.Environment = environment;
            this.Container = container;

            string poshZenAppDataFolder = Path.Combine(environment.ApplicationDataFolder, "PoshZen");
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
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the default.
        /// </summary>
        public static PoshZenContainer Default
        {
            get
            {
                if (instance != null)
                {
                    return instance;
                }

                var container = new UnityContainer();

                // models
                container.RegisterType<ITicket, Ticket>();
                container.RegisterType<IView, View>();               

                // managers
                container.RegisterType<IManager<ITicket>, TicketManager>();
                container.RegisterType<IManager<IView>, ViewManager>();

                // pages

                Create(WindowsEnvironment.Default, container);

                return instance;
            }
        }

        /// <summary>
        /// Gets or sets the client.
        /// </summary>
        public IZendeskClient Client { get; set; }

        /// <summary>
        /// Gets the container.
        /// </summary>
        public IUnityContainer Container { get; private set; }

        /// <summary>
        /// Gets the environment.
        /// </summary>
        public IEnvironment Environment { get; private set; }

        /// <summary>
        /// Gets or sets the settings.
        /// </summary>
        public ClientSettings Settings { get; set; }

        /// <summary>
        /// Gets or sets the settings json path.
        /// </summary>
        public string SettingsJsonPath { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The create.
        /// </summary>
        /// <param name="environment">
        /// The environment.
        /// </param>
        /// <param name="container">
        /// The container.
        /// </param>
        /// <returns>
        /// The <see cref="PoshZenContainer"/>.
        /// </returns>
        public static PoshZenContainer Create(IEnvironment environment, IUnityContainer container)
        {
            lock (syncRoot)
            {
                instance = new PoshZenContainer(environment, container);
            }

            return instance;
        }

        /// <summary>
        /// The resolve client.
        /// </summary>
        /// <returns>
        /// The <see cref="IZendeskClient"/>.
        /// </returns>
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

            return
                this.Client =
                new ZendeskClient(
                    this.Settings.Domain, 
                    this.Settings.ZendeskUser.Unprotect(), 
                    this.Settings.ZendeskSecret.Unprotect(), 
                    this.Settings.CredentialType);
        }

        /// <summary>
        /// The resolve manager.
        /// </summary>
        /// <param name="client">
        /// The client.
        /// </param>
        /// <typeparam name="TInterface">
        /// </typeparam>
        /// <returns>
        /// The <see cref="IManager"/>.
        /// </returns>
        public IManager<TInterface> ResolveManager<TInterface>(IZendeskClient client)
            where TInterface : IZendeskThing, ITrackable
        {
            return
                this.Container.Resolve<IManager<TInterface>>(
                    new ResolverOverride[] { new ParameterOverride("client", client) });
        }

        public void WriteJsonSettings(ClientSettings settings)
        {
            this.Settings = settings;
            string serializedSettings = JsonConvert.SerializeObject(
                this.Settings, 
                new JsonSerializerSettings { Formatting = Formatting.Indented });
            this.Environment.WriteContents(this.SettingsJsonPath, serializedSettings);
        }

        #endregion
    }
}