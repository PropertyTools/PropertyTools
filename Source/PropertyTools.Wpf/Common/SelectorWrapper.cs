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

    public class SelectorWrapper : ISelectorDefinition
    {
        private readonly Selector _selector;
        private readonly object _selectorBindingSource;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="selector"></param>
        /// <param name="bindingSource">The binding source.</param>
        public SelectorWrapper(System.Windows.Controls.Primitives.Selector selector, object bindingSource)
        {
            _selector = selector;
            _selectorBindingSource = bindingSource;
        }

        private string _itemsSourceProperty;
        /// <inheritdoc/>
        public string ItemsSourceProperty
        {
            get
            {
                var bindingExpression = _selector.GetBindingExpression(ItemsControl.ItemsSourceProperty);
                return bindingExpression?.ParentBinding.Path.Path;
            }
            set 
            {
                _itemsSourceProperty = value;

                if (_itemsSourceProperty != null)
                {
                    _selector.DataContext = _selectorBindingSource; // 

                    var itemsSourceBinding = new Binding(_itemsSourceProperty);
                    _selector.SetBinding(ItemsControl.ItemsSourceProperty, itemsSourceBinding);
                }
            }
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