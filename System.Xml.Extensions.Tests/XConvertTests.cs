using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data;

namespace System.Xml.Extensions.Tests
{
    [TestClass]
    public class XConvertTests
    {
        enum Fruit
        {
            Apple       = 1,
            Banana,
            Kiwi,
        }

        [DataTestMethod]
        [DataRow("Apple")]
        [DataRow("1")]
        public void ParsingDefinedEnumValueReturnsEnumValue(string s)
        {
            // Act
            Fruit value = XConvert.To<Fruit>(s);

            // Assert
            Assert.AreEqual(Fruit.Apple, value);
        }

        [TestMethod]
        [DataRow("Papaya")]
        [DataRow("4")]
        [ExpectedException(typeof(ArgumentException))]
        public void ParsingUndefinedEnumValueThrowsException(string s)
        {
            // Act
            XConvert.To<Fruit>(s);
        }

        [TestMethod]
        [DataRow("Papaya")]
        [DataRow("4")]
        public void ParsingUndefinedEnumValueReturnsNull(string s)
        {
            // Act
            Fruit? value = XConvert.ToNullable<Fruit>(s);

            // Assert
            Assert.IsNull(value);
        }

        [TestMethod]
        [DataRow("Papaya")]
        [DataRow("4")]
        public void ParsingUndefinedEnumValueReturnsDefaultValue(string s)
        {
            // Act
            Fruit fruit = XConvert.ToOrDefault<Fruit>(s, Fruit.Banana);

            // Assert
            Assert.AreEqual(Fruit.Banana, fruit);
        }

        [TestMethod]
        [DataRow("Apple")]
        [DataRow("1")]
        public void ParsingDefinedEnumValueReturnsEnumValue2(string s)
        {
            // Act
            Fruit fruit = XConvert.ToOrDefault<Fruit>(s);

            // Assert
            Assert.AreEqual(Fruit.Apple, fruit);
        }

        [TestMethod]
        [DataRow("Papaya")]
        [DataRow("4")]
        [ExpectedException(typeof(ArgumentException))]
        public void ParsingUndefinedEnumValueWithNoDefaultThrowsException(string s)
        {
            // Assert
            Assert.IsFalse(Enum.IsDefined(typeof(Fruit), default(Fruit)));

            // Act
            Fruit _ = XConvert.ToOrDefault<Fruit>(s);
        }

        [TestMethod]
        [DataRow("true", true)]
        [DataRow("1", true)]
        [DataRow("false", false)]
        [DataRow("0", false)]
        public void ParsingXmlBooleanReturnsBool(string s, object expected)
        {
            // Act
            bool resultat = XConvert.To<bool>(s);

            // Assert
            Assert.AreEqual(expected, resultat);
        }

        [TestMethod]
        [DataRow("32", 32)]
        [DataRow("-1", -1)]
        public void ParsingXmlIntegerReturnsInt(string s, object expected)
        {
            // Act
            int resultat = XConvert.To<int>(s);

            // Assert
            Assert.AreEqual(expected, resultat);
        }

        private const int Undef = -1;

        [TestMethod]
        [DataRow("2002-09-24", 2002, 9, 24)]
        [DataRow("2002-09-24Z", 2002, 9, 24)]
        [DataRow("2002-09-24+06:00", 2002, 9, 23, 20)]
        [DataRow("09:30:10.5", Undef, Undef, Undef, 9, 30, 10, 500)]
        [DataRow("09:30:10Z", Undef, Undef, Undef, 9, 30, 10, 0)]
        [DataRow("2002-05-30T09:30:00", 2002, 05, 30, 9, 30, 0, 0)]
        [DataRow("2002-05-30T09:30:10.5", 2002, 05, 30, 9, 30, 10, 500)]
        [DataRow("2002-05-30T09:30:10Z", 2002, 05, 30, 9, 30, 10, 0)]
        public void ParsingXmlDateTimeReturnsDateTime(string s, int year, int month, int day, int hour = 0, int minute = 0, int second = 0, int millisecond = 0)
        {
            // Arrange
            DateTime now = DateTime.Now;
            if (year == Undef) year = now.Year;
            if (month == Undef) month = now.Month;
            if (day == Undef) day = now.Day;

            // Act
            DateTime resultat = XConvert.To<DateTime>(s);

            // Assert
            Assert.AreEqual(year, resultat.Year);
            Assert.AreEqual(month, resultat.Month);
            Assert.AreEqual(day, resultat.Day);
            Assert.AreEqual(hour, resultat.Hour);
            Assert.AreEqual(minute, resultat.Minute);
            Assert.AreEqual(second, resultat.Second);
            Assert.AreEqual(millisecond, resultat.Millisecond);
        }
    }
}
