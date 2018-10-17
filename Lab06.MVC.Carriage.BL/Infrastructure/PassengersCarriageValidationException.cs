using System;
using System.Runtime.Serialization;

namespace Lab06.MVC.Carriage.BL.Infrastructure
{
    [Serializable]
    public class PassengersCarriageValidationException : Exception
    {
        public PassengersCarriageValidationException()
        {
        }

        public PassengersCarriageValidationException(string message)
            : base(message)
        {
        }

        protected PassengersCarriageValidationException(SerializationInfo info, StreamingContext context)
        : base(info, context)
        {
        }

        public PassengersCarriageValidationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public string Property { get; protected set; }

        public PassengersCarriageValidationException(string message, string prop)
            : base(message)
        {
            Property = prop;
        }
    }
}
