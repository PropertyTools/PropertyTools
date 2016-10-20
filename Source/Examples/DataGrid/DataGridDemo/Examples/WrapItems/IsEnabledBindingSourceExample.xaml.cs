// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IsEnabledBindingSourceExample.xaml.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Interaction logic for IsEnabledBindingSourceExample.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DataGridDemo
{
    using System.Collections;

    using PropertyTools.Wpf;

    /// <summary>
    /// Interaction logic for IsEnabledBindingSourceExample.
    /// </summary>
    public partial class IsEnabledBindingSourceExample
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IsEnabledBindingSourceExample" /> class.
        /// </summary>
        public IsEnabledBindingSourceExample()
        {
            this.InitializeComponent();
            this.ItemsSource = new[] { 11d, 0, 0, 0, 22, 0, 0, 0, 33 };
            this.IsItemEnabled = new string[] { "yes", null, null, null, "yes", null, null, null, "yes" };
            this.CellDefinitionFactory = new CustomCellDefinitionFactory(this.IsItemEnabled);
            this.DataContext = this;
        }

        public ICellDefinitionFactory CellDefinitionFactory { get; }

        public class CustomCellDefinitionFactory : CellDefinitionFactory
        {
            private readonly IList isItemEnabledSource;

            public CustomCellDefinitionFactory(IList isItemEnabledSource)
            {
                this.isItemEnabledSource = isItemEnabledSource;
            }

            protected override void ApplyProperties(CellDefinition cd, CellDescriptor d)
            {
                base.ApplyProperties(cd, d);
                cd.IsEnabledBindingSource = this.isItemEnabledSource;
                cd.IsEnabledBindingParameter = "yes";
                cd.IsEnabledBindingPath = d.BindingPath;
            }
        }

        /// <summary>
        /// Gets or sets the items.
        /// </summary>
        public double[] ItemsSource { get; }


        public string[] IsItemEnabled { get; }
    }
}