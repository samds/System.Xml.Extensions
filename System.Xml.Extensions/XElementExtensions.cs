using System.Xml.Linq;

namespace System.Xml.Extensions
{
    public static class XElementExtensions
    {
        /// <summary>
        /// Returns the attribute value to a value type equivalent.
        /// </summary>
        /// <typeparam name="T">The value type to which to convert the attribute value.</typeparam>
        /// <param name="this">An <see cref="XElement"/>.</param>
        /// <param name="name">An attribute name.</param>
        /// <returns></returns>
        public static T AttributeValue<T>(this XElement @this, XName name) where T : struct
        {
            var attribute = @this.Attribute(name);
            return XConvert.To<T>(attribute.Value);
        }

        public static T AttributeValueOrDefault<T>(this XElement @this, XName name, T defaultValue = default) where T : struct
        {
            var attribute = @this.Attribute(name);
            return XConvert.ToOrDefault<T>(attribute.Value);
        }

        public static Nullable<T> AttributeNullableValue<T>(this XElement @this, XName name) where T : struct
        {
            var attribute = @this.Attribute(name);
            return XConvert.ToNullable<T>(attribute.Value);
        }
    }
}
