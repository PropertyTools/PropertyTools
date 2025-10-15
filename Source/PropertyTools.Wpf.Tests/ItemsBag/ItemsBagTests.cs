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

        [Test]
        public void GetValue_NullableTypeWithDifferentValues()
        {
            var t0 = new TestObject() { NullableInt = 10 };
            var t1 = new TestObject() { NullableInt = 20 };
            var bag = new ItemsBag(new[] { t0, t1 });
            var provider = new ItemsBagTypeDescriptionProvider();
            var td = provider.GetTypeDescriptor(typeof(ItemsBag), bag);
            var p1 = td.GetProperties().Find("NullableInt", false);
            Assert.AreEqual(null, p1.GetValue(bag));
        }

        [Test]
        public void GetValue_NullableTypeWithEqualValues()
        {
            var t0 = new TestObject() { NullableInt = 10 };
            var t1 = new TestObject() { NullableInt = 10 };
            var bag = new ItemsBag(new[] { t0, t1 });
            var provider = new ItemsBagTypeDescriptionProvider();
            var td = provider.GetTypeDescriptor(typeof(ItemsBag), bag);
            var p1 = td.GetProperties().Find("NullableInt", false);
            Assert.AreEqual(10, p1.GetValue(bag));
        }

        [Test]
        public void GetValue_GenericStructWithDifferentValues()
        {
            var t0 = new TestObject() { GenericValue = new GenericStruct<int>(42) };
            var t1 = new TestObject() { GenericValue = new GenericStruct<int>(99) };
            var bag = new ItemsBag(new[] { t0, t1 });
            var provider = new ItemsBagTypeDescriptionProvider();
            var td = provider.GetTypeDescriptor(typeof(ItemsBag), bag);
            var p1 = td.GetProperties().Find("GenericValue", false);
            Assert.AreEqual(null, p1.GetValue(bag));
        }

        [Test]
        public void GetValue_GenericStructWithEqualValues()
        {
            var value = new GenericStruct<int>(42);
            var t0 = new TestObject() { GenericValue = value };
            var t1 = new TestObject() { GenericValue = value };
            var bag = new ItemsBag(new[] { t0, t1 });
            var provider = new ItemsBagTypeDescriptionProvider();
            var td = provider.GetTypeDescriptor(typeof(ItemsBag), bag);
            var p1 = td.GetProperties().Find("GenericValue", false);
            var result = p1.GetValue(bag);
            Assert.IsNotNull(result);
            Assert.AreEqual(value, result);
        }

        public struct GenericStruct<T>
        {
            public T Value { get; set; }

            public GenericStruct(T value)
            {
                Value = value;
            }
        }

        private class TestObject
        {
            public bool IsChecked => this.Checked;
            public bool Checked { get; set; }
            public string Name { get; set; }
            public int? NullableInt { get; set; }
            public GenericStruct<int> GenericValue { get; set; }
        }
    }
}