namespace PoshZen
{
    using SharpZendeskApi.Core;

    internal class ClientSettings
    {
        #region Properties

        internal ZendeskAuthenticationMethod CredentialType { get; set; }

        internal string DisplayName { get; set; }

        internal string Domain { get; set; }

        internal string ZendeskSecret { get; set; }

        internal string ZendeskUser { get; set; }

        #endregion
    }
}