namespace PoshZen.Exceptions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.Text;
    using System.Threading.Tasks;

    public class PoshZenException : Exception
    {
        public PoshZenException()
        {
        }

        public PoshZenException(string message)
            : base(message)
        {
        }

        public PoshZenException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected PoshZenException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
