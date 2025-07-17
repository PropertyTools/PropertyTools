// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ListOfListWithBulkChangesExample.xaml.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Interaction logic for ListOfListWithBulkChangesExample.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DataGridDemo
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Linq;

    public class ObservableList<T> : ICollection<T>, IList<T>, IEnumerable<T>, IReadOnlyList<T>, IList, IEnumerable, INotifyCollectionChanged, INotifyPropertyChanged, IEditableObject
    {
        private readonly List<T> list;
        private bool isEditing;

        public ObservableList(List<T> list)
        {
            this.list = list;
        }

        public T this[int index] { get => this.list[index]; set => this.list[index] = value; }
        object IList.this[int index] { get => this.list[index]; set => this.list[index] = (T)value; }

        public int Count => this.list.Count;

        public bool IsReadOnly => false;

        public bool IsFixedSize => false;

        public bool IsSynchronized => true;

        public object SyncRoot => this;

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public event PropertyChangedEventHandler PropertyChanged;

        public void Add(T item)
        {
            this.itemsToAdd.Add(item);
            if (!this.isEditing) { this.EndEdit(); }
        }

        public void AddRange(IEnumerable<T> items)
        {
            this.BeginEdit();
            this.itemsToAdd.AddRange(items);
            this.EndEdit();
        }

        public int Add(object value) { this.Add((T)value); return this.list.Count - 1; }
        public void Clear() => this.list.Clear();
        public bool Contains(T item) => throw new NotImplementedException();
        public bool Contains(object value) => throw new NotImplementedException();
        public void CopyTo(T[] array, int arrayIndex) => this.list.CopyTo(array, arrayIndex);
        public void CopyTo(Array array, int index) => this.list.CopyTo(array.Cast<T>().ToArray(), index);
        public IEnumerator<T> GetEnumerator() => this.list.GetEnumerator();
        public int IndexOf(T item) => throw new NotImplementedException();
        public int IndexOf(object value) => throw new NotImplementedException();
        public void Insert(int index, T item) => this.list.Insert(index, item);
        public void Insert(int index, object value) => this.Insert(index, (T)value);
        public bool Remove(T item) => throw new NotImplementedException();
        public void Remove(object value) => throw new NotImplementedException();
        public void RemoveAt(int index) => throw new NotImplementedException();
        IEnumerator IEnumerable.GetEnumerator() => this.list.GetEnumerator();

        private List<T> itemsToAdd = new List<T>();

        public void BeginEdit()
        {
            this.isEditing = true;
        }

        public void CancelEdit()
        {
            this.itemsToAdd.Clear();
            this.isEditing = false;
        }

        public void EndEdit()
        {
            this.list.AddRange(itemsToAdd);
            // The DataGrid uses CollectionView which does not support Add with multiple items so we cannot use the following line
            // this.CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, itemsToAdd));

            // but it handles any collectionchange event the same way, so we can simply send a Reset...
            this.CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));

            this.OnPropertyChanged(new PropertyChangedEventArgs(nameof(Count)));

            this.itemsToAdd.Clear();
            this.isEditing = false;
        }

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, e);
        }
    }
}