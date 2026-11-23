// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertyGridTests.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf.Tests.PropertyGridNamespace
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using System.Threading;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;
    using NUnit.Framework;
    using PropertyTools.Wpf;

    [TestFixture]
    [Apartment(ApartmentState.STA)]
    public class PropertyGridTests
    {
        [Test]
        public void PropertyPanelStyle_DefaultValue_IsNull()
        {
            var propertyGrid = new PropertyGrid();
            Assert.That(propertyGrid.PropertyPanelStyle, Is.Null);
        }

        [Test]
        public void LabelPanelStyle_DefaultValue_IsNull()
        {
            var propertyGrid = new PropertyGrid();
            Assert.That(propertyGrid.LabelPanelStyle, Is.Null);
        }

        [Test]
        public void PropertyPanelStyle_SetValue_ReturnsSetValue()
        {
            // Arrange
            var propertyGrid = new PropertyGrid();
            var style = new Style(typeof(Grid));
            style.Setters.Add(new Setter(Grid.BackgroundProperty, Brushes.DarkGray));

            // Act
            propertyGrid.PropertyPanelStyle = style;

            // Assert
            Assert.That(propertyGrid.PropertyPanelStyle, Is.SameAs(style));
        }

        [Test]
        public void LabelPanelStyle_SetValue_ReturnsSetValue()
        {
            // Arrange
            var propertyGrid = new PropertyGrid();
            var style = new Style(typeof(DockPanel));
            style.Setters.Add(new Setter(DockPanel.BackgroundProperty, Brushes.DimGray));

            // Act
            propertyGrid.LabelPanelStyle = style;

            // Assert
            Assert.That(propertyGrid.LabelPanelStyle, Is.SameAs(style));
        }

        [Test]
        public void SelectedObjects_BindingToObservableCollection_InitializesCurrentObject()
        {
            // Arrange
            var propertyGrid = new PropertyGrid();
            var testCollection = new ObservableCollection<TestObject>
            {
                new TestObject { TestProperty = "Object1" },
                new TestObject { TestProperty = "Object2" }
            };

            // Act
            propertyGrid.SelectedObjects = testCollection;

            // Assert
            Assert.That(propertyGrid.CurrentObject, Is.Not.Null, "CurrentObject should be initialized when SelectedObjects is set");
        }

        [Test]
        public void SelectedObjects_BindingToObservableCollectionWithSingleItem_SetsCurrentObjectToThatItem()
        {
            // Arrange
            var propertyGrid = new PropertyGrid();
            var testObject = new TestObject { TestProperty = "TestValue" };
            var testCollection = new ObservableCollection<TestObject> { testObject };

            // Act
            propertyGrid.SelectedObjects = testCollection;

            // Assert
            Assert.That(propertyGrid.CurrentObject, Is.EqualTo(testObject), "CurrentObject should be the single item in collection");
        }

        [Test]
        public void SelectedObjects_BindingToObservableCollectionWithMultipleItems_SetsCurrentObjectToItemsBag()
        {
            // Arrange
            var propertyGrid = new PropertyGrid();
            var testCollection = new ObservableCollection<TestObject>
            {
                new TestObject { TestProperty = "Object1" },
                new TestObject { TestProperty = "Object2" }
            };

            // Act
            propertyGrid.SelectedObjects = testCollection;

            // Assert
            Assert.That(propertyGrid.CurrentObject, Is.TypeOf<ItemsBag>(), "CurrentObject should be ItemsBag for multiple items");
        }

        [Test]
        public void SelectedObjects_CollectionChanged_UpdatesCurrentObject()
        {
            // Arrange
            var propertyGrid = new PropertyGrid();
            var testCollection = new ObservableCollection<TestObject>
            {
                new TestObject { TestProperty = "Object1" }
            };
            propertyGrid.SelectedObjects = testCollection;

            // Act
            testCollection.Add(new TestObject { TestProperty = "Object2" });

            // Assert
            Assert.That(propertyGrid.CurrentObject, Is.TypeOf<ItemsBag>(), "CurrentObject should update to ItemsBag when collection grows to multiple items");
        }

        [Test]
        public void SelectedObjects_CollectionClearedAfterInitialization_SetsCurrentObjectToNull()
        {
            // Arrange
            var propertyGrid = new PropertyGrid();
            var testCollection = new ObservableCollection<TestObject>
            {
                new TestObject { TestProperty = "Object1" }
            };
            propertyGrid.SelectedObjects = testCollection;

            // Act
            testCollection.Clear();

            // Assert
            Assert.That(propertyGrid.CurrentObject, Is.Null, "CurrentObject should be null when collection is cleared");
        }

        [Test]
        public void SelectedObjects_SetToNull_ClearsCurrentObject()
        {
            // Arrange
            var propertyGrid = new PropertyGrid();
            var testCollection = new ObservableCollection<TestObject>
            {
                new TestObject { TestProperty = "Object1" }
            };
            propertyGrid.SelectedObjects = testCollection;

            // Act
            propertyGrid.SelectedObjects = null;

            // Assert
            Assert.That(propertyGrid.CurrentObject, Is.Null, "CurrentObject should be null when SelectedObjects is set to null");
        }

        [Test]
        public void Refresh_WithSelectedObject_RecreatesModel()
        {
            // Arrange
            var countingOperator = new CountingOperator();
            var propertyGrid = new PropertyGrid { Operator = countingOperator };
            var testObject = new TestObject { TestProperty = "TestValue" };
            propertyGrid.SelectedObject = testObject;
            var callsBeforeRefresh = countingOperator.CreateModelCount;

            // Act
            propertyGrid.Refresh();

            // Assert
            Assert.That(countingOperator.CreateModelCount, Is.EqualTo(callsBeforeRefresh + 1));
            Assert.That(countingOperator.LastInstance, Is.SameAs(testObject));
        }

        [Test]
        public void Refresh_WithoutSelectedObject_DoesNotThrow()
        {
            // Arrange
            var propertyGrid = new PropertyGrid();

            // Act & Assert
            Assert.That(() => propertyGrid.Refresh(), Throws.Nothing);
        }

        [Test]
        public void Refresh_DynamicPropertiesAddedAndRemoved_ModelReflectsNewPropertySet()
        {
            // Arrange
            var countingOperator = new CountingOperator();
            var propertyGrid = new PropertyGrid { Operator = countingOperator };
            var dynamicObject = new DynamicTestObject();
            dynamicObject.AddProperty("First", "1");
            dynamicObject.AddProperty("Second", "2");
            propertyGrid.SelectedObject = dynamicObject;
            Assert.That(GetPropertyNames(countingOperator.LastModel), Is.EquivalentTo(new[] { "First", "Second" }));

            dynamicObject.RemoveProperty("First");
            dynamicObject.AddProperty("Third", "3");

            // Act
            propertyGrid.Refresh();

            // Assert
            Assert.That(countingOperator.LastInstance, Is.SameAs(dynamicObject));
            Assert.That(GetPropertyNames(countingOperator.LastModel), Is.EquivalentTo(new[] { "Second", "Third" }));
        }

        private static List<string> GetPropertyNames(IEnumerable<Tab> model)
        {
            return model.SelectMany(t => t.Groups).SelectMany(g => g.Properties).Select(p => p.PropertyName).ToList();
        }

        private class TestObject
        {
            public string TestProperty { get; set; }
        }

        private class CountingOperator : PropertyGridOperator
        {
            public int CreateModelCount { get; private set; }

            public object LastInstance { get; private set; }

            public List<Tab> LastModel { get; private set; }

            public override IEnumerable<Tab> CreateModel(object instance, bool isEnumerable, IPropertyGridOptions options)
            {
                this.CreateModelCount++;
                this.LastInstance = instance;
                var model = base.CreateModel(instance, isEnumerable, options);
                this.LastModel = model != null ? model.ToList() : null;
                return this.LastModel;
            }
        }

        /// <summary>
        /// An object whose set of properties can be changed at runtime through <see cref="ICustomTypeDescriptor" />.
        /// </summary>
        private class DynamicTestObject : ICustomTypeDescriptor
        {
            private readonly Dictionary<string, object> values = new Dictionary<string, object>();

            public void AddProperty(string name, object value)
            {
                this.values[name] = value;
            }

            public void RemoveProperty(string name)
            {
                this.values.Remove(name);
            }

            public AttributeCollection GetAttributes() => TypeDescriptor.GetAttributes(this, true);

            public string GetClassName() => TypeDescriptor.GetClassName(this, true);

            public string GetComponentName() => TypeDescriptor.GetComponentName(this, true);

            public TypeConverter GetConverter() => TypeDescriptor.GetConverter(this, true);

            public EventDescriptor GetDefaultEvent() => TypeDescriptor.GetDefaultEvent(this, true);

            public PropertyDescriptor GetDefaultProperty() => null;

            public object GetEditor(Type editorBaseType) => TypeDescriptor.GetEditor(this, editorBaseType, true);

            public EventDescriptorCollection GetEvents() => TypeDescriptor.GetEvents(this, true);

            public EventDescriptorCollection GetEvents(Attribute[] attributes) => TypeDescriptor.GetEvents(this, attributes, true);

            public PropertyDescriptorCollection GetProperties() => this.GetProperties(null);

            public PropertyDescriptorCollection GetProperties(Attribute[] attributes)
            {
                var descriptors = this.values.Keys.Select(k => (PropertyDescriptor)new DynamicPropertyDescriptor(k)).ToArray();
                return new PropertyDescriptorCollection(descriptors);
            }

            public object GetPropertyOwner(PropertyDescriptor pd) => this;

            private class DynamicPropertyDescriptor : PropertyDescriptor
            {
                public DynamicPropertyDescriptor(string name)
                    : base(name, null)
                {
                }

                public override Type ComponentType => typeof(DynamicTestObject);

                public override bool IsReadOnly => false;

                public override Type PropertyType => typeof(string);

                public override bool CanResetValue(object component) => false;

                public override object GetValue(object component)
                {
                    object value;
                    return ((DynamicTestObject)component).values.TryGetValue(this.Name, out value) ? value : null;
                }

                public override void ResetValue(object component)
                {
                }

                public override void SetValue(object component, object value)
                {
                    ((DynamicTestObject)component).values[this.Name] = value;
                }

                public override bool ShouldSerializeValue(object component) => false;
            }
        }
    }
}
