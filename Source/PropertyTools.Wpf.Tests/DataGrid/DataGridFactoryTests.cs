// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataGridFactoryTests.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf.Tests
{
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Markup;

    using NUnit.Framework;

    using PropertyTools.Wpf;

    [TestFixture]
    [Apartment(System.Threading.ApartmentState.STA)]
    public class DataGridFactoryTests
    {
        private const string TemplateXaml = @"
<ControlTemplate xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""
                 xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""
                 xmlns:pt=""clr-namespace:PropertyTools.Wpf;assembly=PropertyTools.Wpf""
                 TargetType=""{x:Type pt:DataGrid}"">
    <Grid x:Name=""PART_Grid"">
        <ScrollViewer x:Name=""PART_SheetScrollViewer"" Focusable=""False"">
            <Grid x:Name=""PART_SheetGrid"">
                <Border x:Name=""PART_SelectionBackground"" />
                <Border x:Name=""PART_CurrentBackground"" />
                <Border x:Name=""PART_Selection"" />
                <Border x:Name=""PART_AutoFillSelection"" Visibility=""Hidden"" />
                <Border x:Name=""PART_AutoFillBox"" />
            </Grid>
        </ScrollViewer>
        <ScrollViewer x:Name=""PART_ColumnScrollViewer"">
            <Grid x:Name=""PART_ColumnGrid"">
                <Border x:Name=""PART_ColumnSelectionBackground"" />
            </Grid>
        </ScrollViewer>
        <ScrollViewer x:Name=""PART_RowScrollViewer"">
            <Grid x:Name=""PART_RowGrid"">
                <Border x:Name=""PART_RowSelectionBackground"" />
            </Grid>
        </ScrollViewer>
        <Border x:Name=""PART_TopLeft"" />
    </Grid>
</ControlTemplate>";

        [Test]
        public void CellDefinitionFactory_ChangedAfterGridWasBuilt_RebuildsGridContentWithNewFactory()
        {
            // Arrange
            var dataGrid = CreateTemplatedDataGrid();
            dataGrid.ItemsSource = new List<int> { 1, 2, 3 };
            var factory = new CountingCellDefinitionFactory();

            // Act
            dataGrid.CellDefinitionFactory = factory;

            // Assert
            Assert.That(factory.CreateCellDefinitionCallCount, Is.GreaterThan(0));
        }

        [Test]
        public void ControlFactory_ChangedAfterGridWasBuilt_RebuildsGridContentWithNewFactory()
        {
            // Arrange
            var dataGrid = CreateTemplatedDataGrid();
            dataGrid.ItemsSource = new List<int> { 1, 2, 3 };
            var factory = new CountingControlFactory();

            // Act
            dataGrid.ControlFactory = factory;

            // Assert
            Assert.That(factory.CreateDisplayControlCallCount, Is.GreaterThan(0));
        }

        private static PropertyTools.Wpf.DataGrid CreateTemplatedDataGrid()
        {
            var template = (ControlTemplate)XamlReader.Parse(TemplateXaml);
            var dataGrid = new PropertyTools.Wpf.DataGrid { Template = template };
            dataGrid.ApplyTemplate();
            return dataGrid;
        }

        private class CountingCellDefinitionFactory : ICellDefinitionFactory
        {
            private readonly CellDefinitionFactory inner = new CellDefinitionFactory();

            public int CreateCellDefinitionCallCount { get; private set; }

            public CellDefinition CreateCellDefinition(CellDescriptor d)
            {
                this.CreateCellDefinitionCallCount++;
                return this.inner.CreateCellDefinition(d);
            }
        }

        private class CountingControlFactory : IDataGridControlFactory
        {
            private readonly DataGridControlFactory inner = new DataGridControlFactory();

            public int CreateDisplayControlCallCount { get; private set; }

            public FrameworkElement CreateDisplayControl(CellDefinition cellDefinition)
            {
                this.CreateDisplayControlCallCount++;
                return this.inner.CreateDisplayControl(cellDefinition);
            }

            public FrameworkElement CreateEditControl(CellDefinition cellDefinition)
            {
                return this.inner.CreateEditControl(cellDefinition);
            }
        }
    }
}
