namespace System.Xml.Extensions
{
    public static class XConvert
    {
        /// <summary>
        /// Converts a <see cref="string"/> to an enum type equivalent.
        /// </summary>
        /// <typeparam name="TEnum">The enum type to which to convert <c>value</c>.</typeparam>
        /// <param name="value">The string representation of the enumeration name or underlying value to convert.</param>
        /// <returns>An object of type <typeparamref name="TEnum"/> whose value is represented by <c>value</c>.</returns>
        /// <exception cref="System.ArgumentException">
        /// value is either an empty string or only contains white space. 
        /// value is a name, but not one of the named constants defined for the enumeration.
        /// value is outside the range of the underlying type of enumType.
        /// </exception>
        private static TEnum ToDefinedEnum<TEnum>(string value) where TEnum : struct
        {
            if (Enum.TryParse<TEnum>(value, out TEnum result))
                if (Enum.IsDefined(typeof(TEnum), result))
                    return result;
            throw new ArgumentException($"The constant '{value}' doesn't exist in the enumeration '{typeof(TEnum).Name}'.");
        }

        /// <summary>
        /// Converts a <see cref="string"/> to a value type equivalent.
        /// </summary>
        /// <typeparam name="T">The value type to which to convert <c>s</c>.</typeparam>
        /// <param name="s">The string representation of the XML value to convert.</param>
        /// <returns>An object of type <typeparamref name="T"/> whose value is represented by <c>s</c>.</returns>
        /// <exception cref="System.ArgumentException"><typeparamref name="T"/> is not supported.</exception>
        /// <exception cref="System.ArgumentNullException"><c>s</c> is null.</exception>
        /// <exception cref="System.FormatException"><c>s</c> does not represent a <typeparamref name="T"/> value.</exception>
        public static T To<T>(string s) where T : struct
        {
            var d = default(T);
            switch (d)
            {
                case Enum _:
                    return ToDefinedEnum<T>(s);
                case bool _:
                    return (T)(Object)XmlConvert.ToBoolean(s);
                case short _:
                    return (T)(Object)XmlConvert.ToInt16(s);
                case int _:
                    return (T)(Object)XmlConvert.ToInt32(s);
                case long _:
                    return (T)(Object)XmlConvert.ToInt64(s);
                case float _:
                    return (T)(Object)XmlConvert.ToSingle(s);
                case double _:
                    return (T)(Object)XmlConvert.ToDouble(s);
                case decimal _:
                    return (T)(Object)XmlConvert.ToDecimal(s);
                case byte _:
                    return (T)(Object)XmlConvert.ToByte(s);
                case sbyte _:
                    return (T)(Object)XmlConvert.ToSByte(s);
                case char _:
                    return (T)(Object)XmlConvert.ToChar(s);
                case DateTime _:
                    return (T)(Object)XmlConvert.ToDateTime(s, XmlDateTimeSerializationMode.RoundtripKind);
                case DateTimeOffset _:
                    return (T)(Object)XmlConvert.ToDateTimeOffset(s);
                case Guid _:
                    return (T)(Object)XmlConvert.ToGuid(s);
                default:
                    throw new ArgumentException($"Type '{typeof(T).Name}' not supported.");
            }
        }

        /// <summary>
        /// Converts a <see cref="string"/> to a value type equivalent.
        /// </summary>
        /// <typeparam name="T">The value type to which to convert <c>s</c>.</typeparam>
        /// <param name="s">The string representation of the XML value to convert.</param>
        /// <param name="defaultValue">A value to return if the string cannot be converted to the given type.</param>
        /// <returns>An object of type <typeparamref name="T"/> whose value is represented by <c>s</c> or <c>defaultValue</c>.</returns>
        public static T ToOrDefault<T>(string s, T defaultValue = default) where T : struct
        {
            if (TryTo<T>(s, out T result))
                return result;

            if (typeof(T).IsEnum && !typeof(T).IsEnumDefined(defaultValue))
                throw new ArgumentException(nameof(defaultValue));
            return defaultValue;
        }

        /// <summary>
        /// Converts a <see cref="string"/> to a value type equivalent.
        /// </summary>
        /// <typeparam name="T">A value type.</typeparam>
        /// <param name="s">The string representation of the XML value to convert.</param>
        /// <returns>An object of type <typeparamref name="T"/> whose value is represented by <c>s</c> or <see langword="null"/> if the string cannot be converted to the given type.</returns>
        public static T? ToNullable<T>(string s) where T : struct
        {
            if (TryTo<T>(s, out T result))
                return result;
            else
                return null;
        }

        /// <summary>
        /// Converts a <see cref="string"/> to a value type equivalent.
        /// </summary>
        /// <typeparam name="T">The value type to which to convert <c>s</c>.</typeparam>
        /// <param name="s">The string representation of the XML value to convert.</param>
        /// <param name="result">
        /// When this method returns, result contains an object of type <typeparamref name="T"/> whose value is represented by value if the convert operation succeeds. 
        /// If the convert operation fails, result contains the default value of the underlying type of <typeparamref name="T"/>. 
        /// This parameter is passed uninitialized.
        /// </param>
        /// <returns><see langword="true"/> if the value parameter was converted successfully; otherwise, <see langword="false"/>.</returns>
        public static bool TryTo<T>(string s, out T result) where T : struct
        {
            try
            {
                result = To<T>(s);
                return true;
            }
            catch (Exception)
            {
                result = default;
                return false;
            }
        }
    }
}
