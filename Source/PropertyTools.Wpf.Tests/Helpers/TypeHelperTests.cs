// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TypeHelperTests.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Represents a class implementing IList{List{T}} and IList.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf.Tests
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Windows.Media;

    using NUnit.Framework;

    [TestFixture]
    public class TypeHelperTests
    {
        [Test]
        public void FindBiggestCommonType_UniformList_ReturnsCorrectType()
        {
            var brushes = new[] { new SolidColorBrush(), new SolidColorBrush() };
            Assert.AreEqual(typeof(SolidColorBrush), TypeHelper.FindBiggestCommonType(brushes));
        }

        [Test]
        public void FindBiggestCommonType_MixedList_ReturnsBaseType()
        {
            var brushes = new Brush[] { new SolidColorBrush(), new LinearGradientBrush() };
            Assert.AreEqual(typeof(Brush), TypeHelper.FindBiggestCommonType(brushes));
        }

        [Test]
        public void FindBiggestCommonType_ListOfObjects_ReturnsBaseType()
        {
            var brushes = new object[] { new SolidColorBrush(), new LinearGradientBrush() };
            Assert.AreEqual(typeof(Brush), TypeHelper.FindBiggestCommonType(brushes));
        }

        private enum TestEnum
        {
            Test1,

            Test2,

            Test3
        }

        [Test]
        public void GetEnumType_Enum_ReturnsInt()
        {
            Assert.AreEqual(typeof(TestEnum), TypeHelper.GetEnumType(typeof(TestEnum)));
        }

        [Test]
        public void GetEnumType_NullableEnum_ReturnsInt()
        {
            Assert.AreEqual(typeof(TestEnum), TypeHelper.GetEnumType(typeof(TestEnum?)));
        }

        [Test]
        public void GetItemType_ListInt_ReturnsInt()
        {
            Assert.AreEqual(typeof(int), TypeHelper.GetItemType(new List<int>()));
        }

        [Test]
        public void GetItemType_ArrayOfNullableDouble_ReturnsNullableDouble()
        {
            Assert.AreEqual(typeof(double?), TypeHelper.GetItemType(new double?[5]));
        }

        [Test]
        public void GetItemType_Dictionary_ReturnsKeyValuePair()
        {
            Assert.AreEqual(typeof(KeyValuePair<int, double>), TypeHelper.GetItemType(new Dictionary<int, double>()));
        }

        [Test]
        public void GetItemType_Null_ReturnsNull()
        {
            Assert.AreEqual(null, TypeHelper.GetItemType(null));
        }

        [Test]
        public void IsIListIList_ObservableCollectionObservableCollection_ReturnTrue()
        {
            Assert.IsTrue(TypeHelper.IsIListIList(typeof(List<List<double>>)));
        }

        [Test]
        public void IsIListIList_SubclassOfIListIList_ReturnTrue()
        {
            Assert.IsTrue(TypeHelper.IsIListIList(typeof(Testclass<double>)));
        }

        [Test]
        public void IsIListIList_DataTable_ReturnFalse()
        {
            var dt = new DataTable();
            Assert.IsFalse(TypeHelper.IsIListIList(dt.DefaultView.GetType()));
        }

        /// <summary>
        /// Represents a class implementing IList{List{T}} and IList. 
        /// </summary>
        /// <typeparam name="T">
        /// The type of the element.
        /// </typeparam>
        public class Testclass<T> : IList<IList<T>>, IList
        {
            /// <summary>
            /// Returns an enumerator that iterates through the collection.
            /// </summary>
            /// <returns>
            /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
            /// </returns>
            IEnumerator<IList<T>> IEnumerable<IList<T>>.GetEnumerator()
            {
                throw new NotImplementedException();
            }

            /// <summary>
            /// Returns an enumerator that iterates through the collection.
            /// </summary>
            /// <returns>
            /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
            /// </returns>
            public IEnumerator<T> GetEnumerator()
            {
                throw new NotImplementedException();
            }

            /// <summary>
            /// Returns an enumerator that iterates through a collection.
            /// </summary>
            /// <returns>
            /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
            /// </returns>
            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }

            /// <summary>
            /// Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1"/>.
            /// </summary>
            /// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param><exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.</exception>
            public void Add(T item)
            {
                throw new NotImplementedException();
            }

            /// <summary>
            /// Adds an item to the <see cref="T:System.Collections.IList"/>.
            /// </summary>
            /// <returns>
            /// The position into which the new element was inserted, or -1 to indicate that the item was not inserted into the collection,
            /// </returns>
            /// <param name="value">The object to add to the <see cref="T:System.Collections.IList"/>. </param><exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.IList"/> is read-only.-or- The <see cref="T:System.Collections.IList"/> has a fixed size. </exception>
            public int Add(object value)
            {
                throw new NotImplementedException();
            }

            /// <summary>
            /// Determines whether the <see cref="T:System.Collections.IList"/> contains a specific value.
            /// </summary>
            /// <returns>
            /// true if the <see cref="T:System.Object"/> is found in the <see cref="T:System.Collections.IList"/>; otherwise, false.
            /// </returns>
            /// <param name="value">The object to locate in the <see cref="T:System.Collections.IList"/>. </param>
            public bool Contains(object value)
            {
                throw new NotImplementedException();
            }

            /// <summary>
            /// Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1"/>.
            /// </summary>
            /// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param><exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.</exception>
            public void Add(IList<T> item)
            {
                throw new NotImplementedException();
            }

            /// <summary>
            /// Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1"/>.
            /// </summary>
            /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only. </exception>
            public void Clear()
            {
                throw new NotImplementedException();
            }

            /// <summary>
            /// Determines whether the <see cref="T:System.Collections.Generic.ICollection`1"/> contains a specific value.
            /// </summary>
            /// <returns>
            /// true if <paramref name="item"/> is found in the <see cref="T:System.Collections.Generic.ICollection`1"/>; otherwise, false.
            /// </returns>
            /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param>
            public bool Contains(IList<T> item)
            {
                throw new NotImplementedException();
            }

            /// <summary>
            /// Copies the elements of the <see cref="T:System.Collections.Generic.ICollection`1"/> to an <see cref="T:System.Array"/>, starting at a particular <see cref="T:System.Array"/> index.
            /// </summary>
            /// <param name="array">The one-dimensional <see cref="T:System.Array"/> that is the destination of the elements copied from <see cref="T:System.Collections.Generic.ICollection`1"/>. The <see cref="T:System.Array"/> must have zero-based indexing.</param><param name="arrayIndex">The zero-based index in <paramref name="array"/> at which copying begins.</param><exception cref="T:System.ArgumentNullException"><paramref name="array"/> is null.</exception><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="arrayIndex"/> is less than 0.</exception><exception cref="T:System.ArgumentException">The number of elements in the source <see cref="T:System.Collections.Generic.ICollection`1"/> is greater than the available space from <paramref name="arrayIndex"/> to the end of the destination <paramref name="array"/>.</exception>
            public void CopyTo(IList<T>[] array, int arrayIndex)
            {
                throw new NotImplementedException();
            }

            /// <summary>
            /// Removes the first occurrence of a specific object from the <see cref="T:System.Collections.Generic.ICollection`1"/>.
            /// </summary>
            /// <returns>
            /// true if <paramref name="item"/> was successfully removed from the <see cref="T:System.Collections.Generic.ICollection`1"/>; otherwise, false. This method also returns false if <paramref name="item"/> is not found in the original <see cref="T:System.Collections.Generic.ICollection`1"/>.
            /// </returns>
            /// <param name="item">The object to remove from the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param><exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.</exception>
            public bool Remove(IList<T> item)
            {
                throw new NotImplementedException();
            }

            /// <summary>
            /// Gets the number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"/>.
            /// </summary>
            /// <returns>
            /// The number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"/>.
            /// </returns>
            public int Count { get; private set; }

            /// <summary>
            /// Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.
            /// </summary>
            /// <returns>
            /// true if the <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only; otherwise, false.
            /// </returns>
            public bool IsReadOnly { get; private set; }

            /// <summary>
            /// Removes all items from the <see cref="T:System.Collections.IList"/>.
            /// </summary>
            /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.IList"/> is read-only. </exception>
            void IList.Clear()
            {
                throw new NotImplementedException();
            }

            /// <summary>
            /// Determines the index of a specific item in the <see cref="T:System.Collections.IList"/>.
            /// </summary>
            /// <returns>
            /// The index of <paramref name="value"/> if found in the list; otherwise, -1.
            /// </returns>
            /// <param name="value">The object to locate in the <see cref="T:System.Collections.IList"/>. </param>
            public int IndexOf(object value)
            {
                throw new NotImplementedException();
            }

            /// <summary>
            /// Inserts an item to the <see cref="T:System.Collections.IList"/> at the specified index.
            /// </summary>
            /// <param name="index">The zero-based index at which <paramref name="value"/> should be inserted. </param><param name="value">The object to insert into the <see cref="T:System.Collections.IList"/>. </param><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="index"/> is not a valid index in the <see cref="T:System.Collections.IList"/>. </exception><exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.IList"/> is read-only.-or- The <see cref="T:System.Collections.IList"/> has a fixed size. </exception><exception cref="T:System.NullReferenceException"><paramref name="value"/> is null reference in the <see cref="T:System.Collections.IList"/>.</exception>
            public void Insert(int index, object value)
            {
                throw new NotImplementedException();
            }

            /// <summary>
            /// Removes the first occurrence of a specific object from the <see cref="T:System.Collections.IList"/>.
            /// </summary>
            /// <param name="value">The object to remove from the <see cref="T:System.Collections.IList"/>. </param><exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.IList"/> is read-only.-or- The <see cref="T:System.Collections.IList"/> has a fixed size. </exception>
            public void Remove(object value)
            {
                throw new NotImplementedException();
            }

            /// <summary>
            /// Determines the index of a specific item in the <see cref="T:System.Collections.Generic.IList`1"/>.
            /// </summary>
            /// <returns>
            /// The index of <paramref name="item"/> if found in the list; otherwise, -1.
            /// </returns>
            /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.IList`1"/>.</param>
            public int IndexOf(IList<T> item)
            {
                throw new NotImplementedException();
            }

            /// <summary>
            /// Inserts an item to the <see cref="T:System.Collections.Generic.IList`1"/> at the specified index.
            /// </summary>
            /// <param name="index">The zero-based index at which <paramref name="item"/> should be inserted.</param><param name="item">The object to insert into the <see cref="T:System.Collections.Generic.IList`1"/>.</param><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="index"/> is not a valid index in the <see cref="T:System.Collections.Generic.IList`1"/>.</exception><exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.IList`1"/> is read-only.</exception>
            public void Insert(int index, IList<T> item)
            {
                throw new NotImplementedException();
            }

            /// <summary>
            /// Removes the <see cref="T:System.Collections.Generic.IList`1"/> item at the specified index.
            /// </summary>
            /// <param name="index">The zero-based index of the item to remove.</param><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="index"/> is not a valid index in the <see cref="T:System.Collections.Generic.IList`1"/>.</exception><exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.IList`1"/> is read-only.</exception>
            public void RemoveAt(int index)
            {
                throw new NotImplementedException();
            }

            /// <summary>
            /// Gets or sets the element at the specified index.
            /// </summary>
            /// <returns>
            /// The element at the specified index.
            /// </returns>
            /// <param name="index">The zero-based index of the element to get or set.</param><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="index"/> is not a valid index in the <see cref="T:System.Collections.Generic.IList`1"/>.</exception><exception cref="T:System.NotSupportedException">The property is set and the <see cref="T:System.Collections.Generic.IList`1"/> is read-only.</exception>
            IList<T> IList<IList<T>>.this[int index]
            {
                get
                {
                    throw new NotImplementedException();
                }
                set
                {
                    throw new NotImplementedException();
                }
            }

            /// <summary>
            /// Removes the <see cref="T:System.Collections.IList"/> item at the specified index.
            /// </summary>
            /// <param name="index">The zero-based index of the item to remove. </param><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="index"/> is not a valid index in the <see cref="T:System.Collections.IList"/>. </exception><exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.IList"/> is read-only.-or- The <see cref="T:System.Collections.IList"/> has a fixed size. </exception>
            void IList.RemoveAt(int index)
            {
                throw new NotImplementedException();
            }

            /// <summary>
            /// Gets or sets the element at the specified index.
            /// </summary>
            /// <returns>
            /// The element at the specified index.
            /// </returns>
            /// <param name="index">The zero-based index of the element to get or set. </param><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="index"/> is not a valid index in the <see cref="T:System.Collections.IList"/>. </exception><exception cref="T:System.NotSupportedException">The property is set and the <see cref="T:System.Collections.IList"/> is read-only. </exception>
            object IList.this[int index]
            {
                get
                {
                    throw new NotImplementedException();
                }
                set
                {
                    throw new NotImplementedException();
                }
            }

            /// <summary>
            /// Gets a value indicating whether the <see cref="T:System.Collections.IList"/> is read-only.
            /// </summary>
            /// <returns>
            /// true if the <see cref="T:System.Collections.IList"/> is read-only; otherwise, false.
            /// </returns>
            bool IList.IsReadOnly
            {
                get
                {
                    return false;
                }
            }

            /// <summary>
            /// Gets a value indicating whether the <see cref="T:System.Collections.IList"/> has a fixed size.
            /// </summary>
            /// <returns>
            /// true if the <see cref="T:System.Collections.IList"/> has a fixed size; otherwise, false.
            /// </returns>
            public bool IsFixedSize { get; private set; }

            /// <summary>
            /// Determines whether the <see cref="T:System.Collections.Generic.ICollection`1"/> contains a specific value.
            /// </summary>
            /// <returns>
            /// true if <paramref name="item"/> is found in the <see cref="T:System.Collections.Generic.ICollection`1"/>; otherwise, false.
            /// </returns>
            /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param>
            public bool Contains(T item)
            {
                throw new NotImplementedException();
            }

            /// <summary>
            /// Copies the elements of the <see cref="T:System.Collections.Generic.ICollection`1"/> to an <see cref="T:System.Array"/>, starting at a particular <see cref="T:System.Array"/> index.
            /// </summary>
            /// <param name="array">The one-dimensional <see cref="T:System.Array"/> that is the destination of the elements copied from <see cref="T:System.Collections.Generic.ICollection`1"/>. The <see cref="T:System.Array"/> must have zero-based indexing.</param><param name="arrayIndex">The zero-based index in <paramref name="array"/> at which copying begins.</param><exception cref="T:System.ArgumentNullException"><paramref name="array"/> is null.</exception><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="arrayIndex"/> is less than 0.</exception><exception cref="T:System.ArgumentException">The number of elements in the source <see cref="T:System.Collections.Generic.ICollection`1"/> is greater than the available space from <paramref name="arrayIndex"/> to the end of the destination <paramref name="array"/>.</exception>
            public void CopyTo(T[] array, int arrayIndex)
            {
                throw new NotImplementedException();
            }

            /// <summary>
            /// Removes the first occurrence of a specific object from the <see cref="T:System.Collections.Generic.ICollection`1"/>.
            /// </summary>
            /// <returns>
            /// true if <paramref name="item"/> was successfully removed from the <see cref="T:System.Collections.Generic.ICollection`1"/>; otherwise, false. This method also returns false if <paramref name="item"/> is not found in the original <see cref="T:System.Collections.Generic.ICollection`1"/>.
            /// </returns>
            /// <param name="item">The object to remove from the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param><exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.</exception>
            public bool Remove(T item)
            {
                throw new NotImplementedException();
            }

            /// <summary>
            /// Copies the elements of the <see cref="T:System.Collections.ICollection"/> to an <see cref="T:System.Array"/>, starting at a particular <see cref="T:System.Array"/> index.
            /// </summary>
            /// <param name="array">The one-dimensional <see cref="T:System.Array"/> that is the destination of the elements copied from <see cref="T:System.Collections.ICollection"/>. The <see cref="T:System.Array"/> must have zero-based indexing. </param><param name="index">The zero-based index in <paramref name="array"/> at which copying begins. </param><exception cref="T:System.ArgumentNullException"><paramref name="array"/> is null. </exception><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="index"/> is less than zero. </exception><exception cref="T:System.ArgumentException"><paramref name="array"/> is multidimensional.-or- The number of elements in the source <see cref="T:System.Collections.ICollection"/> is greater than the available space from <paramref name="index"/> to the end of the destination <paramref name="array"/>.-or-The type of the source <see cref="T:System.Collections.ICollection"/> cannot be cast automatically to the type of the destination <paramref name="array"/>.</exception>
            public void CopyTo(Array array, int index)
            {
                throw new NotImplementedException();
            }

            /// <summary>
            /// Gets the number of elements contained in the <see cref="T:System.Collections.ICollection"/>.
            /// </summary>
            /// <returns>
            /// The number of elements contained in the <see cref="T:System.Collections.ICollection"/>.
            /// </returns>
            int ICollection.Count
            {
                get
                {
                    return 0;
                }
            }

            /// <summary>
            /// Gets an object that can be used to synchronize access to the <see cref="T:System.Collections.ICollection"/>.
            /// </summary>
            /// <returns>
            /// An object that can be used to synchronize access to the <see cref="T:System.Collections.ICollection"/>.
            /// </returns>
            public object SyncRoot { get; private set; }

            /// <summary>
            /// Gets a value indicating whether access to the <see cref="T:System.Collections.ICollection"/> is synchronized (thread safe).
            /// </summary>
            /// <returns>
            /// true if access to the <see cref="T:System.Collections.ICollection"/> is synchronized (thread safe); otherwise, false.
            /// </returns>
            public bool IsSynchronized { get; private set; }

            /// <summary>
            /// Determines the index of a specific item in the <see cref="T:System.Collections.Generic.IList`1"/>.
            /// </summary>
            /// <returns>
            /// The index of <paramref name="item"/> if found in the list; otherwise, -1.
            /// </returns>
            /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.IList`1"/>.</param>
            public int IndexOf(T item)
            {
                throw new NotImplementedException();
            }

            /// <summary>
            /// Inserts an item to the <see cref="T:System.Collections.Generic.IList`1"/> at the specified index.
            /// </summary>
            /// <param name="index">The zero-based index at which <paramref name="item"/> should be inserted.</param><param name="item">The object to insert into the <see cref="T:System.Collections.Generic.IList`1"/>.</param><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="index"/> is not a valid index in the <see cref="T:System.Collections.Generic.IList`1"/>.</exception><exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.IList`1"/> is read-only.</exception>
            public void Insert(int index, T item)
            {
                throw new NotImplementedException();
            }

            /// <summary>
            /// Gets or sets the element at the specified index.
            /// </summary>
            /// <returns>
            /// The element at the specified index.
            /// </returns>
            /// <param name="index">The zero-based index of the element to get or set.</param><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="index"/> is not a valid index in the <see cref="T:System.Collections.Generic.IList`1"/>.</exception><exception cref="T:System.NotSupportedException">The property is set and the <see cref="T:System.Collections.Generic.IList`1"/> is read-only.</exception>
            public T this[int index]
            {
                get
                {
                    throw new NotImplementedException();
                }
                set
                {
                    throw new NotImplementedException();
                }
            }
        }
    }
}