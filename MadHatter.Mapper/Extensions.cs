using System;

namespace MadHatter
{
    internal static class Extensions
    {
        public static Type GetGenericType(this Type type)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() != null)
                return type.GetGenericArguments()[0];

            return null;
        }
    }
}