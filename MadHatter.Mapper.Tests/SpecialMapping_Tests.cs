using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace MadHatter.Tools.Mapper.Tests
{
    [TestClass]
    public class SpecialMapping_Tests
    {
        [TestMethod]
        public void ByteArray_String_MissingMapping_Test()
        {
            var ex = Assert.ThrowsException<UnmappedPropertyException>(() =>
            {
                ObjectMapper.Maps<StringClass>().To<ByteArrayClass>().
                    Create();
            });

            Assert.IsTrue(ex.Message.Contains(nameof(StringClass.ConvertMe)));
            Assert.IsTrue(ex.Message.Contains(nameof(ByteArrayClass.ConvertMe)));
            Assert.IsTrue(ex.Message.Contains(nameof(StringClass.StringValue)));
            Assert.IsTrue(ex.Message.Contains(nameof(ByteArrayClass.ByteArrayValue)));
        }

        [TestMethod]
        public void ByteArray_To_String_Mapping_Test()
        {
            var sut = ObjectMapper.Maps<ByteArrayClass>().To<StringClass>()
                .Using(s => s.ConvertMe).ToSet(t => t.ConvertMe)
                .Using(s => s.ByteArrayValue).ToSet(t => t.StringValue)
                .Create();

            var source = new ByteArrayClass()
            {
                ConvertMe = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 },
                ByteArrayValue = new byte[] { 9, 8, 7, 6, 5, 4, 3, 2, 1 }
            };

            var target = sut.Map(source);

            var expected_ConvertMe = Convert.ToBase64String(source.ConvertMe);
            var expected_ByteArrayValue = Convert.ToBase64String(source.ByteArrayValue);

            Assert.AreEqual(expected_ConvertMe, target.ConvertMe);
            Assert.AreEqual(expected_ByteArrayValue, target.StringValue);
        }

        [TestMethod]
        public void String_To_ByteArray_Mapping_Test()
        {
            var sut = ObjectMapper.Maps<StringClass>().To<ByteArrayClass>()
                .Using(s => s.ConvertMe).ToSet(t => t.ConvertMe)
                .Using(s => s.StringValue).ToSet(t => t.ByteArrayValue)
                .Create();

            var source = new StringClass()
            {
                ConvertMe = Convert.ToBase64String(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 }),
                StringValue = Convert.ToBase64String(new byte[] { 9, 8, 7, 6, 5, 4, 3, 2, 1 })
            };

            var target = sut.Map(source);

            var expected_ConvertMe = Convert.FromBase64String(source.ConvertMe);
            var expected_StringValue = Convert.FromBase64String(source.StringValue);

            CollectionAssert.AreEqual(expected_ConvertMe, target.ConvertMe);
            CollectionAssert.AreEqual(expected_StringValue, target.ByteArrayValue);
        }


        public class StringClass
        {
            public string ConvertMe { get; set; }

            public string StringValue { get; set; }
        }

        public class ByteArrayClass
        {
            public byte[] ConvertMe { get; set; }

            public byte[] ByteArrayValue { get; set; }
        }
    }
}
