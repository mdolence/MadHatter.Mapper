using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MadHatter.Tools.Mapper.Tests
{
    [TestClass]
    public class ObjectMapper_Tests
    {
        [TestMethod]
        public void ObjectMapper_Success_Test()
        {
            var source = new Source()
            {
                String = "STRING_VALUE",
                Int = 23,
                Child = new Child() { String = "VALUE", Int = 32354 },
                Children = new[] { new Child() { String = "VALUE_A", Int = 19 }, new Child() { String = "VALUE_B", Int = 20 } },
                One = "1"
            };

            var sut = ObjectMapper.Maps<Source>()
                                   .To<Target>()
                                   .Using(s => s.One).ToSet(t => t.Two)
                                   .IgnoringSourceProperty(s => s.Children)
                                   .IgnoringTargetProperty(t => t.Children)
                                   .Create();

            var target = sut.Map(source);


            Assert.AreEqual(source.String, target.String);
            Assert.AreEqual(source.Int, target.Int);

            Assert.AreEqual(source.Child.String, target.Child.String);
            Assert.AreEqual(source.Child.Int, target.Child.Int);

            //Assert.AreEqual(source.Children.Length, target.Children.Length);

            Assert.AreEqual(source.One, target.Two);
        }

        [TestMethod]
        public void ObjectMapper_IgnoreProperty_Test()
        {
            var source = new Source()
            {
                String = "STRING_VALUE",
                Int = 23,
                Child = new Child() { String = "VALUE", Int = 32354 },
                Children = new[] { new Child() { String = "VALUE_A", Int = 19 }, new Child() { String = "VALUE_B", Int = 20 } },
            };

            var sut = ObjectMapper.Maps<Source>()
                                   .To<Target>()
                                   .Using(s => s.One).ToSet(t => t.Two)
                                   .IgnoringSourceProperty(s => s.Children)
                                   .IgnoringTargetProperty(t => t.Children)
                                   .Create();

            var target = sut.Map(source);

            Assert.AreEqual(source.String, target.String);
            Assert.AreEqual(source.Int, target.Int);

            Assert.AreEqual(source.Child.String, target.Child.String);
            Assert.AreEqual(source.Child.Int, target.Child.Int);

            Assert.IsNull(target.Children);

            Assert.AreEqual(source.One, target.Two);
        }

        [TestMethod]
        public void ObjectMapper_MissingProperty_Test()
        {
            Assert.ThrowsException<UnmappedPropertyException>(() =>
            {
                var sut = ObjectMapper.Maps<Source>()
                           .To<Target>()
                           .Create();
            });
        }

        public class Source
        {
            public string String { get; set; }

            public int Int { get; set; }

            public Child Child { get; set; }

            public Child[] Children { get; set; }

            public string One { get; set; }

            public Guid? FromNullGuid { get; set; }

            public Guid ToNullGuid { get; set; }

            public Guid? NullGuid { get; set; }
        }

        public class Target
        {
            public string String { get; set; }

            public int Int { get; set; }

            public Child Child { get; set; }

            public Kid[] Children { get; set; }

            public string Two { get; set; }

            public Guid FromNullGuid { get; set; }

            public Guid? ToNullGuid { get; set; }

            public Guid? NullGuid { get; set; }
        }

        public class Child
        {
            public string String { get; set; }

            public int Int { get; set; }
        }

        public class Kid
        {
            public string String { get; set; }

            public int Int { get; set; }
        }
    }
}
