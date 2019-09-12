using System.Collections;
using System.IO;
using System.Xml.Serialization;

namespace MadHatter
{
    /// <summary>
    /// Fluent API for mapping the properties of one object to another object.
    /// </summary>
    public class ObjectMapper
    {
        // Constructors

        private ObjectMapper() { }


        // Methods

        /// <summary>
        /// Specify the type to map from.
        /// </summary>
        /// <typeparam name="TSource">The type to map from.</typeparam>
        /// <returns>An instance of <see cref="SourceContext{TSource}"/> for setting source mapping parameters.</returns>
        public static SourceContext<TSource> Maps<TSource>() where TSource : class
            => new SourceContext<TSource>();
    }
}