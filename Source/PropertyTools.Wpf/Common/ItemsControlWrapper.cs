// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ItemsControlWrapper.cs" company="PropertyTools">
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
    using System.Windows.Data;

    public abstract class ItemsControlWrapper : ISelectorDefinition
    {
        private readonly ItemsControl _itemsControl;
        private readonly object _itemsControlBindingSource;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="itemsControl"></param>
        /// <param name="bindingSource">The binding source.</param>
        public ItemsControlWrapper(ItemsControl itemsControl, object bindingSource)
        {
            _itemsControl = itemsControl;
            _itemsControlBindingSource = bindingSource;
        }

        private string _itemsSourcePropertyName;
        /// <inheritdoc/>
        public string ItemsSourcePropertyName
        {
            get
            {
                var bindingExpression = _itemsControl.GetBindingExpression(ItemsControl.ItemsSourceProperty);
                return bindingExpression?.ParentBinding.Path.Path;
            }
            set
            {
                _itemsSourcePropertyName = value;

                if (_itemsSourcePropertyName != null)
                {
                    _itemsControl.DataContext = _itemsControlBindingSource; // 

                    var itemsSourceBinding = new Binding(_itemsSourcePropertyName);
                    _itemsControl.SetBinding(ItemsControl.ItemsSourceProperty, itemsSourceBinding);
                }
            }
        }

        /// <inheritdoc/>
        public IEnumerable ItemsSource
        {
            get => _itemsControl.ItemsSource;
            set => _itemsControl.ItemsSource = value;
        }

        /// <inheritdoc/>
        public abstract string SelectedValuePath { get; set; }

        /// <inheritdoc/>
        public string DisplayMemberPath
        {
            get => _itemsControl.DisplayMemberPath;
            set => _itemsControl.DisplayMemberPath = value;
        }

        /// <inheritdoc/>
        public bool DisplayTextForNullItem { get; set; }
    }
}