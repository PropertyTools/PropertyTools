// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IListCommitOnTabExample.xaml.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Interaction logic for IListCommitOnTabExample.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DataGridDemo
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.Specialized;

    using PropertyTools.Wpf;

    /// <summary>
    /// Reproduces issue #81 with an <see cref="IList{T}"/> source raising replace notifications from indexer updates.
    /// </summary>
    public partial class IListCommitOnTabExample
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IListCommitOnTabExample" /> class.
        /// </summary>
        public IListCommitOnTabExample()
        {
            this.InitializeComponent();

            this.ItemsSource = new ReplaceNotifyingList<Mass>
            {
                0 * Mass.Kilogram,
                1 * Mass.Kilogram,
                2 * Mass.Kilogram
            };

            this.CellDefinitionFactory.RegisterValueConverter(typeof(Mass), new MassValueConverter());
            this.DataContext = this;
        }

        /// <summary>
        /// Gets the items source.
        /// </summary>
        public IList<Mass> ItemsSource { get; }

        /// <summary>
        /// Gets the cell definition factory.
        /// </summary>
        public CellDefinitionFactory CellDefinitionFactory { get; } = new CellDefinitionFactory();

        private sealed class ReplaceNotifyingList<T> : IList<T>, IList, INotifyCollectionChanged
        {
            private readonly List<T> innerList = new List<T>();

            public event NotifyCollectionChangedEventHandler CollectionChanged;

            public T this[int index]
            {
                get => this.innerList[index];
                set
                {
                    var oldItem = this.innerList[index];
                    this.innerList[index] = value;
                    this.CollectionChanged?.Invoke(
                        this,
                        new NotifyCollectionChangedEventArgs(
                            NotifyCollectionChangedAction.Replace,
                            value,
                            oldItem,
                            index));
                }
            }

            object IList.this[int index]
            {
                get => this[index];
                set => this[index] = (T)value;
            }

            public int Count => this.innerList.Count;

            public bool IsReadOnly => false;

            public bool IsFixedSize => false;

            public bool IsSynchronized => false;

            public object SyncRoot => this;

            public void Add(T item)
            {
                this.innerList.Add(item);
                this.CollectionChanged?.Invoke(
                    this,
                    new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, this.innerList.Count - 1));
            }

            public int Add(object value)
            {
                this.Add((T)value);
                return this.innerList.Count - 1;
            }

            public void Clear()
            {
                if (this.innerList.Count == 0)
                {
                    return;
                }

                this.innerList.Clear();
                this.CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }

            public bool Contains(T item)
            {
                return this.innerList.Contains(item);
            }

            public bool Contains(object value)
            {
                return value is T item && this.Contains(item);
            }

            public void CopyTo(T[] array, int arrayIndex)
            {
                this.innerList.CopyTo(array, arrayIndex);
            }

            public void CopyTo(Array array, int index)
            {
                ((ICollection)this.innerList).CopyTo(array, index);
            }

            public IEnumerator<T> GetEnumerator()
            {
                return this.innerList.GetEnumerator();
            }

            public int IndexOf(T item)
            {
                return this.innerList.IndexOf(item);
            }

            public int IndexOf(object value)
            {
                return value is T item ? this.IndexOf(item) : -1;
            }

            public void Insert(int index, T item)
            {
                this.innerList.Insert(index, item);
                this.CollectionChanged?.Invoke(
                    this,
                    new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, index));
            }

            public void Insert(int index, object value)
            {
                this.Insert(index, (T)value);
            }

            public bool Remove(T item)
            {
                var index = this.IndexOf(item);
                if (index < 0)
                {
                    return false;
                }

                this.RemoveAt(index);
                return true;
            }

            public void Remove(object value)
            {
                if (value is T item)
                {
                    this.Remove(item);
                }
            }

            public void RemoveAt(int index)
            {
                var oldItem = this.innerList[index];
                this.innerList.RemoveAt(index);
                this.CollectionChanged?.Invoke(
                    this,
                    new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, oldItem, index));
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }
        }
    }
}
