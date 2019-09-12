using System;
using System.Linq.Expressions;
using System.Reflection;

namespace MadHatter
{
    /// <summary>
    /// Provides methods for confiuring the property mapping.
    /// </summary>
    /// <typeparam name="TSource">The source type of the mapping.</typeparam>
    /// <typeparam name="TTarget">The target type of the mapping.</typeparam>
    public class PropertyMappingContext<TSource, TTarget>
        where TSource : class
        where TTarget : class
    {
        // Constructors

        internal PropertyMappingContext(MappingContext<TSource, TTarget> parentContext, PropertyInfo source)
        {
            this.ParentContext = parentContext;
            this.SourceProperty = source;
        }


        // Properties

        internal MappingContext<TSource, TTarget> ParentContext { get; private set; }

        internal PropertyInfo SourceProperty { get; private set; }


        // Methods

        /// <summary>
        /// Configure the target property of a new mapping.
        /// </summary>
        /// <typeparam name="TTargetProperty"></typeparam>
        /// <param name="property">An <see cref="Expression"/> representing the target property.</param>
        /// <returns>The <see cref="Mapping{TSource, TTarget}"/> instance use to call this context.</returns>
        public MappingContext<TSource, TTarget> ToSet<TTargetProperty>(Expression<Func<TTarget, TTargetProperty>> property)
        {
            PropertyInfo targetProperty = (PropertyInfo)((MemberExpression)property.Body).Member;

            this.ParentContext.MappingActions.Add(new MappingAction(this.SourceProperty, targetProperty));

            return this.ParentContext;
        }
    }
}