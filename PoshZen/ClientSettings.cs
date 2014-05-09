namespace PoshZen
{
    using SharpZendeskApi;

    internal class ClientSettings
    {
        #region Properties

        public ZendeskAuthenticationMethod CredentialType { get; set; }

        public string DisplayName { get; set; }

        public string Domain { get; set; }

        public string ZendeskSecret { get; set; }

        public string ZendeskUser { get; set; }

        #endregion
    }
}