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
        public void Constructor_WithArray_CreatesItemsBag()
        {
            var t0 = new TestObject();
            var t1 = new TestObject();
            var bag = new ItemsBag(new[] { t0, t1 });

            Assert.IsNotNull(bag);
            Assert.AreEqual(2, bag.Objects.Length);
            Assert.AreSame(t0, bag.Objects[0]);
            Assert.AreSame(t1, bag.Objects[1]);
        }

        [Test]
        public void Constructor_WithList_CreatesItemsBag()
        {
            var t0 = new TestObject();
            var t1 = new TestObject();
            var list = new System.Collections.Generic.List<TestObject> { t0, t1 };
            var bag = new ItemsBag(list);

            Assert.IsNotNull(bag);
            Assert.AreEqual(2, bag.Objects.Length);
            Assert.AreSame(t0, bag.Objects[0]);
            Assert.AreSame(t1, bag.Objects[1]);
        }

        [Test]
        public void Constructor_WithSingleObject_CreatesItemsBag()
        {
            var t0 = new TestObject();
            var bag = new ItemsBag(new[] { t0 });

            Assert.IsNotNull(bag);
            Assert.AreEqual(1, bag.Objects.Length);
            Assert.AreSame(t0, bag.Objects[0]);
        }

        [Test]
        public void Constructor_WithEmptyCollection_CreatesItemsBag()
        {
            var bag = new ItemsBag(new TestObject[0]);

            Assert.IsNotNull(bag);
            Assert.AreEqual(0, bag.Objects.Length);
        }

        [Test]
        public void BiggestType_WithSameType_ReturnsThatType()
        {
            var t0 = new TestObject();
            var t1 = new TestObject();
            var bag = new ItemsBag(new[] { t0, t1 });

            Assert.AreEqual(typeof(TestObject), bag.BiggestType);
        }

        [Test]
        public void SetValue_WithMultipleObjects_SetsAllValues()
        {
            var t0 = new TestObject() { Name = "John" };
            var t1 = new TestObject() { Name = "Jane" };
            var bag = new ItemsBag(new[] { t0, t1 });
            var provider = new ItemsBagTypeDescriptionProvider();
            var td = provider.GetTypeDescriptor(typeof(ItemsBag), bag);
            var p1 = td.GetProperties().Find("Name", false);

            p1.SetValue(bag, "Bob");

            Assert.AreEqual("Bob", t0.Name);
            Assert.AreEqual("Bob", t1.Name);
        }

        [Test]
        public void SetValue_WithSingleObject_SetsValue()
        {
            var t0 = new TestObject() { Checked = false };
            var bag = new ItemsBag(new[] { t0 });
            var provider = new ItemsBagTypeDescriptionProvider();
            var td = provider.GetTypeDescriptor(typeof(ItemsBag), bag);
            var p1 = td.GetProperties().Find("Checked", false);

            p1.SetValue(bag, true);

            Assert.AreEqual(true, t0.Checked);
        }

        [Test]
        public void GetValue_WithSingleObject_ReturnsValue()
        {
            var t0 = new TestObject() { Name = "John" };
            var bag = new ItemsBag(new[] { t0 });
            var provider = new ItemsBagTypeDescriptionProvider();
            var td = provider.GetTypeDescriptor(typeof(ItemsBag), bag);
            var p1 = td.GetProperties().Find("Name", false);

            Assert.AreEqual("John", p1.GetValue(bag));
        }

        [Test]
        public void GetValue_WithNullReferenceTypeValues_ReturnsNull()
        {
            var t0 = new TestObject() { Name = null };
            var t1 = new TestObject() { Name = null };
            var bag = new ItemsBag(new[] { t0, t1 });
            var provider = new ItemsBagTypeDescriptionProvider();
            var td = provider.GetTypeDescriptor(typeof(ItemsBag), bag);
            var p1 = td.GetProperties().Find("Name", false);

            Assert.AreEqual(null, p1.GetValue(bag));
        }

        [Test]
        public void GetValue_WithMixedNullAndNonNullValues_ReturnsNull()
        {
            var t0 = new TestObject() { Name = "John" };
            var t1 = new TestObject() { Name = null };
            var bag = new ItemsBag(new[] { t0, t1 });
            var provider = new ItemsBagTypeDescriptionProvider();
            var td = provider.GetTypeDescriptor(typeof(ItemsBag), bag);
            var p1 = td.GetProperties().Find("Name", false);

            Assert.AreEqual(null, p1.GetValue(bag));
        }

        [Test]
        public void PropertyChanged_WhenPropertySet_IsRaised()
        {
            var t0 = new TestObject();
            var bag = new ItemsBag(new[] { t0 });
            var provider = new ItemsBagTypeDescriptionProvider();
            var td = provider.GetTypeDescriptor(typeof(ItemsBag), bag);
            var p1 = td.GetProperties().Find("Checked", false);

            bool eventRaised = false;
            string propertyName = null;
            bag.PropertyChanged += (sender, e) =>
            {
                eventRaised = true;
                propertyName = e.PropertyName;
            };

            p1.SetValue(bag, true);

            Assert.IsTrue(eventRaised);
            Assert.AreEqual("Checked", propertyName);
        }

        [Test]
        public void PropertyChanged_WhenObjectPropertyChanges_IsRelayed()
        {
            var t0 = new ObservableTestObject();
            var bag = new ItemsBag(new[] { t0 });

            bool eventRaised = false;
            string propertyName = null;
            bag.PropertyChanged += (sender, e) =>
            {
                eventRaised = true;
                propertyName = e.PropertyName;
            };

            t0.Name = "NewName";

            Assert.IsTrue(eventRaised);
            Assert.AreEqual("Name", propertyName);
        }

        [Test]
        public void Dispose_UnsubscribesFromPropertyChangedEvents()
        {
            var t0 = new ObservableTestObject();
            var bag = new ItemsBag(new[] { t0 });

            bool eventRaised = false;
            bag.PropertyChanged += (sender, e) => { eventRaised = true; };

            bag.Dispose();

            t0.Name = "NewName";

            Assert.IsFalse(eventRaised);
        }

        [Test]
        public void PropertyType_ForValueType_ReturnsNullableType()
        {
            var t0 = new TestObject();
            var bag = new ItemsBag(new[] { t0 });
            var provider = new ItemsBagTypeDescriptionProvider();
            var td = provider.GetTypeDescriptor(typeof(ItemsBag), bag);
            var p1 = td.GetProperties().Find("Checked", false);

            Assert.AreEqual(typeof(bool?), p1.PropertyType);
        }

        [Test]
        public void PropertyType_ForReferenceType_ReturnsOriginalType()
        {
            var t0 = new TestObject();
            var bag = new ItemsBag(new[] { t0 });
            var provider = new ItemsBagTypeDescriptionProvider();
            var td = provider.GetTypeDescriptor(typeof(ItemsBag), bag);
            var p1 = td.GetProperties().Find("Name", false);

            Assert.AreEqual(typeof(string), p1.PropertyType);
        }

        [Test]
        public void GetProperties_ReturnsAllPublicProperties()
        {
            var t0 = new TestObject();
            var bag = new ItemsBag(new[] { t0 });
            var provider = new ItemsBagTypeDescriptionProvider();
            var td = provider.GetTypeDescriptor(typeof(ItemsBag), bag);
            var props = td.GetProperties();

            Assert.IsNotNull(props.Find("Checked", false));
            Assert.IsNotNull(props.Find("Name", false));
            Assert.IsNotNull(props.Find("IsChecked", false));
        }

        [Test]
        public void CanResetValue_ReturnsFalse()
        {
            var t0 = new TestObject();
            var bag = new ItemsBag(new[] { t0 });
            var provider = new ItemsBagTypeDescriptionProvider();
            var td = provider.GetTypeDescriptor(typeof(ItemsBag), bag);
            var p1 = td.GetProperties().Find("Name", false);

            Assert.IsFalse(p1.CanResetValue(bag));
        }

        [Test]
        public void ShouldSerializeValue_ReturnsFalse()
        {
            var t0 = new TestObject();
            var bag = new ItemsBag(new[] { t0 });
            var provider = new ItemsBagTypeDescriptionProvider();
            var td = provider.GetTypeDescriptor(typeof(ItemsBag), bag);
            var p1 = td.GetProperties().Find("Name", false);

            Assert.IsFalse(p1.ShouldSerializeValue(bag));
        }

        private class TestObject
        {
            public bool IsChecked => this.Checked;
            public bool Checked { get; set; }
            public string Name { get; set; }
        }

        private class ObservableTestObject : System.ComponentModel.INotifyPropertyChanged
        {
            private string name;

            public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

            public string Name
            {
                get { return this.name; }
                set
                {
                    if (this.name != value)
                    {
                        this.name = value;
                        this.OnPropertyChanged("Name");
                    }
                }
            }

            protected virtual void OnPropertyChanged(string propertyName)
            {
                this.PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
}