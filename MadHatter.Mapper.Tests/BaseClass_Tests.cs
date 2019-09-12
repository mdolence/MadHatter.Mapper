using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace MadHatter.Tools.Mapper.Tests
{
    [TestClass]
    public class BaseClass_Tests
    {
        [TestMethod]
        public void BaseClass_PropertyMapping_Tests()
        {
            var sut = ObjectMapper.Maps<Source>().To<Target>()
                .Using(s => s.SourceString).ToSet(t => t.TargetBaseString)
                .Using(s => s.SourceBaseString).ToSet(t => t.TargetString)
                .Create();

            var source = new Source()
            {
                Name = "NAME",
                BaseString = "BASESTRING",
                SourceString = "SOURCESTRING",
                SourceBaseString = "SOURCEBASESTRING"
            };

            var target = sut.Map(source);

            Assert.AreEqual(source.Name, target.Name);
            Assert.AreEqual(source.BaseString, target.BaseString);
            Assert.AreEqual(source.SourceString, target.TargetBaseString);
            Assert.AreEqual(source.SourceBaseString, target.TargetString);
        }

        [TestMethod]
        public void BaseClass_Ignoring_Tests()
        {
            var sut = ObjectMapper.Maps<Source>().To<Target>()
                .Using(s => s.SourceString).ToSet(t => t.TargetString)
                .IgnoringSourceProperty(s => s.SourceBaseString)
                .IgnoringTargetProperty(t => t.TargetBaseString)
                .Create();

            var source = new Source()
            {
                Name = "NAME",
                BaseString = "BASESTRING",
                SourceString = "SOURCESTRING",
                SourceBaseString = "SOURCEBASESTRING"
            };

            var target = sut.Map(source);
        }

        [TestMethod]
        public void BaseClass_MissingMapping_Tests()
        {
            var ex = Assert.ThrowsException<UnmappedPropertyException>(() =>
            {
                ObjectMapper.Maps<Source>().To<Target>()
                   .Using(s => s.SourceString).ToSet(t => t.TargetString)
                   .Create();
            });

            Assert.IsTrue(ex.Message.Contains(nameof(Source.SourceBaseString)));
            Assert.IsTrue(ex.Message.Contains(nameof(Target.TargetBaseString)));
        }


        public class Source : SourceBase
        {
            public string Name { get; set; }

            public string SourceString { get; set; }
        }

        public class SourceBase
        {
            public string BaseString { get; set; }

            public string SourceBaseString { get; set; }
        }

        public class Target : TargetBase
        {
            public string Name { get; set; }

            public string TargetString { get; set; }
        }

        public class TargetBase
        {
            public string BaseString { get; set; }

            public string TargetBaseString { get; set; }
        }
    }
}
