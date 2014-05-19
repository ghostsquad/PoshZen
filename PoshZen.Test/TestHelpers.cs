namespace PoshZen.Test
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using SharpZendeskApi;

    using CredentialManagement;

    using Xunit;

    public static class TestHelpers
    {
        public const string CredentialTargetFormat = "ZendeskEnd2EndTests-{0}";

        public const string CredentialUserNameExample = "emailaddress@@http://mydomain.zendesk.com/api/v2";

        public static ZendeskClientBase GetClient(
            ZendeskAuthenticationMethod authenticationMethod,
            bool useGoodPasswordToken = true)
        {
            try
            {
                var credKeyValue = GetTestCredential(authenticationMethod);
                var password = useGoodPasswordToken ? credKeyValue.Value.Password : Guid.Empty.ToString();

                var client = new ZendeskClient(
                    credKeyValue.Key,
                    credKeyValue.Value.Username,
                    password,
                    authenticationMethod);

                return client;
            }
            catch (Exception)
            {
                var expectedTarget = string.Format(CredentialTargetFormat, authenticationMethod);

                var assertMessage =
                    string.Format(
                        "Unable to run {0} authentication End2End tests.\n\nCreate a new Generic Credential in Credential Manager with:\n\nTarget Address as: \n\n[{1}] and\n\nUsername in this format: \n\n[{2}] ",
                        authenticationMethod,
                        expectedTarget,
                        CredentialUserNameExample);

                Assert.False(true, assertMessage);
            }

            return null;
        }

        public static KeyValuePair<string, Credential> GetTestCredential(
            ZendeskAuthenticationMethod authenticationMethod)
        {
            var credTarget = string.Format(CredentialTargetFormat, authenticationMethod);

            var credential = new Credential { Target = credTarget };
            var loadResult = credential.Load();

            var exceptionMsg = string.Format("No credential exists for given target {0}", credTarget);

            if (!loadResult)
            {
                throw new InvalidOperationException(exceptionMsg);
            }

            var indexOfDoubleAt = credential.Username.IndexOf("@@", StringComparison.Ordinal);
            if (indexOfDoubleAt < 0)
            {
                throw new InvalidOperationException(exceptionMsg);
            }

            var keyValue = new KeyValuePair<string, Credential>(
                credential.Username.Substring(indexOfDoubleAt + 2),
                credential);

            credential.Username = credential.Username.Substring(0, indexOfDoubleAt);

            return keyValue;
        }
    }
}
