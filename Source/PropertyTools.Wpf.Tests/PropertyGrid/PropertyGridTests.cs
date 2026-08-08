// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertyGridTests.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf.Tests.PropertyGridNamespace
{
    using System.Collections.ObjectModel;
    using System.Threading;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Markup;
    using System.Windows.Media;
    using NUnit.Framework;
    using PropertyTools.Wpf;

    [TestFixture]
    [Apartment(ApartmentState.STA)]
    public class PropertyGridTests
    {
        private const string TemplateXaml = @"
<ControlTemplate xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""
                 xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""
                 xmlns:pt=""clr-namespace:PropertyTools.Wpf;assembly=PropertyTools.Wpf""
                 TargetType=""{x:Type pt:PropertyGrid}"">
    <Grid>
        <TabControl x:Name=""PART_Tabs"" />
        <ScrollViewer x:Name=""PART_ScrollViewer"">
            <StackPanel x:Name=""PART_Panel"" />
        </ScrollViewer>
    </Grid>
</ControlTemplate>";

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
        public void ControlFactory_SelectedObjectSetBeforeFactory_UsesUpdatedFactory()
        {
            // Arrange
            var propertyGrid = CreateTemplatedPropertyGrid();
            propertyGrid.SelectedObject = new TestObject { TestProperty = "TestValue" };
            var factory = new CountingControlFactory();

            // Act
            propertyGrid.ControlFactory = factory;

            // Assert
            Assert.That(factory.CreateControlCallCount, Is.GreaterThan(0));
        }

        private static PropertyGrid CreateTemplatedPropertyGrid()
        {
            var template = (ControlTemplate)XamlReader.Parse(TemplateXaml);
            var propertyGrid = new PropertyGrid { Template = template };
            propertyGrid.ApplyTemplate();
            return propertyGrid;
        }

        private class TestObject
        {
            public string TestProperty { get; set; }
        }

        private class CountingControlFactory : PropertyGridControlFactory
        {
            public int CreateControlCallCount { get; private set; }

            public override FrameworkElement CreateControl(PropertyItem property, PropertyControlFactoryOptions options, object instance = null)
            {
                this.CreateControlCallCount++;
                return base.CreateControl(property, options, instance);
            }
        }
    }
}
