// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertyGridEditorTests.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf.Tests.PropertyGridNamespace
{
    using System.Linq;
    using System.Windows.Controls;

    using NUnit.Framework;

    using PropertyTools.Wpf;

    /// <summary>
    /// Headless component tests that verify the <see cref="PropertyGrid" /> generates
    /// the expected editors in its visual tree for common property types.
    /// </summary>
    public class PropertyGridEditorTests : WpfTestBase
    {
        [Test]
        public void SelectedObject_StringProperty_GeneratesTextBoxEditor()
        {
            // Arrange
            var propertyGrid = new PropertyGrid { SelectedObject = new EditorTestModel() };

            // Act
            PrepareForLayout(propertyGrid);
            DoEvents();
            propertyGrid.UpdateLayout();

            // Assert
            var textBoxes = FindVisualChildren<TextBox>(propertyGrid).ToList();
            Assert.That(textBoxes, Is.Not.Empty, "A TextBox editor should be generated for the string property");
        }

        [Test]
        public void SelectedObject_BoolProperty_GeneratesCheckBoxEditor()
        {
            // Arrange
            var propertyGrid = new PropertyGrid { SelectedObject = new EditorTestModel() };

            // Act
            PrepareForLayout(propertyGrid);
            DoEvents();
            propertyGrid.UpdateLayout();

            // Assert
            var checkBoxes = FindVisualChildren<CheckBox>(propertyGrid).ToList();
            Assert.That(checkBoxes, Is.Not.Empty, "A CheckBox editor should be generated for the bool property");
        }

        [Test]
        public void SelectedObject_ChangedToNull_ClearsCurrentObject()
        {
            // Arrange
            var propertyGrid = new PropertyGrid { SelectedObject = new EditorTestModel() };
            PrepareForLayout(propertyGrid);

            // Act
            propertyGrid.SelectedObject = null;

            // Assert
            Assert.That(propertyGrid.CurrentObject, Is.Null);
        }

        /// <summary>
        /// A simple model used to verify editor generation.
        /// </summary>
        private class EditorTestModel
        {
            public string Name { get; set; } = "Test";

            public bool IsEnabled { get; set; } = true;
        }
    }
}
