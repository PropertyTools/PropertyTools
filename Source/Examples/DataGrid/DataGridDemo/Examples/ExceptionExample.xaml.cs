// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExceptionExample.xaml.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Interaction logic for ExceptionExample.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DataGridDemo
{
    using System;
    using System.Collections.ObjectModel;

    using PropertyTools;

    /// <summary>
    /// Interaction logic for ExceptionExample.
    /// </summary>
    public partial class ExceptionExample
    {
        /// <summary>
        /// The static items source.
        /// </summary>
        /// <remarks>The static field makes it possible to use the same items source in multiple windows. 
        /// This is great for testing change notifications!</remarks>
        private static readonly ObservableCollection<ExampleObject> StaticItems =
            new ObservableCollection<ExampleObject> { new ExampleObject() };

        /// <summary>
        /// Initializes a new instance of the <see cref="NullableExample" /> class.
        /// </summary>
        public ExceptionExample()
        {
            this.InitializeComponent();
            this.DataContext = this;
        }

        /// <summary>
        /// Gets the items source.
        /// </summary>
        public ObservableCollection<ExampleObject> Items => StaticItems;

        public class ExampleObject : Observable
        {
            private string notNull;
            private readonly string notEditable;
            private string notEmpty;

            public ExampleObject()
            {
                this.notNull = "do not press delete";
                this.notEmpty = "do not set to empty";
                this.notEditable = "do not edit";
            }

            public string NotNull
            {
                get { return this.notNull; }
                set
                {
                    if (value == null) throw new InvalidOperationException();
                    this.SetValue(ref this.notNull, value);
                }
            }

            public string NotEmpty
            {
                get { return this.notEmpty; }
                set
                {
                    if (string.IsNullOrEmpty(value)) throw new InvalidOperationException();
                    this.SetValue(ref this.notEmpty, value);
                }
            }

            public string NotEditable
            {
                get { return this.notEditable; }
                set
                {
                    throw new InvalidOperationException();
                }
            }
        }
    }
}