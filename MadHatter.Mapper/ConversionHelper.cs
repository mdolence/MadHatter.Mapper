using System;

namespace MadHatter
{
    internal static class ConversionHelper
    {
        public static T ConvertToValue<T>(T? value) where T : struct
            => (value.HasValue ? value.Value : default(T));

        public static T? ConvertToNullable<T>(T value) where T : struct
            => new T?(value);

        public static byte[] ConvertStringToByteArray(string value)
            => Convert.FromBase64String(value);

        public static string ConvertByteArrayToString(byte[] value)
            => Convert.ToBase64String(value);
    }
}