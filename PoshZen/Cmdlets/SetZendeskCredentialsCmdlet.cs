namespace PoshZen.Cmdlets
{
    using System;
    using System.Management.Automation;

    using SharpZendeskApi;

    [Cmdlet(VerbsCommon.Set, CmdletNamingConstants.ZendeskCredentials, DefaultParameterSetName = ParamSetDefault)]
    public class SetZendeskCredentialsCmdlet : PSCmdlet
    {
        #region Constants

        private const string ParamSetDefault = "default";

        private const string ParamSetPsCred = "PSCred";

        #endregion

        #region Fields

        private ZendeskAuthenticationMethod authenticationMethod = ZendeskAuthenticationMethod.Basic;

        #endregion

        #region Public Properties

        [Parameter(Position = 3, ParameterSetName = ParamSetDefault)]
        [Parameter(Position = 2, ParameterSetName = ParamSetPsCred)]
        public ZendeskAuthenticationMethod AuthenticationMethod
        {
            get
            {
                return this.authenticationMethod;
            }

            set
            {
                this.authenticationMethod = value;
            }
        }

        [Parameter(Position = 1, Mandatory = true, ParameterSetName = ParamSetPsCred)]
        [ValidateNotNullOrEmpty]
        public PSCredential Credential { get; set; }

        [Parameter(Position = 0, Mandatory = true, ParameterSetName = ParamSetDefault)]
        [Parameter(Position = 0, Mandatory = true, ParameterSetName = ParamSetPsCred)]
        [ValidateNotNullOrEmpty]
        public string Domain { get; set; }

        [Parameter(Position = 2, Mandatory = true, ParameterSetName = ParamSetDefault)]
        [ValidateNotNullOrEmpty]
        public string Password { get; set; }

        [Parameter(Position = 1, Mandatory = true, ParameterSetName = ParamSetDefault)]
        [ValidateNotNullOrEmpty]
        public string UserName { get; set; }

        #endregion

        #region Methods

        protected override void ProcessRecord()
        {
            var clientSettings = new ClientSettings
                                     {
                                         Domain = this.Domain, 
                                         CredentialType = this.AuthenticationMethod, 
                                         DisplayName = Guid.NewGuid().ToString()
                                     };

            switch (this.ParameterSetName)
            {
                case ParamSetDefault:
                    {
                        clientSettings.ZendeskUser = this.UserName.Protect();
                        clientSettings.ZendeskSecret = this.Password.Protect();
                        break;
                    }

                case ParamSetPsCred:
                    {
                        clientSettings.ZendeskUser = this.Credential.UserName.Protect();
                        clientSettings.ZendeskUser = this.Credential.Password.Protect();
                        break;
                    }
            }

            PoshZenContainer.Default.WriteJsonSettings(clientSettings);
        }

        #endregion
    }
}