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
            Assert.AreEqual(false, p1.IsReadOnly);
            p1.SetValue(bag, true);
            Assert.AreEqual(true, t0.Checked);
            Assert.AreEqual(true, t0.IsChecked);

            var p2 = props.Find("IsChecked", false);

            Assert.AreEqual(true, p2.IsReadOnly);
            Assert.Throws<ArgumentException>(() => p2.SetValue(bag, false));
            Assert.AreEqual(true, t0.IsChecked);
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
            Assert.AreEqual(null, p1.GetValue(bag));
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
            Assert.AreEqual(true, p1.GetValue(bag));
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
            Assert.AreEqual(null, p1.GetValue(bag));
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
            Assert.AreEqual("John", p1.GetValue(bag));
        }

        private class TestObject
        {
            public bool IsChecked => this.Checked;
            public bool Checked { get; set; }
            public string Name { get; set; }
        }
    }
}