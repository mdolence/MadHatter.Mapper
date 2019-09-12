using System;

namespace MadHatter
{
    /// <summary>
    /// The default implementation of <see cref="IMapping{TSource, TTarget}"/>.
    /// </summary>
    /// <typeparam name="TSource">The source type of the mapping.</typeparam>
    /// <typeparam name="TTarget">The target type of the mapping.</typeparam>
    public class Mapping<TSource, TTarget> : IMapping<TSource, TTarget>
        where TSource : class
        where TTarget : class
    {
        // Constructors

        internal Mapping(Action<TSource, TTarget> action)
        {
            this.MappingAction = action;
        }


        // Properties

        internal Action<TSource, TTarget> MappingAction { get; private set; }


        // Methods

        /// <summary>
        /// Maps the <paramref name="source"/> object to a new instance of <typeparamref name="TTarget"/>.
        /// </summary>
        /// <param name="source">The source object.</param>
        /// <returns>An instance of the target object.</returns>
        public TTarget Map(TSource source)
        {
            var target = Activator.CreateInstance<TTarget>();

            this.MappingAction?.Invoke(source, target);

            return target ?? default(TTarget);
        }
    }
}