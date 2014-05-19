// thanks to beefarino for a nice base class for invoking powershell cmdlets
// https://github.com/beefarino/scriptcs-powershell-module/blob/master/src/CodeOwls.PowerShell.ScriptCS.Tests/ScriptCSCmdletTestBase.cs
namespace PoshZen.Test
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Management.Automation;
    using System.Management.Automation.Runspaces;

    public abstract class ScriptCsCmdletTestBase
    {
        #region Methods

        protected RunspaceInvocationData Invoke(string script)
        {
            var invocationData = new RunspaceInvocationData();
            using (var rs = RunspaceFactory.CreateRunspace())
            {
                rs.Open();
                using (var ps = PowerShell.Create())
                {
                    ps.Runspace = rs;

                    ps.AddScript("ls PoshZen.psd1 | import-module;").Invoke();
                    ps.AddScript(script);

                    ps.Invoke().ToList().ForEach(invocationData.Results.Add);

                    PopulateRunspaceInvocationData(ps, invocationData);

                    return invocationData;
                }
            }
        }

        protected RunspaceInvocationData Invoke(PSCommand command, Dictionary<string, object> variables = null)
        {
            var invocationData = new RunspaceInvocationData();
            using (var rs = RunspaceFactory.CreateRunspace())
            {
                rs.Open();
                using (var ps = PowerShell.Create())
                {
                    ps.Runspace = rs;

                    ps.AddScript("ls PoshZen.psd1 | import-module;").Invoke();

                    if (variables != null)
                    {
                        variables.ToList().ForEach(x => rs.SessionStateProxy.SetVariable(x.Key, x.Value));
                    }

                    ps.Commands = command;
                    ps.Invoke().ToList().ForEach(invocationData.Results.Add);

                    PopulateRunspaceInvocationData(ps, invocationData);

                    return invocationData;
                }
            }
        }

        protected RunspaceInvocationData Invoke(IEnumerable<string> script)
        {
            var invocationData = new RunspaceInvocationData();
            using (var rs = RunspaceFactory.CreateRunspace())
            {
                rs.Open();
                using (var ps = PowerShell.Create())
                {
                    ps.Runspace = rs;

                    ps.AddScript("ls *.dll | import-module;").Invoke();
                    script.ToList().ForEach(
                        s =>
                            {
                                ps.AddScript(s);
                                ps.Invoke().ToList().ForEach(invocationData.Results.Add);
                            });

                    PopulateRunspaceInvocationData(ps, invocationData);

                    return invocationData;
                }
            }
        }

        private static void PopulateRunspaceInvocationData(PowerShell ps, RunspaceInvocationData invocationData)
        {
            invocationData.ErrorRecords = ps.Streams.Error.ToList();
            invocationData.DebugRecords = ps.Streams.Debug.ToList();
            invocationData.ProgressRecords = ps.Streams.Progress.ToList();
            invocationData.VerboseRecords = ps.Streams.Verbose.ToList();
            invocationData.WarningRecords = ps.Streams.Warning.ToList();

            if (invocationData.ErrorRecords.Count > 0)
            {
                throw invocationData.ErrorRecords[0].Exception;
            }
        }

        #endregion
    }
}