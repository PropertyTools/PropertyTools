// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertyGridOperatorTests.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf.Tests.PropertyGridNamespace
{
    using System;
    using System.ComponentModel;
    using System.Data.Common;
    using System.Linq;
    using System.Threading;

    using NUnit.Framework;

    using PropertyTools.Wpf;

    using Category = System.ComponentModel.CategoryAttribute;

    /// <summary>
    /// Unit tests for <see cref="PropertyGridOperator" /> focusing on the fix for issue #288
    /// (PropertyGrid breaks when bound to a <see cref="DbConnectionStringBuilder" /> subclass).
    /// </summary>
    [TestFixture]
    [Apartment(ApartmentState.STA)]
    public class PropertyGridOperatorTests
    {
        // -----------------------------------------------------------------------
        // Helpers
        // -----------------------------------------------------------------------

        /// <summary>
        /// Exposes the protected <c>GetPropertyCollection</c> method for testing.
        /// </summary>
        private class TestableOperator : PropertyGridOperator
        {
            public PropertyDescriptorCollection GetPropertyCollectionPublic(object instance)
                => this.GetPropertyCollection(instance);
        }

        /// <summary>
        /// A minimal <see cref="DbConnectionStringBuilder" /> subclass that mirrors the pattern
        /// used by FirebirdSql.Data.FirebirdClient.FbConnectionStringBuilder (issue #288).
        /// Properties use typed CLR getters/setters that read from / write to the underlying
        /// string dictionary, so the dictionary's raw values differ from the typed property values.
        /// </summary>
        private class MinimalConnectionStringBuilder : DbConnectionStringBuilder
        {
            [Category("Connection")]
            [DefaultValue(true)]
            public bool Pooling
            {
                get => this.TryGetValue("pooling", out var v) && bool.TryParse(v?.ToString(), out var b) ? b : true;
                set => this["pooling"] = value;
            }

            [Category("Connection")]
            [DefaultValue(false)]
            public bool Compression
            {
                get => this.TryGetValue("compress", out var v) && bool.TryParse(v?.ToString(), out var b) && b;
                set => this["compress"] = value;
            }

            [Category("Source")]
            [DefaultValue(FakeServer.Default)]
            public FakeServer ServerType
            {
                get
                {
                    if (this.TryGetValue("server type", out var v) &&
                        int.TryParse(v?.ToString(), out var i) &&
                        System.Enum.IsDefined(typeof(FakeServer), i))
                    {
                        return (FakeServer)i;
                    }
                    return FakeServer.Default;
                }
                set => this["server type"] = (int)value;
            }

            [Category("Connection")]
            [DefaultValue(3050)]
            public int Port
            {
                get => this.TryGetValue("port", out var v) && int.TryParse(v?.ToString(), out var i) ? i : 3050;
                set => this["port"] = value;
            }
        }

        private enum FakeServer { Default = 0, Embedded = 1 }

        private class InheritedCategoryTestOptions : IPropertyGridOptions
        {
            public Type RequiredAttribute => null;

            public bool ShowDeclaredOnly => false;

            public bool ShowReadOnlyProperties => true;
        }

        private class OrderedBaseModel
        {
            [PropertyTools.DataAnnotations.Category("Inherited|Base category", groupSortIndex: 0)]
            public int BaseValue { get; set; }
        }

        private class OrderedDerivedModel : OrderedBaseModel
        {
            [PropertyTools.DataAnnotations.Category("Inherited|Derived category", groupSortIndex: 1)]
            public int DerivedValue { get; set; }
        }

        // -----------------------------------------------------------------------
        // Tests for GetPropertyCollection with ICustomTypeDescriptor objects
        // -----------------------------------------------------------------------

        [Test]
        public void GetPropertyCollection_ICustomTypeDescriptorObject_BoolDescriptorGetValueReturnsTypedBool()
        {
            // Arrange
            var op = new TestableOperator();
            var builder = new MinimalConnectionStringBuilder();

            // Act
            var props = op.GetPropertyCollectionPublic(builder);
            var pd = props["Pooling"];

            // Assert – the descriptor must be present and GetValue must return a bool,
            // not null (unset key) or a raw string ("True").
            Assert.That(pd, Is.Not.Null, "Descriptor for 'Pooling' should exist");
            var value = pd.GetValue(builder);
            Assert.That(value, Is.TypeOf<bool>(), "GetValue should return bool, not null or string");
        }

        [Test]
        public void GetPropertyCollection_ICustomTypeDescriptorObject_BoolDescriptorGetValueReflectsDefaultValue()
        {
            // Arrange
            var op = new TestableOperator();
            var builder = new MinimalConnectionStringBuilder();

            // Act – "Pooling" has never been set; the CLR getter returns the default (true)
            var props = op.GetPropertyCollectionPublic(builder);
            var pd = props["Pooling"];

            // Assert
            Assert.That((bool)pd.GetValue(builder), Is.True,
                "Unset Pooling should return default value true via CLR property getter");
        }

        [Test]
        public void GetPropertyCollection_ICustomTypeDescriptorObject_BoolDescriptorGetValueReturnsUpdatedValue()
        {
            // Arrange
            var op = new TestableOperator();
            var builder = new MinimalConnectionStringBuilder { Compression = true };

            // Act
            var props = op.GetPropertyCollectionPublic(builder);
            var pd = props["Compression"];

            // Assert
            var value = pd.GetValue(builder);
            Assert.That(value, Is.TypeOf<bool>());
            Assert.That((bool)value, Is.True);
        }

        [Test]
        public void GetPropertyCollection_ICustomTypeDescriptorObject_EnumDescriptorGetValueReturnsTypedEnum()
        {
            // Arrange
            var op = new TestableOperator();
            var builder = new MinimalConnectionStringBuilder();

            // Act
            var props = op.GetPropertyCollectionPublic(builder);
            var pd = props["ServerType"];

            // Assert – must be the enum type, not null or a string
            Assert.That(pd, Is.Not.Null, "Descriptor for 'ServerType' should exist");
            var value = pd.GetValue(builder);
            Assert.That(value, Is.TypeOf<FakeServer>(),
                "GetValue for enum property should return the enum type, not null or int string");
        }

        [Test]
        public void GetPropertyCollection_ICustomTypeDescriptorObject_IntDescriptorGetValueReturnsTypedInt()
        {
            // Arrange
            var op = new TestableOperator();
            var builder = new MinimalConnectionStringBuilder();

            // Act
            var props = op.GetPropertyCollectionPublic(builder);
            var pd = props["Port"];

            // Assert
            Assert.That(pd, Is.Not.Null);
            var value = pd.GetValue(builder);
            Assert.That(value, Is.TypeOf<int>());
            Assert.That((int)value, Is.EqualTo(3050));
        }

        [Test]
        public void GetPropertyCollection_ICustomTypeDescriptorObject_DescriptorPropertyTypeMatchesClrType()
        {
            // Arrange
            var op = new TestableOperator();
            var builder = new MinimalConnectionStringBuilder();

            // Act
            var props = op.GetPropertyCollectionPublic(builder);

            // Assert – descriptors should have the correct CLR property types
            Assert.That(props["Pooling"].PropertyType, Is.EqualTo(typeof(bool)));
            Assert.That(props["ServerType"].PropertyType, Is.EqualTo(typeof(FakeServer)));
            Assert.That(props["Port"].PropertyType, Is.EqualTo(typeof(int)));
        }

        // -----------------------------------------------------------------------
        // Tests for normal (non-ICustomTypeDescriptor) objects – ensure no regression
        // -----------------------------------------------------------------------

        private class PlainModel
        {
            public bool Flag { get; set; } = true;
            public string Name { get; set; } = "test";
            public int Count { get; set; } = 42;
        }

        [Test]
        public void GetPropertyCollection_NormalObject_BoolDescriptorGetValueReturnsTypedBool()
        {
            // Arrange
            var op = new TestableOperator();
            var model = new PlainModel();

            // Act
            var props = op.GetPropertyCollectionPublic(model);
            var pd = props["Flag"];

            // Assert – normal objects should continue working as before
            Assert.That(pd, Is.Not.Null);
            var value = pd.GetValue(model);
            Assert.That(value, Is.TypeOf<bool>());
            Assert.That((bool)value, Is.True);
        }

        [Test]
        public void GetPropertyCollection_NormalObject_StringDescriptorGetValueReturnsString()
        {
            // Arrange
            var op = new TestableOperator();
            var model = new PlainModel();

            // Act
            var props = op.GetPropertyCollectionPublic(model);
            var pd = props["Name"];

            // Assert
            Assert.That((string)pd.GetValue(model), Is.EqualTo("test"));
        }

        [Test]
        public void GetPropertyCollection_NormalObject_IntDescriptorGetValueReturnsInt()
        {
            // Arrange
            var op = new TestableOperator();
            var model = new PlainModel();

            // Act
            var props = op.GetPropertyCollectionPublic(model);
            var pd = props["Count"];

            // Assert
            Assert.That((int)pd.GetValue(model), Is.EqualTo(42));
        }

        [Test]
        public void CreateModel_InheritedCategorySortIndexesSpecified_GroupsFollowExplicitOrder()
        {
            // Arrange
            var op = new PropertyGridOperator();
            var model = new OrderedDerivedModel();

            // Act
            var tabs = op.CreateModel(model, false, new InheritedCategoryTestOptions()).ToList();

            // Assert
            Assert.That(tabs, Has.Count.EqualTo(1));
            Assert.That(tabs[0].Groups.Select(g => g.Name).ToArray(), Is.EqualTo(new[] { "Base category", "Derived category" }));
        }
    }
}
