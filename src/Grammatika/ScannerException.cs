using System;

namespace Grammatika
{
    using System.Runtime.Serialization;

    public class ScannerException : Exception
    {
        public ScannerException()
        {
        }

        public ScannerException(string message) : base(message)
        {
        }

        public ScannerException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ScannerException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}