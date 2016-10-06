// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BackgroundBindingSourceExample.xaml.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Interaction logic for BackgroundBindingSourceExample.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DataGridDemo
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Windows.Media;

    using PropertyTools.Wpf;

    /// <summary>
    /// Interaction logic for BackgroundBindingSourceExample.
    /// </summary>
    public partial class BackgroundBindingSourceExample
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BackgroundBindingSourceExample" /> class.
        /// </summary>
        public BackgroundBindingSourceExample()
        {
            this.InitializeComponent();
            this.ItemsSource = new[] { 11d, 0, 0, 0, 22, 0, 0, 0, 33 };
            this.BackgroundSource = new[]
                                  {
                                      Brushes.LightBlue, Brushes.LightGray, Brushes.LightGray, Brushes.LightGray,
                                      Brushes.LightBlue, Brushes.LightGray, Brushes.LightGray, Brushes.LightGray,
                                      Brushes.LightBlue,
                                  };
            this.CellDefinitionFactory = new CustomCellDefinitionFactory(this.BackgroundSource);
            this.DataContext = this;
        }

        public CustomCellDefinitionFactory CellDefinitionFactory { get; set; }

        /// <summary>
        /// Gets or sets the items.
        /// </summary>
        public double[] ItemsSource { get; }

        public Brush[] BackgroundSource { get; }

        public class CustomCellDefinitionFactory : CellDefinitionFactory
        {
            private readonly IList backgroundSource;

            public CustomCellDefinitionFactory(IList backgroundSource)
            {
                this.backgroundSource = backgroundSource;
            }

            public override CellDefinition CreateCellDefinition(DataGrid owner, CellRef cell, object item)
            {
                return base.CreateCellDefinition(owner, cell, item);
            }

            protected override void ApplyProperties(CellDefinition cd, DataGrid owner, CellRef cell, PropertyDefinition pd, object item)
            {
                base.ApplyProperties(cd, owner, cell, pd, item);
                cd.BackgroundBindingSource = this.backgroundSource;
                cd.BackgroundBindingPath = owner.Operator.GetBindingPath(owner, cell);
            }
        }
    }
}