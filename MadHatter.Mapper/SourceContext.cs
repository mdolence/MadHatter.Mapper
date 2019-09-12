namespace MadHatter
{
    /// <summary>
    /// Provides methods for configuring the mapping of the source object.
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    public class SourceContext<TSource>
        where TSource : class
    {
        // Constructors

        internal SourceContext() { }


        // Methods

        /// <summary>
        /// Specify the type to map to.
        /// </summary>
        /// <typeparam name="TTarget">The type to map to.</typeparam>
        /// <returns>An instance of <see cref="MappingContext{TSource, TTarget}"/> to create specific property mappings.</returns>
        public MappingContext<TSource, TTarget> To<TTarget>() where TTarget : class
            => new MappingContext<TSource, TTarget>();
    }
}