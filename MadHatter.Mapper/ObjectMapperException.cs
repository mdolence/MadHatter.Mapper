using System;

namespace MadHatter
{
    public class ObjectMapperException : Exception
    {
        public ObjectMapperException(string message)
            : base(message) { }

        public ObjectMapperException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}