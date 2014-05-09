namespace PoshZen.Test
{
    using System;
    using System.IO;

    public class ContainerFixture
    {
        #region Fields

        private readonly string expectedSettings;

        private ClientSettings settings;

        #endregion

        #region Constructors and Destructors

        public ContainerFixture()
        {
            this.expectedSettings =
                File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"testfiles\settings.json"));
            this.settings = new ClientSettings
                                {
                                    CredentialType = 0, 
                                    DisplayName = "DisplayNamefb183b4f-3ec5-42a2-a786-95fcb051a098", 
                                    Domain = "Domain8e84fbb7-76a7-47e1-b36f-4060aec365bf", 
                                    ZendeskSecret = "ZendeskSecret8dd2f3ed-6cd1-4b0f-bb95-61cc15dfaf61", 
                                    ZendeskUser = "ZendeskUser3bae769d-1121-4f23-a288-7ed951c6033d"
                                };
        }

        #endregion

        #region Public Properties

        public string ExpectedSettings
        {
            get
            {
                return this.expectedSettings;
            }
        }

        #endregion

        #region Properties

        internal ClientSettings Settings
        {
            get
            {
                return this.settings;
            }
        }

        #endregion
    }
}