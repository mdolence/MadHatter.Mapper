namespace MadHatter
{
    /// <summary>
    /// Defines a contract for mapping an object of <typeparamref name="TSource"/> to <typeparamref name="TTarget"/>.
    /// </summary>
    /// <typeparam name="TSource">The source type of the mapping.</typeparam>
    /// <typeparam name="TTarget">The target type of the mapping.</typeparam>
    public interface IMapping<TSource, TTarget>
    {
        /// <summary>
        /// Maps the <paramref name="source"/> object to a new instance of <typeparamref name="TTarget"/>.
        /// </summary>
        /// <param name="source">The source object.</param>
        /// <returns>An instance of the target object.</returns>
        TTarget Map(TSource source);
    }
}