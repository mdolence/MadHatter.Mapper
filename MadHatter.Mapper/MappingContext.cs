using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace MadHatter
{
    /// <summary>
    /// Provides methods for configuring property mappings.
    /// </summary>
    /// <typeparam name="TSource">The source type of the mapping.</typeparam>
    /// <typeparam name="TTarget">The target type of the mapping.</typeparam>
    public class MappingContext<TSource, TTarget>
        where TSource : class
        where TTarget : class
    {
        // Fields

        private readonly IEnumerable<PropertyInfo> _sourceProperties;
        private readonly IEnumerable<PropertyInfo> _targetProperties;

        private readonly ParameterExpression _sourceParam = Expression.Parameter(typeof(TSource));
        private readonly ParameterExpression _targetParam = Expression.Parameter(typeof(TTarget));


        // Constructors

        internal MappingContext()
        {
            _sourceProperties = typeof(TSource).GetProperties();
            _targetProperties = typeof(TTarget).GetProperties();

            this.Initialize();
        }


        // Properties

        internal Collection<MappingAction> MappingActions { get; } = new Collection<MappingAction>();

        internal Collection<PropertyInfo> IgnoreSourceProperties { get; } = new Collection<PropertyInfo>();

        internal Collection<PropertyInfo> IgnoreTargetProperties { get; } = new Collection<PropertyInfo>();


        // Methods

        private void Initialize()
        {
            foreach (PropertyInfo src in _sourceProperties)
            {
                foreach (PropertyInfo tgt in _targetProperties)
                {
                    if (src.Name == tgt.Name && (src.PropertyType == tgt.PropertyType || (src.PropertyType == tgt.PropertyType.GetGenericType() || src.PropertyType.GetGenericType() == tgt.PropertyType)))
                    {
                        this.MappingActions.Add(new MappingAction(src, tgt));
                    }
                }
            }
        }

        /// <summary>
        /// <para>Configure the source property of a new mapping.</para>
        /// <para>All properties with matching types and names are automatically mapped.</para>
        /// <para>All properties need to be accounted for, use the <see cref="IgnoreSourceProperties"/> and <see cref="IgnoreTargetProperties"/> methods to ignore unused properties.</para>
        /// </summary>
        /// <typeparam name="TSourceProperty">The type of the source property.</typeparam>
        /// <param name="property">An <see cref="Expression"/> representing the property.</param>
        /// <returns>An instance of <see cref="PropertyMappingContext{TSource, TTarget}"/> to configure specific property mapping.</returns>
        public PropertyMappingContext<TSource, TTarget> Using<TSourceProperty>(Expression<Func<TSource, TSourceProperty>> property)
        {
            PropertyInfo sourceProperty = (PropertyInfo)((MemberExpression)property.Body).Member;
            return new PropertyMappingContext<TSource, TTarget>(this, sourceProperty);
        }

        /// <summary>
        /// <para>Ignores the specified source property.</para>
        /// <para>Use this method if the property will not be mapped using another process.</para>
        /// </summary>
        /// <typeparam name="TSourceProperty">The type of the source property.</typeparam>
        /// <param name="ignore">An <see cref="Expression"/> representing the ignored property.</param>
        /// <returns>This instance of <see cref="MappingContext{TSource, TTarget}"/></returns>
        public MappingContext<TSource, TTarget> IgnoringSourceProperty<TSourceProperty>(Expression<Func<TSource, TSourceProperty>> ignore)
        {
            PropertyInfo ignoreProperty = (PropertyInfo)((MemberExpression)ignore.Body).Member;
            this.IgnoreSourceProperties.Add(ignoreProperty);
            return this;
        }

        /// <summary>
        /// <para>Ignores the specified target property.</para>
        /// <para>Use this method if the property will not be mapped using another process.</para>
        /// </summary>
        /// <typeparam name="TTargetProperty">The type of the target property.</typeparam>
        /// <param name="ignore">An <see cref="Expression"/> representing the ignored property.</param>
        /// <returns>This instance of <see cref="MappingContext{TSource, TTarget}"/></returns>
        public MappingContext<TSource, TTarget> IgnoringTargetProperty<TTargetProperty>(Expression<Func<TTarget, TTargetProperty>> ignore)
        {
            PropertyInfo ignoreProperty = (PropertyInfo)((MemberExpression)ignore.Body).Member;
            this.IgnoreTargetProperties.Add(ignoreProperty);
            return this;
        }

        /// <summary>
        /// <para>Ignores the specified source property. </para>
        /// <para>Use this method if the property will be mapped using another process.</para>
        /// <para>This method calls <see cref="IgnoringSourceProperty{TSourceProperty}(Expression{Func{TSource, TSourceProperty}})"/>.</para>
        /// </summary>
        /// <typeparam name="TSourceProperty">The type of the source property.</typeparam>
        /// <param name="ignore">An <see cref="Expression"/> representing the property.</param>
        /// <returns>This instance of <see cref="MappingContext{TSource, TTarget}"/></returns>
        public MappingContext<TSource, TTarget> DeferSourceProperty<TSourceProperty>(Expression<Func<TSource, TSourceProperty>> ignore)
            => this.IgnoringSourceProperty(ignore);

        /// <summary>
        /// <para>Ignores the specified target property.</para>
        /// <para>Use this method if the property will be mapped using another process.</para>
        /// <para>This method calls <see cref="IgnoringTargetProperty{TTargetProperty}(Expression{Func{TTarget, TTargetProperty}})"/>.</para>
        /// </summary>
        /// <typeparam name="TTargetProperty">The type of the target property.</typeparam>
        /// <param name="ignore">An <see cref="Expression"/> representing the ignored property.</param>
        /// <returns>This instance of <see cref="MappingContext{TSource, TTarget}"/></returns>
        public MappingContext<TSource, TTarget> DeferTargetProperty<TTargetProperty>(Expression<Func<TTarget, TTargetProperty>> ignore)
            => this.IgnoringTargetProperty(ignore);

        /// <summary>
        /// Creates an implementation of <see cref="IMapping{TSource, TTarget}"/> to use when mapping objects.
        /// </summary>
        /// <returns>An instance of <see cref="IMapping{TSource, TTarget}"/>.</returns>
        public IMapping<TSource, TTarget> Create()
        {
            var missingSourceProperties = _sourceProperties.Where(p => !this.MappingActions.Select(m => m.SourceProperty).Any(sp => sp.Name == p.Name))
                                                           .Where(p => !this.IgnoreSourceProperties.Any(sp => sp.Name == p.Name));
            var missingTargetProperties = _targetProperties.Where(p => !this.MappingActions.Select(m => m.TargetProperty).Any(sp => sp.Name == p.Name))
                                                           .Where(p => !this.IgnoreTargetProperties.Any(sp => sp.Name == p.Name));

            if (missingSourceProperties.Any() || missingTargetProperties.Any())
            {
                throw new UnmappedPropertyException(typeof(TSource),  missingSourceProperties.Select(p => p.Name).ToArray(), typeof(TTarget), missingTargetProperties.Select(p => p.Name).ToArray());
            }

            var actions = new List<Expression>();

            var mappingActions = this.MappingActions.Where(m => !this.IgnoreSourceProperties.Contains(m.SourceProperty) && !this.IgnoreTargetProperties.Contains(m.TargetProperty));

            foreach (MappingAction action in mappingActions)
            {
                var setter = action.TargetProperty.GetSetMethod();
                var getter = Expression.Property(_sourceParam, action.SourceProperty);

                if (action.SourceProperty.PropertyType == typeof(string) && action.TargetProperty.PropertyType == typeof(byte[]))
                {
                    actions.Add(this.MappingStringToByteArray(action, getter, setter));
                }
                else if (action.SourceProperty.PropertyType == typeof(byte[]) && action.TargetProperty.PropertyType == typeof(string))
                {
                    actions.Add(this.MappingByteArrayToString(action, getter, setter));
                }
                else if (!action.SourceProperty.PropertyType.IsGenericType && action.TargetProperty.PropertyType.IsGenericType && action.TargetProperty.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    actions.Add(this.MappingValueToNullable(action, getter, setter));
                }
                else if (action.SourceProperty.PropertyType.IsGenericType && action.SourceProperty.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) && !action.TargetProperty.PropertyType.IsGenericType)
                {
                    actions.Add(this.MappingNullableToValue(action, getter, setter));
                }
                else
                {
                    actions.Add(this.MappingDefault(action, getter, setter));
                }

            }

            if (actions.Count > 0)
            {
                var body = Expression.Block(actions.ToArray());

                var compiledAction = Expression.Lambda<Action<TSource, TTarget>>(body, _sourceParam, _targetParam)
                    .Compile();

                return new Mapping<TSource, TTarget>(compiledAction);
            }

            return new Mapping<TSource, TTarget>(null);
        }


        #region [ Mapping Methods ]

        private Expression MappingStringToByteArray(MappingAction action, MemberExpression getter, MethodInfo setter)
        {
            var call = Expression.Call(typeof(ConversionHelper), nameof(ConversionHelper.ConvertStringToByteArray), null, getter);
            return Expression.Call(_targetParam, setter, call);
        }

        private Expression MappingByteArrayToString(MappingAction action, MemberExpression getter, MethodInfo setter)
        {
            var call = Expression.Call(typeof(ConversionHelper), nameof(ConversionHelper.ConvertByteArrayToString), null, getter);
            return Expression.Call(_targetParam, setter, call);
        }

        private Expression MappingNullableToValue(MappingAction action, MemberExpression getter, MethodInfo setter)
        {
            var call = Expression.Call(typeof(ConversionHelper), nameof(ConversionHelper.ConvertToValue), new[] { action.TargetProperty.PropertyType }, getter);
            return Expression.Call(_targetParam, setter, call);
        }

        private Expression MappingValueToNullable(MappingAction action, MemberExpression getter, MethodInfo setter)
        {
            var call = Expression.Call(typeof(ConversionHelper), nameof(ConversionHelper.ConvertToNullable), new[] { action.SourceProperty.PropertyType }, getter);
            return Expression.Call(_targetParam, setter, call);
        }

        private Expression MappingDefault(MappingAction action, MemberExpression getter, MethodInfo setter)
        {
            return Expression.Call(_targetParam, setter, getter);
        }

        #endregion
    }
}