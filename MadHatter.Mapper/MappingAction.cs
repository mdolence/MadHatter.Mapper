using System.Reflection;

namespace MadHatter
{
    internal class MappingAction
    {
        public MappingAction(PropertyInfo sourceProperty, PropertyInfo targetProperty)
        {
            this.SourceProperty = sourceProperty;
            this.TargetProperty = targetProperty;
        }

        public PropertyInfo SourceProperty { get; private set; }

        public PropertyInfo TargetProperty { get; private set; }
    }
}