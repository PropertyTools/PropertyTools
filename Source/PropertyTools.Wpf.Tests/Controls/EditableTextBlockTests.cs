// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EditableTextBlockTests.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf.Tests
{
    using NUnit.Framework;

    using PropertyTools.Wpf;

    /// <summary>
    /// Headless component tests for <see cref="EditableTextBlock" />.
    /// </summary>
    public class EditableTextBlockTests : WpfTestBase
    {
        [Test]
        public void Text_SetValue_RoundTrips()
        {
            // Arrange
            var editableTextBlock = new EditableTextBlock();

            // Act
            editableTextBlock.Text = "Hello";

            // Assert
            Assert.That(editableTextBlock.Text, Is.EqualTo("Hello"));
        }

        [Test]
        public void IsEditing_DefaultValue_IsFalse()
        {
            // Arrange / Act
            var editableTextBlock = new EditableTextBlock();

            // Assert
            Assert.That(editableTextBlock.IsEditing, Is.False);
        }

        [Test]
        public void IsEditing_SetToTrueInsidePanel_ShowsTextBoxEditor()
        {
            // Arrange
            var editableTextBlock = new EditableTextBlock { Text = "Hello" };
            var panel = new System.Windows.Controls.Grid();
            panel.Children.Add(editableTextBlock);
            PrepareForLayout(panel);

            // Act
            editableTextBlock.IsEditing = true;

            // Assert
            Assert.That(panel.Children.Count, Is.EqualTo(2), "A TextBox editor should be inserted into the panel");
            Assert.That(panel.Children[0], Is.InstanceOf<System.Windows.Controls.TextBox>());
            Assert.That(editableTextBlock.Visibility, Is.EqualTo(System.Windows.Visibility.Collapsed));
        }

        [Test]
        public void IsEditing_SetToFalseAfterEditing_RemovesTextBoxEditor()
        {
            // Arrange
            var editableTextBlock = new EditableTextBlock { Text = "Hello" };
            var panel = new System.Windows.Controls.Grid();
            panel.Children.Add(editableTextBlock);
            PrepareForLayout(panel);
            editableTextBlock.IsEditing = true;

            // Act
            editableTextBlock.IsEditing = false;

            // Assert
            Assert.That(panel.Children.Count, Is.EqualTo(1), "The TextBox editor should be removed from the panel");
            Assert.That(editableTextBlock.Visibility, Is.EqualTo(System.Windows.Visibility.Visible));
        }
    }
}
