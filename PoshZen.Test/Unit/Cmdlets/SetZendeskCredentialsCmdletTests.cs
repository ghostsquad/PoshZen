namespace PoshZen.Test.Unit.Cmdlets
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Management.Automation;
    using System.Text;
    using System.Threading.Tasks;

    using FluentAssertions;

    using PoshZen.Cmdlets;

    using Xunit;
    using Xunit.Extensions;

    public class SetZendeskCredentialsCmdletTests : ScriptCsCmdletTestBase
    {
        [Theory]
        [PropertyData("CmdletNullInputScenarios")]
        public void GivenNullArgumentExpectArgumentNullException(string script)
        {
            const string ExpectedException = "Cannot process command because of one or more missing mandatory parameters: Domain Password UserName.";            

            // arrange
            var cmd = new PSCommand();
            cmd.AddCommand("Set-ZendeskCredentials");

            // act
            this.Invoking(x => x.Invoke(cmd)).ShouldThrow<ParameterBindingException>().WithMessage(ExpectedException);
        }

        public static IEnumerable<object[]> CmdletNullInputScenarios
        {
            get
            {
                return new[]
                {
                    new object[] { "Set-ZendeskCredentials" },
                    new object[] { "Set-ZendeskCredentials -Domain foo" },
                    new object[] { "Set-ZendeskCredentials -Domain foo -Password foo" }                    
                };
            }
        }        
    }
}
