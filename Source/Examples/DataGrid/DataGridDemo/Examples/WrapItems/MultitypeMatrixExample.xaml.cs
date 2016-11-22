// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MultitypeMatrixExample.xaml.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Interaction logic for MultitypeMatrixExample.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DataGridDemo
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Globalization;

    /// <summary>
    /// Interaction logic for MultitypeMatrixExample.
    /// </summary>
    public partial class MultitypeMatrixExample
    {
        private static readonly MultitypeMatrix StaticMatrix = new MultitypeMatrix(3, 3)
        {
            [0, 0] = true,
            [0, 1] = false,
            [0, 2] = null,
            [1, 0] = "Hello",
            [1, 1] = string.Empty,
            [1, 2] = null,
            [2, 0] = 1,
            [2, 1] = Math.PI,
            [2, 2] = Fruit.Apple
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="MultitypeMatrixExample" /> class.
        /// </summary>
        public MultitypeMatrixExample()
        {
            this.InitializeComponent();
            this.DataContext = this;
        }

        /// <summary>
        /// Gets or sets the items.
        /// </summary>
        public MultitypeMatrix Matrix => StaticMatrix;

        public class MultitypeMatrix : IList, IList<object>, INotifyCollectionChanged
        {
            private readonly int m;

            private readonly int n;

            private readonly object[] values;

            public MultitypeMatrix(int m, int n)
            {
                this.m = m;
                this.n = n;
                this.values = new object[m * n];
            }

            public int M => this.m;

            public int N => this.n;

            public int Count => this.values.Length;

            public object SyncRoot => this.values;

            public bool IsSynchronized => false;

            public bool IsReadOnly => false;

            public bool IsFixedSize => true;

            public object this[int index]
            {
                get
                {
                    return this.values[index];
                }
                set
                {
                    if (this.values[index] != null)
                    {
                        if (value == null)
                        {
                            throw new InvalidOperationException("Cannot clear element " + index);
                        }

                        value = Convert.ChangeType(value, this.values[index].GetType(), CultureInfo.InvariantCulture);
                        if (this.values[index].GetType() != value.GetType())
                        {
                            throw new InvalidOperationException("Cannot change type of element " + index);
                        }
                    }

                    var oldValue = this.values[index];
                    this.values[index] = value;
                    this.CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, value, oldValue, index));
                }
            }

            public object this[int i, int j]
            {
                get
                {
                    return this.values[this.GetIndex(i, j)];
                }
                set
                {
                    this.values[this.GetIndex(i, j)] = value;
                }
            }

            int IList.Add(object value)
            {
                throw new NotImplementedException();
            }

            void ICollection<object>.Clear()
            {
                throw new NotImplementedException();
            }

            bool ICollection<object>.Contains(object item)
            {
                throw new NotImplementedException();
            }

            void ICollection<object>.CopyTo(object[] array, int arrayIndex)
            {
                throw new NotImplementedException();
            }

            bool ICollection<object>.Remove(object item)
            {
                throw new NotImplementedException();
            }

            bool IList.Contains(object value)
            {
                throw new NotImplementedException();
            }

            void ICollection<object>.Add(object item)
            {
                throw new NotImplementedException();
            }

            void IList.Clear()
            {
                throw new NotImplementedException();
            }

            int IList.IndexOf(object value)
            {
                for (int i = 0; i < this.values.Length; i++)
                {
                    if (object.ReferenceEquals(this.values[i], value))
                    {
                        return i;
                    }
                }

                return -1;
            }

            void IList<object>.Insert(int index, object item)
            {
                throw new NotImplementedException();
            }

            void IList<object>.RemoveAt(int index)
            {
                throw new NotImplementedException();
            }

            int IList<object>.IndexOf(object item)
            {
                throw new NotImplementedException();
            }

            void IList.Insert(int index, object value)
            {
                throw new NotImplementedException();
            }

            void IList.Remove(object value)
            {
                throw new NotImplementedException();
            }

            void IList.RemoveAt(int index)
            {
                throw new NotImplementedException();
            }

            IEnumerator<object> IEnumerable<object>.GetEnumerator()
            {
                throw new NotImplementedException();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.values.GetEnumerator();
            }

            void ICollection.CopyTo(Array array, int index)
            {
                this.values.CopyTo(array, index);
            }

            private int GetIndex(int i, int j)
            {
                return i * this.n + j;
            }

            public event NotifyCollectionChangedEventHandler CollectionChanged;
        }
    }
}