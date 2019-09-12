using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MadHatter.Tools.Mapper.Tests
{
    [TestClass]
    public class CollectionMapping_Tests
    {
        [TestMethod]
        public void CollectionMapping_Test()
        {
            var source = new Source()
            {
                List = new List<SourceChild>()
                {
                    new SourceChild() { Id = Guid.NewGuid() },
                    new SourceChild() { Id = Guid.NewGuid() },
                    new SourceChild() { Id = Guid.NewGuid() },
                    new SourceChild() { Id = Guid.NewGuid() },
                    new SourceChild() { Id = Guid.NewGuid() },
                    new SourceChild() { Id = Guid.NewGuid() },
                    new SourceChild() { Id = Guid.NewGuid() },
                    new SourceChild() { Id = Guid.NewGuid() },
                    new SourceChild() { Id = Guid.NewGuid() },
                    new SourceChild() { Id = Guid.NewGuid() },
                }
            };

            var childMapping = ObjectMapper.Maps<SourceChild>().To<TargetChild>()
                .Create();

            var sut = ObjectMapper.Maps<Source>().To<Target>()
                .DeferSourceProperty(s => s.List)
                .DeferTargetProperty(t => t.List)
                .Create();

            var target = sut.Map(source);

            target.List = new List<TargetChild>();
            target.List.AddRange(source.List.Select(s => childMapping.Map(s)));

            Assert.IsNotNull(target);

            Assert.AreEqual(10, target.List?.Count);
        }

        private class Source
        {
            public List<SourceChild> List { get; set; }
        }

        private class SourceChild
        {
            public Guid Id { get; set; }
        }

        private class Target
        {
            public List<TargetChild> List { get; set; }
        }

        private class TargetChild
        {
            public Guid Id { get; set; }

        }

    }
}
