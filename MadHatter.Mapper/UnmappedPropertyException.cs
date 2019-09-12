using System;

namespace MadHatter
{
    /// <summary>
    /// The exception that is thrown when an unmapped source or target property is detected.
    /// </summary>
    public class UnmappedPropertyException : ObjectMapperException
    {
        // Constructors

        /// <summary>
        /// Initializes a new instance of <see cref="UnmappedPropertyException"/> using the supplied missing source properties and missing target properties.
        /// </summary>
        /// <param name="missingSourceProperties">An array of missing source properties.</param>
        /// <param name="missingTargetProperties">An array of missing target properties.</param>
        [Obsolete]
        public UnmappedPropertyException(string[] missingSourceProperties, string[] missingTargetProperties)
            : base(BuildMessage(null, missingSourceProperties, null, missingTargetProperties))
        {
            this.Data.Add("UnmappedSourceProperties", String.Join(", ", missingSourceProperties));
            this.Data.Add("UnmappedTargetProperties", String.Join(", ", missingTargetProperties));

            this.MissingSourceProperties = missingSourceProperties;
            this.MissingTargetProperties = missingTargetProperties;
            this.SourceType = null;
            this.TargetType = null;
        }

        /// <summary>
        /// Initializes a new instance of <see cref="UnmappedPropertyException"/> using the supplied source type, missing source properties, target type, and missing target properties.
        /// </summary>
        /// <param name="sourceType">The type of the source object.</param>
        /// <param name="missingSourceProperties">An array of missing source properties.</param>
        /// <param name="targetType">The type of the target object.</param>
        /// <param name="missingTargetProperties">An array of missing target properties.</param>
        public UnmappedPropertyException(Type sourceType, string[] missingSourceProperties, Type targetType, string[] missingTargetProperties)
            : base(BuildMessage(sourceType, missingSourceProperties, targetType, missingTargetProperties))
        {
            this.Data.Add("UnmappedSourceProperties", String.Join(", ", missingSourceProperties));
            this.Data.Add("UnmappedTargetProperties", String.Join(", ", missingTargetProperties));
            this.Data.Add("Mapping", $"{sourceType.FullName} => {targetType.FullName}");

            this.MissingSourceProperties = missingSourceProperties;
            this.MissingTargetProperties = missingTargetProperties;
            this.SourceType = sourceType;
            this.TargetType = targetType;
        }


        // Properties

        /// <summary>
        /// Get the <see cref="Type"/> of the source object.
        /// </summary>
        public Type SourceType { get; }

        /// <summary>
        /// Gets an array of unmapped source properties.
        /// </summary>
        public string[] MissingSourceProperties { get; }

        /// <summary>
        /// Gets the <see cref="Type"/> of the target object.
        /// </summary>
        public Type TargetType { get; }

        /// <summary>
        /// Gets an array of unmapped target properties.
        /// </summary>
        public string[] MissingTargetProperties { get; }


        // Methods

        internal static string BuildMessage(Type sourceType, string[] missingSourceProperties, Type targetType, string[] missingTargetProperties)
            => $"All properties need to be handled:\r\nMapping: {sourceType.FullName} => {targetType.FullName}\r\nUnmapped Source Properties: {String.Join(", ", missingSourceProperties)}\r\nUnmapped Target Properties: {String.Join(", ", missingTargetProperties)}";


    }
}