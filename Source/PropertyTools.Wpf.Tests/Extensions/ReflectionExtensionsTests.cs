// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReflectionExtensionsTests.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf.Tests
{
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;

    using NUnit.Framework;

    [TestFixture]
    public class ReflectionExtensionsTests
    {
        public enum Enum1
        {
            [Browsable(false)]
            NotShownValue,
            Value1,
            Value2,
            [Browsable(false)]
            [System.ComponentModel.Description("Description of AnotherNotShownValue")]
            AnotherNotShownValue,
            [System.ComponentModel.Description("Description of Value3")]
            Value3
        }

        [AttributeUsage(AttributeTargets.Property)]
        private class TestBaseAttribute : Attribute
        {
        }

        private class TestDerivedAttribute : TestBaseAttribute
        {
        }

        private class ReflectionTestModel
        {
            [TestDerivedAttribute]
            public ObservableCollection<string> Values { get; } = new ObservableCollection<string>();

            public string Name { get; set; }
        }

        [Test]
        public void FilterOnBrowsableAttribute()
        {
            Assert.That(Enum.GetValues(typeof(Enum1)).Length, Is.EqualTo(5));
            Assert.That(Enum.GetValues(typeof(Enum1)).FilterOnBrowsableAttribute().Count(), Is.EqualTo(3));
        }

        [Test]
        public void FilterOnEnumFilterAttribute_ExcludeMode_ExcludesSpecifiedValues()
        {
            var values = Enum.GetValues(typeof(Enum1)).FilterOnBrowsableAttribute();
            var filter = new DataAnnotations.EnumFilterAttribute(DataAnnotations.EnumFilterAttribute.FilteringMode.Exclude, Enum1.Value1);
            var result = values.FilterOnEnumFilterAttribute(filter);
            Assert.That(result, Does.Not.Contain(Enum1.Value1));
            Assert.That(result, Contains.Item(Enum1.Value2));
            Assert.That(result, Contains.Item(Enum1.Value3));
        }

        [Test]
        public void FilterOnEnumFilterAttribute_IncludeMode_IncludesOnlySpecifiedValues()
        {
            var values = Enum.GetValues(typeof(Enum1)).FilterOnBrowsableAttribute();
            var filter = new DataAnnotations.EnumFilterAttribute(DataAnnotations.EnumFilterAttribute.FilteringMode.Include, Enum1.Value1);
            var result = values.FilterOnEnumFilterAttribute(filter);
            Assert.That(result, Contains.Item(Enum1.Value1));
            Assert.That(result, Does.Not.Contain(Enum1.Value2));
            Assert.That(result, Does.Not.Contain(Enum1.Value3));
        }

        [Test]
        public void FilterOnEnumFilterAttribute_NullFilter_ReturnsAllValues()
        {
            var values = Enum.GetValues(typeof(Enum1)).FilterOnBrowsableAttribute();
            var result = values.FilterOnEnumFilterAttribute(null);
            Assert.That(result.Count, Is.EqualTo(values.Count));
        }

        [Test]
        public void GetFirstAttributeOrDefault_ExactAttributeType_ReturnsAttribute()
        {
            var descriptor = TypeDescriptor.GetProperties(typeof(ReflectionTestModel))[nameof(ReflectionTestModel.Values)];

            var attribute = descriptor.GetFirstAttributeOrDefault(typeof(TestDerivedAttribute));

            Assert.That(attribute, Is.TypeOf<TestDerivedAttribute>());
        }

        [Test]
        public void GetFirstAttributeOrDefault_DerivedAttribute_ReturnsAttribute()
        {
            var descriptor = TypeDescriptor.GetProperties(typeof(ReflectionTestModel))[nameof(ReflectionTestModel.Values)];

            var attribute = descriptor.GetFirstAttributeOrDefault(typeof(TestBaseAttribute));

            Assert.That(attribute, Is.TypeOf<TestDerivedAttribute>());
        }

        [Test]
        public void GetFirstAttributeOrDefault_MissingAttribute_ReturnsNull()
        {
            var descriptor = TypeDescriptor.GetProperties(typeof(ReflectionTestModel))[nameof(ReflectionTestModel.Name)];

            var attribute = descriptor.GetFirstAttributeOrDefault(typeof(TestBaseAttribute));

            Assert.That(attribute, Is.Null);
        }
    }
}