// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ItemsBagTests.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf.Tests
{
    using NUnit.Framework;

    using System;

    [TestFixture]
    public class ItemsBagTests
    {
        [Test]
        public void SetValue_ReadOnlyProperty()
        {

            var t0 = new TestObject();
            var bag = new ItemsBag(new[] { t0 });
            var provider = new ItemsBagTypeDescriptionProvider();
            var td = provider.GetTypeDescriptor(typeof(ItemsBag), bag);
            var props = td.GetProperties();

            var p1 = props.Find("Checked", false);
            Assert.That(p1.IsReadOnly, Is.EqualTo(false));
            p1.SetValue(bag, true);
            Assert.That(t0.Checked, Is.EqualTo(true));
            Assert.That(t0.IsChecked, Is.EqualTo(true));

            var p2 = props.Find("IsChecked", false);

            Assert.That(p2.IsReadOnly, Is.EqualTo(true));
            Assert.Throws<ArgumentException>(() => p2.SetValue(bag, false));
            Assert.That(t0.IsChecked, Is.EqualTo(true));
        }

        [Test]
        public void GetValue_ValueTypeWithMultipleValues()
        {
            var t0 = new TestObject() { Checked = true };
            var t1 = new TestObject() { Checked = false };
            var bag = new ItemsBag(new[] { t0, t1 });
            var provider = new ItemsBagTypeDescriptionProvider();
            var td = provider.GetTypeDescriptor(typeof(ItemsBag), bag);
            var p1 = td.GetProperties().Find("Checked", false);
            Assert.That(p1.GetValue(bag), Is.EqualTo(null));
        }

        [Test]
        public void GetValue_ValueTypeWithEqualValues()
        {
            var t0 = new TestObject() { Checked = true };
            var t1 = new TestObject() { Checked = true };
            var bag = new ItemsBag(new[] { t0, t1 });
            var provider = new ItemsBagTypeDescriptionProvider();
            var td = provider.GetTypeDescriptor(typeof(ItemsBag), bag);
            var p1 = td.GetProperties().Find("Checked", false);
            Assert.That(p1.GetValue(bag), Is.EqualTo(true));
        }

        [Test]
        public void GetValue_ReferenceTypeWithDifferentValues()
        {
            var t0 = new TestObject() { Name = "John" };
            var t1 = new TestObject() { Name = "James" };
            var bag = new ItemsBag(new[] { t0, t1 });
            var provider = new ItemsBagTypeDescriptionProvider();
            var td = provider.GetTypeDescriptor(typeof(ItemsBag), bag);
            var p1 = td.GetProperties().Find("Name", false);
            Assert.That(p1.GetValue(bag), Is.EqualTo(null));
        }

        [Test]
        public void GetValue_ReferenceTypeWithEqualValues()
        {
            var t0 = new TestObject() { Name = "John" };
            var t1 = new TestObject() { Name = "John" };
            var bag = new ItemsBag(new[] { t0, t1 });
            var provider = new ItemsBagTypeDescriptionProvider();
            var td = provider.GetTypeDescriptor(typeof(ItemsBag), bag);
            var p1 = td.GetProperties().Find("Name", false);
            Assert.That(p1.GetValue(bag), Is.EqualTo("John"));
        }

        private class TestObject
        {
            public bool IsChecked => this.Checked;
            public bool Checked { get; set; }
            public string Name { get; set; }
        }
    }
}