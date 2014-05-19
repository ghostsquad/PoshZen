namespace PoshZen.Test
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Management.Automation;

    public class RunspaceInvocationData
    {
        #region Constructors and Destructors

        public RunspaceInvocationData()
        {
            this.Results = new Collection<PSObject>();
            this.ErrorRecords = new List<ErrorRecord>();
            this.DebugRecords = new List<DebugRecord>();
            this.ProgressRecords = new List<ProgressRecord>();
            this.VerboseRecords = new List<VerboseRecord>();
            this.WarningRecords = new List<WarningRecord>();
        }

        #endregion

        #region Public Properties

        public List<DebugRecord> DebugRecords { get; set; }

        public List<ErrorRecord> ErrorRecords { get; set; }

        public List<ProgressRecord> ProgressRecords { get; set; }

        public Collection<PSObject> Results { get; set; }

        public List<VerboseRecord> VerboseRecords { get; set; }

        public List<WarningRecord> WarningRecords { get; set; }

        #endregion
    }
}