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
    using System.Windows.Controls.Primitives;

    public class SelectorWrapper : ISelectorDefinition
    {
        private readonly Selector _selector;

        public SelectorWrapper(System.Windows.Controls.Primitives.Selector selector)
        {
            _selector = selector;
        }

        /// <inheritdoc/>
        public IEnumerable ItemsSource
        {
            get => _selector.ItemsSource;
            set => _selector.ItemsSource = value;
        }

        /// <inheritdoc/>
        public string SelectedValuePath
        {
            get => _selector.SelectedValuePath;
            set => _selector.SelectedValuePath = value;
        }

        /// <inheritdoc/>
        public string DisplayMemberPath
        {
            get => _selector.DisplayMemberPath;
            set => _selector.DisplayMemberPath = value;
        }

        /// <inheritdoc/>
        public bool DisplayTextForNullItem
        {
            get => throw new System.NotImplementedException();
            set => throw new System.NotImplementedException();
        }
    }


}