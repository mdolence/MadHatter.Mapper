using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace MadHatter
{
    /// <summary>
    /// The exception that is thrown when a mapping cannot be performed using the specified parameters.
    /// </summary>
    public sealed class InvalidMappingException : ObjectMapperException
    {
        public InvalidMappingException(string message)
            : base(message) { }

        public InvalidMappingException(PropertyInfo from, PropertyInfo to, Exception innerException)
            : this($"Invalid mapping of '{from.DeclaringType.Name}.{from.Name}' to '{to.DeclaringType.Name}.{to.Name}'.", innerException) { }

        public InvalidMappingException(Type from, Type to, Exception innerException)
            : this($"Invalid mapping of '{from.Name}' to '{to.Name}'.", innerException) { }

        public InvalidMappingException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
