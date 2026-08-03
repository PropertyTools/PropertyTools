// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataGridEditingTests.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf.Tests
{
    using System.Reflection;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;

    using NUnit.Framework;

    using PropertyTools.Wpf;

    [TestFixture]
    [Apartment(System.Threading.ApartmentState.STA)]
    public class DataGridEditingTests
    {
        [Test]
        public void RemoveEditControl_VisibleTextEditorAndUpdateRequested_UpdatesSourceBeforeRemovingEditor()
        {
            // Arrange
            var source = new TextEditSource { Value = "0" };
            var textEditor = this.CreateTextEditor(source);
            textEditor.Text = "5";
            textEditor.Visibility = Visibility.Visible;

            var dataGrid = new PropertyTools.Wpf.DataGrid();
            var sheetGrid = new Grid();
            sheetGrid.Children.Add(textEditor);

            SetPrivateField(dataGrid, "sheetGrid", sheetGrid);
            SetPrivateField(dataGrid, "currentEditControl", textEditor);

            // Act
            InvokeRemoveEditControl(dataGrid, true);

            // Assert
            Assert.That(source.Value, Is.EqualTo("5"));
            Assert.That(sheetGrid.Children.Contains(textEditor), Is.False);
        }

        [Test]
        public void RemoveEditControl_HiddenTextEditorAndUpdateRequested_DoesNotUpdateSource()
        {
            // Arrange — simulates the pre-created hidden editor created by ShowEditControl()
            var source = new TextEditSource { Value = "0" };
            var textEditor = this.CreateTextEditor(source);
            textEditor.Text = "5";
            textEditor.Visibility = Visibility.Hidden;

            var dataGrid = new PropertyTools.Wpf.DataGrid();
            var sheetGrid = new Grid();
            sheetGrid.Children.Add(textEditor);

            SetPrivateField(dataGrid, "sheetGrid", sheetGrid);
            SetPrivateField(dataGrid, "currentEditControl", textEditor);

            // Act
            InvokeRemoveEditControl(dataGrid, true);

            // Assert — hidden editor must not commit: it was never actively edited
            Assert.That(source.Value, Is.EqualTo("0"));
            Assert.That(sheetGrid.Children.Contains(textEditor), Is.False);
        }

        [Test]
        public void RemoveEditControl_TextEditorAndNoUpdateRequested_DoesNotUpdateSource()
        {
            // Arrange
            var source = new TextEditSource { Value = "0" };
            var textEditor = this.CreateTextEditor(source);
            textEditor.Text = "5";
            textEditor.Visibility = Visibility.Visible;

            var dataGrid = new PropertyTools.Wpf.DataGrid();
            var sheetGrid = new Grid();
            sheetGrid.Children.Add(textEditor);

            SetPrivateField(dataGrid, "sheetGrid", sheetGrid);
            SetPrivateField(dataGrid, "currentEditControl", textEditor);

            // Act
            InvokeRemoveEditControl(dataGrid, false);

            // Assert
            Assert.That(source.Value, Is.EqualTo("0"));
            Assert.That(sheetGrid.Children.Contains(textEditor), Is.False);
        }

        private static void InvokeRemoveEditControl(PropertyTools.Wpf.DataGrid dataGrid, bool updateTextBindingSource)
        {
            var removeEditControl = typeof(PropertyTools.Wpf.DataGrid).GetMethod("RemoveEditControl", BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.That(removeEditControl, Is.Not.Null);
            removeEditControl.Invoke(dataGrid, new object[] { updateTextBindingSource });
        }

        private static void SetPrivateField(object target, string fieldName, object value)
        {
            var fieldInfo = target.GetType().GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.That(fieldInfo, Is.Not.Null);
            fieldInfo.SetValue(target, value);
        }

        private TextBox CreateTextEditor(TextEditSource source)
        {
            var textEditor = new TextBox();
            var binding = new Binding(nameof(TextEditSource.Value))
            {
                Source = source,
                Mode = BindingMode.TwoWay,
                UpdateSourceTrigger = UpdateSourceTrigger.Explicit
            };
            textEditor.SetBinding(TextBox.TextProperty, binding);
            return textEditor;
        }

        private class TextEditSource
        {
            public string Value { get; set; }
        }
    }
}
