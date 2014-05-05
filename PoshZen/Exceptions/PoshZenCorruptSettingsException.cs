namespace PoshZen.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    public class PoshZenCorruptSettingsException : Exception
    {
        #region Constructors and Destructors

        public PoshZenCorruptSettingsException()
        {
        }

        public PoshZenCorruptSettingsException(string message)
            : base(message)
        {
        }

        public PoshZenCorruptSettingsException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected PoshZenCorruptSettingsException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        #endregion
    }
}