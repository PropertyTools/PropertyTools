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
        public void SetValue_ReadOnlyProperty_ThrowsArgumentException()
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
        public void GetValue_ValueTypeWithDifferentValues_ReturnsNull()
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
        public void GetValue_ValueTypeWithEqualValues_ReturnsValue()
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
        public void GetValue_ReferenceTypeWithDifferentValues_ReturnsNull()
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
        public void GetValue_ReferenceTypeWithEqualValues_ReturnsValue()
        {
            var t0 = new TestObject() { Name = "John" };
            var t1 = new TestObject() { Name = "John" };
            var bag = new ItemsBag(new[] { t0, t1 });
            var provider = new ItemsBagTypeDescriptionProvider();
            var td = provider.GetTypeDescriptor(typeof(ItemsBag), bag);
            var p1 = td.GetProperties().Find("Name", false);
            Assert.That(p1.GetValue(bag), Is.EqualTo("John"));
        }

        [Test]
        public void Constructor_WithArray_CreatesItemsBag()
        {
            var t0 = new TestObject();
            var t1 = new TestObject();
            var bag = new ItemsBag(new[] { t0, t1 });

            Assert.That(bag, Is.Not.Null);
            Assert.That(bag.Objects.Length, Is.EqualTo(2));
            Assert.That(bag.Objects[0], Is.SameAs(t0));
            Assert.That(bag.Objects[1], Is.SameAs(t1));
        }

        [Test]
        public void Constructor_WithList_CreatesItemsBag()
        {
            var t0 = new TestObject();
            var t1 = new TestObject();
            var list = new System.Collections.Generic.List<TestObject> { t0, t1 };
            var bag = new ItemsBag(list);

            Assert.That(bag, Is.Not.Null);
            Assert.That(bag.Objects.Length, Is.EqualTo(2));
            Assert.That(bag.Objects[0], Is.SameAs(t0));
            Assert.That(bag.Objects[1], Is.SameAs(t1));
        }

        [Test]
        public void Constructor_WithSingleObject_CreatesItemsBag()
        {
            var t0 = new TestObject();
            var bag = new ItemsBag(new[] { t0 });

            Assert.That(bag, Is.Not.Null);
            Assert.That(bag.Objects.Length, Is.EqualTo(1));
            Assert.That(bag.Objects[0], Is.SameAs(t0));
        }

        [Test]
        public void Constructor_WithEmptyCollection_CreatesItemsBag()
        {
            var bag = new ItemsBag(new TestObject[0]);

            Assert.That(bag, Is.Not.Null);
            Assert.That(bag.Objects.Length, Is.EqualTo(0));
        }

        [Test]
        public void BiggestType_WithSameType_ReturnsThatType()
        {
            var t0 = new TestObject();
            var t1 = new TestObject();
            var bag = new ItemsBag(new[] { t0, t1 });

            Assert.That(bag.BiggestType, Is.EqualTo(typeof(TestObject)));
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

            Assert.That(t0.Name, Is.EqualTo("Bob"));
            Assert.That(t1.Name, Is.EqualTo("Bob"));
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

            Assert.That(t0.Checked, Is.EqualTo(true));
        }

        [Test]
        public void GetValue_WithSingleObject_ReturnsValue()
        {
            var t0 = new TestObject() { Name = "John" };
            var bag = new ItemsBag(new[] { t0 });
            var provider = new ItemsBagTypeDescriptionProvider();
            var td = provider.GetTypeDescriptor(typeof(ItemsBag), bag);
            var p1 = td.GetProperties().Find("Name", false);

            Assert.That(p1.GetValue(bag), Is.EqualTo("John"));
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

            Assert.That(p1.GetValue(bag), Is.EqualTo(null));
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

            Assert.That(p1.GetValue(bag), Is.EqualTo(null));
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
            Assert.That(p1.GetValue(bag), Is.EqualTo(10));
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
            Assert.That(p1.GetValue(bag), Is.EqualTo(null));
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
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(value));
        }

        public struct GenericStruct<T>
        {
            public T Value { get; set; }

            public GenericStruct(T value)
            {
                Value = value;
            }

            public static implicit operator GenericStruct<T>(T value)
            {
                return new GenericStruct<T>(value);
            }

            public static implicit operator T(GenericStruct<T> gs)
            {
                return gs.Value;
            }

            public override bool Equals(object obj)
            {
                if (obj is GenericStruct<T> other)
                {
                    return object.Equals(Value, other.Value);
                }
                return false;
            }

            public override int GetHashCode()
            {
                return Value?.GetHashCode() ?? 0;
            }
        }

        [Test]
        public void GenericStruct_ImplicitConversionFromValue_WorksCorrectly()
        {
            GenericStruct<int> gs = 42;
            Assert.That(gs.Value, Is.EqualTo(42));
        }

        [Test]
        public void GenericStruct_ImplicitConversionToValue_WorksCorrectly()
        {
            var gs = new GenericStruct<int>(42);
            int value = gs;
            Assert.That(value, Is.EqualTo(42));
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

            Assert.That(p1.GetValue(bag), Is.EqualTo(null));
        }

        [Test]
        public void PropertyChanged_WhenPropertySet_RaisesEvent()
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

            Assert.That(eventRaised, Is.True);
            Assert.That(propertyName, Is.EqualTo("Checked"));
        }

        [Test]
        public void PropertyChanged_WhenObjectPropertyChanges_RelaysEvent()
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

            Assert.That(eventRaised, Is.True);
            Assert.That(propertyName, Is.EqualTo("Name"));
        }

        [Test]
        public void Dispose_AfterCreation_UnsubscribesFromPropertyChangedEvents()
        {
            var t0 = new ObservableTestObject();
            var bag = new ItemsBag(new[] { t0 });

            bool eventRaised = false;
            bag.PropertyChanged += (sender, e) => { eventRaised = true; };

            bag.Dispose();

            t0.Name = "NewName";

            Assert.That(eventRaised, Is.False);
        }

        [Test]
        public void PropertyType_ForValueType_ReturnsNullableType()
        {
            var t0 = new TestObject();
            var bag = new ItemsBag(new[] { t0 });
            var provider = new ItemsBagTypeDescriptionProvider();
            var td = provider.GetTypeDescriptor(typeof(ItemsBag), bag);
            var p1 = td.GetProperties().Find("Checked", false);

            Assert.That(p1.PropertyType, Is.EqualTo(typeof(bool?)));
        }

        [Test]
        public void PropertyType_ForReferenceType_ReturnsOriginalType()
        {
            var t0 = new TestObject();
            var bag = new ItemsBag(new[] { t0 });
            var provider = new ItemsBagTypeDescriptionProvider();
            var td = provider.GetTypeDescriptor(typeof(ItemsBag), bag);
            var p1 = td.GetProperties().Find("Name", false);

            Assert.That(p1.PropertyType, Is.EqualTo(typeof(string)));
        }

        [Test]
        public void GetProperties_ForItemsBag_ReturnsAllPublicProperties()
        {
            var t0 = new TestObject();
            var bag = new ItemsBag(new[] { t0 });
            var provider = new ItemsBagTypeDescriptionProvider();
            var td = provider.GetTypeDescriptor(typeof(ItemsBag), bag);
            var props = td.GetProperties();

            Assert.That(props.Find("Checked", false), Is.Not.Null);
            Assert.That(props.Find("Name", false), Is.Not.Null);
            Assert.That(props.Find("IsChecked", false), Is.Not.Null);
        }

        [Test]
        public void CanResetValue_ForAnyProperty_ReturnsFalse()
        {
            var t0 = new TestObject();
            var bag = new ItemsBag(new[] { t0 });
            var provider = new ItemsBagTypeDescriptionProvider();
            var td = provider.GetTypeDescriptor(typeof(ItemsBag), bag);
            var p1 = td.GetProperties().Find("Name", false);

            Assert.That(p1.CanResetValue(bag), Is.False);
        }

        [Test]
        public void ShouldSerializeValue_ForAnyProperty_ReturnsFalse()
        {
            var t0 = new TestObject();
            var bag = new ItemsBag(new[] { t0 });
            var provider = new ItemsBagTypeDescriptionProvider();
            var td = provider.GetTypeDescriptor(typeof(ItemsBag), bag);
            var p1 = td.GetProperties().Find("Name", false);

            Assert.That(p1.ShouldSerializeValue(bag), Is.False);
        }

        [Test]
        public void PropertyType_ForIntValueType_ReturnsNullableInt()
        {
            var t0 = new TestObject();
            var bag = new ItemsBag(new[] { t0 });
            var provider = new ItemsBagTypeDescriptionProvider();
            var td = provider.GetTypeDescriptor(typeof(ItemsBag), bag);
            var p1 = td.GetProperties().Find("IntValue", false);

            Assert.That(p1.PropertyType, Is.EqualTo(typeof(int?)));
        }

        [Test]
        public void PropertyType_ForDoubleValueType_ReturnsNullableDouble()
        {
            var t0 = new TestObject();
            var bag = new ItemsBag(new[] { t0 });
            var provider = new ItemsBagTypeDescriptionProvider();
            var td = provider.GetTypeDescriptor(typeof(ItemsBag), bag);
            var p1 = td.GetProperties().Find("DoubleValue", false);

            Assert.That(p1.PropertyType, Is.EqualTo(typeof(double?)));
        }

        [Test]
        public void PropertyType_ForEnumValueType_ReturnsNullableEnum()
        {
            var t0 = new TestObject();
            var bag = new ItemsBag(new[] { t0 });
            var provider = new ItemsBagTypeDescriptionProvider();
            var td = provider.GetTypeDescriptor(typeof(ItemsBag), bag);
            var p1 = td.GetProperties().Find("EnumValue", false);

            Assert.That(p1.PropertyType, Is.EqualTo(typeof(TestEnum?)));
        }

        [Test]
        public void PropertyType_ForGenericStructValueType_ReturnsNullableGenericStruct()
        {
            var t0 = new TestObject();
            var bag = new ItemsBag(new[] { t0 });
            var provider = new ItemsBagTypeDescriptionProvider();
            var td = provider.GetTypeDescriptor(typeof(ItemsBag), bag);
            var p1 = td.GetProperties().Find("GenericValue", false);

            Assert.That(p1.PropertyType, Is.EqualTo(typeof(GenericStruct<int>?)));
        }

        [Test]
        public void PropertyType_ForAlreadyNullableInt_ReturnsOriginalNullableType()
        {
            var t0 = new TestObject();
            var bag = new ItemsBag(new[] { t0 });
            var provider = new ItemsBagTypeDescriptionProvider();
            var td = provider.GetTypeDescriptor(typeof(ItemsBag), bag);
            var p1 = td.GetProperties().Find("NullableInt", false);

            Assert.That(p1.PropertyType, Is.EqualTo(typeof(int?)));
        }

        [Test]
        public void GetValue_AlreadyNullableIntWithEqualValues_ReturnsValue()
        {
            var t0 = new TestObject() { NullableInt = 42 };
            var t1 = new TestObject() { NullableInt = 42 };
            var bag = new ItemsBag(new[] { t0, t1 });
            var provider = new ItemsBagTypeDescriptionProvider();
            var td = provider.GetTypeDescriptor(typeof(ItemsBag), bag);
            var p1 = td.GetProperties().Find("NullableInt", false);

            Assert.That(p1.GetValue(bag), Is.EqualTo(42));
        }

        [Test]
        public void GetValue_AlreadyNullableIntWithDifferentValues_ReturnsNull()
        {
            var t0 = new TestObject() { NullableInt = 10 };
            var t1 = new TestObject() { NullableInt = 20 };
            var bag = new ItemsBag(new[] { t0, t1 });
            var provider = new ItemsBagTypeDescriptionProvider();
            var td = provider.GetTypeDescriptor(typeof(ItemsBag), bag);
            var p1 = td.GetProperties().Find("NullableInt", false);

            Assert.That(p1.GetValue(bag), Is.EqualTo(null));
        }

        [Test]
        public void GetValue_AlreadyNullableIntWithNullValues_ReturnsNull()
        {
            var t0 = new TestObject() { NullableInt = null };
            var t1 = new TestObject() { NullableInt = null };
            var bag = new ItemsBag(new[] { t0, t1 });
            var provider = new ItemsBagTypeDescriptionProvider();
            var td = provider.GetTypeDescriptor(typeof(ItemsBag), bag);
            var p1 = td.GetProperties().Find("NullableInt", false);

            Assert.That(p1.GetValue(bag), Is.EqualTo(null));
        }

        [Test]
        public void SetValue_AlreadyNullableIntWithMultipleObjects_SetsAllValues()
        {
            var t0 = new TestObject() { NullableInt = 10 };
            var t1 = new TestObject() { NullableInt = 20 };
            var bag = new ItemsBag(new[] { t0, t1 });
            var provider = new ItemsBagTypeDescriptionProvider();
            var td = provider.GetTypeDescriptor(typeof(ItemsBag), bag);
            var p1 = td.GetProperties().Find("NullableInt", false);

            p1.SetValue(bag, 100);

            Assert.That(t0.NullableInt, Is.EqualTo(100));
            Assert.That(t1.NullableInt, Is.EqualTo(100));
        }

        [Test]
        public void SetValue_AlreadyNullableIntWithNull_SetsAllValuesToNull()
        {
            var t0 = new TestObject() { NullableInt = 10 };
            var t1 = new TestObject() { NullableInt = 20 };
            var bag = new ItemsBag(new[] { t0, t1 });
            var provider = new ItemsBagTypeDescriptionProvider();
            var td = provider.GetTypeDescriptor(typeof(ItemsBag), bag);
            var p1 = td.GetProperties().Find("NullableInt", false);

            p1.SetValue(bag, null);

            Assert.That(t0.NullableInt, Is.EqualTo(null));
            Assert.That(t1.NullableInt, Is.EqualTo(null));
        }

        [Test]
        public void GetValue_IntValueWithDifferentValues_ReturnsNull()
        {
            var t0 = new TestObject() { IntValue = 10 };
            var t1 = new TestObject() { IntValue = 20 };
            var bag = new ItemsBag(new[] { t0, t1 });
            var provider = new ItemsBagTypeDescriptionProvider();
            var td = provider.GetTypeDescriptor(typeof(ItemsBag), bag);
            var p1 = td.GetProperties().Find("IntValue", false);

            Assert.That(p1.GetValue(bag), Is.EqualTo(null));
        }

        [Test]
        public void GetValue_IntValueWithEqualValues_ReturnsValue()
        {
            var t0 = new TestObject() { IntValue = 42 };
            var t1 = new TestObject() { IntValue = 42 };
            var bag = new ItemsBag(new[] { t0, t1 });
            var provider = new ItemsBagTypeDescriptionProvider();
            var td = provider.GetTypeDescriptor(typeof(ItemsBag), bag);
            var p1 = td.GetProperties().Find("IntValue", false);

            Assert.That(p1.GetValue(bag), Is.EqualTo(42));
        }

        [Test]
        public void GetValue_DoubleValueWithDifferentValues_ReturnsNull()
        {
            var t0 = new TestObject() { DoubleValue = 1.5 };
            var t1 = new TestObject() { DoubleValue = 2.5 };
            var bag = new ItemsBag(new[] { t0, t1 });
            var provider = new ItemsBagTypeDescriptionProvider();
            var td = provider.GetTypeDescriptor(typeof(ItemsBag), bag);
            var p1 = td.GetProperties().Find("DoubleValue", false);

            Assert.That(p1.GetValue(bag), Is.EqualTo(null));
        }

        [Test]
        public void GetValue_DoubleValueWithEqualValues_ReturnsValue()
        {
            var t0 = new TestObject() { DoubleValue = 3.14 };
            var t1 = new TestObject() { DoubleValue = 3.14 };
            var bag = new ItemsBag(new[] { t0, t1 });
            var provider = new ItemsBagTypeDescriptionProvider();
            var td = provider.GetTypeDescriptor(typeof(ItemsBag), bag);
            var p1 = td.GetProperties().Find("DoubleValue", false);

            Assert.That(p1.GetValue(bag), Is.EqualTo(3.14));
        }

        [Test]
        public void GetValue_EnumValueWithDifferentValues_ReturnsNull()
        {
            var t0 = new TestObject() { EnumValue = TestEnum.First };
            var t1 = new TestObject() { EnumValue = TestEnum.Second };
            var bag = new ItemsBag(new[] { t0, t1 });
            var provider = new ItemsBagTypeDescriptionProvider();
            var td = provider.GetTypeDescriptor(typeof(ItemsBag), bag);
            var p1 = td.GetProperties().Find("EnumValue", false);

            Assert.That(p1.GetValue(bag), Is.EqualTo(null));
        }

        [Test]
        public void GetValue_EnumValueWithEqualValues_ReturnsValue()
        {
            var t0 = new TestObject() { EnumValue = TestEnum.Third };
            var t1 = new TestObject() { EnumValue = TestEnum.Third };
            var bag = new ItemsBag(new[] { t0, t1 });
            var provider = new ItemsBagTypeDescriptionProvider();
            var td = provider.GetTypeDescriptor(typeof(ItemsBag), bag);
            var p1 = td.GetProperties().Find("EnumValue", false);

            Assert.That(p1.GetValue(bag), Is.EqualTo(TestEnum.Third));
        }

        [Test]
        public void SetValue_IntValueWithMultipleObjects_SetsAllValues()
        {
            var t0 = new TestObject() { IntValue = 10 };
            var t1 = new TestObject() { IntValue = 20 };
            var bag = new ItemsBag(new[] { t0, t1 });
            var provider = new ItemsBagTypeDescriptionProvider();
            var td = provider.GetTypeDescriptor(typeof(ItemsBag), bag);
            var p1 = td.GetProperties().Find("IntValue", false);

            p1.SetValue(bag, 100);

            Assert.That(t0.IntValue, Is.EqualTo(100));
            Assert.That(t1.IntValue, Is.EqualTo(100));
        }

        [Test]
        public void SetValue_DoubleValueWithMultipleObjects_SetsAllValues()
        {
            var t0 = new TestObject() { DoubleValue = 1.5 };
            var t1 = new TestObject() { DoubleValue = 2.5 };
            var bag = new ItemsBag(new[] { t0, t1 });
            var provider = new ItemsBagTypeDescriptionProvider();
            var td = provider.GetTypeDescriptor(typeof(ItemsBag), bag);
            var p1 = td.GetProperties().Find("DoubleValue", false);

            p1.SetValue(bag, 9.99);

            Assert.That(t0.DoubleValue, Is.EqualTo(9.99));
            Assert.That(t1.DoubleValue, Is.EqualTo(9.99));
        }

        [Test]
        public void SetValue_EnumValueWithMultipleObjects_SetsAllValues()
        {
            var t0 = new TestObject() { EnumValue = TestEnum.First };
            var t1 = new TestObject() { EnumValue = TestEnum.Second };
            var bag = new ItemsBag(new[] { t0, t1 });
            var provider = new ItemsBagTypeDescriptionProvider();
            var td = provider.GetTypeDescriptor(typeof(ItemsBag), bag);
            var p1 = td.GetProperties().Find("EnumValue", false);

            p1.SetValue(bag, TestEnum.Third);

            Assert.That(t0.EnumValue, Is.EqualTo(TestEnum.Third));
            Assert.That(t1.EnumValue, Is.EqualTo(TestEnum.Third));
        }

        [Test]
        public void SetValue_GenericStructValueWithMultipleObjects_SetsAllValues()
        {
            var t0 = new TestObject() { GenericValue = new GenericStruct<int>(10) };
            var t1 = new TestObject() { GenericValue = new GenericStruct<int>(20) };
            var bag = new ItemsBag(new[] { t0, t1 });
            var provider = new ItemsBagTypeDescriptionProvider();
            var td = provider.GetTypeDescriptor(typeof(ItemsBag), bag);
            var p1 = td.GetProperties().Find("GenericValue", false);

            var newValue = new GenericStruct<int>(100);
            p1.SetValue(bag, newValue);

            Assert.That(t0.GenericValue, Is.EqualTo(newValue));
            Assert.That(t1.GenericValue, Is.EqualTo(newValue));
        }

        private class TestObject
        {
            public bool IsChecked => this.Checked;
            public bool Checked { get; set; }
            public string Name { get; set; }
            public int? NullableInt { get; set; }
            public GenericStruct<int> GenericValue { get; set; }
            public int IntValue { get; set; }
            public double DoubleValue { get; set; }
            public TestEnum EnumValue { get; set; }
        }

        private enum TestEnum
        {
            None,
            First,
            Second,
            Third
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