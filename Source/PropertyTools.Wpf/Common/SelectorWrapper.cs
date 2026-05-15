// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SelectorWrapper.cs" company="PropertyTools">
//   Copyright (c) 2025 PropertyTools contributors
// </copyright>
// <summary>
//   Defines an ISelectorDefinition wrapper for System.Windows.Controls.Primitives.Selector control.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf.Common
{
    using System.Collections;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Data;

    public class SelectorWrapper : ItemsControlWrapper, ISelectorDefinition
    {
        private readonly Selector _selector;

        /// <summary>
        /// Initializes a new instance of the <see cref = "SelectorWrapper" /> class.
        /// </summary>
        /// <param name="selector"></param>
        /// <param name="bindingSource">The binding source.</param>
        public SelectorWrapper(System.Windows.Controls.Primitives.Selector selector, object bindingSource)
           : base(selector, bindingSource)
        {
            _selector = selector;
        }

        /// <inheritdoc/>
        public override string SelectedValuePath
        {
            get => _selector.SelectedValuePath;
            set => _selector.SelectedValuePath = value;
        }
    }
}