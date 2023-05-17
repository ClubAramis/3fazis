using System;
using System.Runtime.Serialization;

namespace RF.Modules.TestFlightAppointment.Services.Implementations
{
    [Serializable]
    internal class TestFlightException : Exception
    {
        public TestFlightException()
        {
        }

        public TestFlightException(string message) : base(message)
        {
        }

        public TestFlightException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected TestFlightException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}