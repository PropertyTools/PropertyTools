// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ShellViewModel.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Represents a viewmodel for the shell.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace UndoRedoDemo
{
    using System;
    using System.Diagnostics;
    using System.Text;

    using Caliburn.Micro;

    /// <summary>
    /// Represents a view model for the shell.
    /// </summary>
    public class ShellViewModel : Screen
    {
        private bool isModified;
        private string output;

        /// <summary>
        /// The output string builder.
        /// </summary>
        private readonly StringBuilder outputBuilder = new StringBuilder();

        /// <summary>
        /// Initializes static members of the <see cref="ShellViewModel" /> class.
        /// </summary>
        static ShellViewModel()
        {
            UndoRedoService = new UndoRedoService();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref = "ShellViewModel" /> class.
        /// </summary>
        public ShellViewModel()
        {
            this.Measurements = new UndoableCollection<Measurement>();
            this.Title = "Undo/redo demo";
            var tl = new DelegateTraceListener();
            tl.OnAppend += this.AppendTraceMessage;
        }

        /// <summary>
        /// Gets or sets the undo redo service.
        /// </summary>
        /// <value>The undo redo service.</value>
        public static UndoRedoService UndoRedoService { get; set; }

        /// <summary>
        /// Gets the actual title.
        /// </summary>
        /// <value>The actual title.</value>
        public string ActualTitle
        {
            get
            {
                return this.Title + (this.IsModified ? "*" : string.Empty);
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance can add item.
        /// </summary>
        public bool CanAddItem
        {
            get
            {
                return this.Measurements.Count < 10 && this.SelectedIndex != -2;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the selected item can be deleted.
        /// </summary>
        public bool CanDeleteItem
        {
            get
            {
                return this.SelectedIndex >= 0 && this.SelectedIndex < this.Measurements.Count;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the item can be modified.
        /// </summary>
        public bool CanModifyItem
        {
            get
            {
                return this.SelectedIndex >= 0 && this.SelectedIndex < this.Measurements.Count;
            }
        }

        /// <summary>
        /// Gets a value indicating whether redo is possible.
        /// </summary>
        public bool CanRedo
        {
            get
            {
                // UndoRedoService.CanRedo is called!
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Gets a value indicating whether undo is possible.
        /// </summary>
        public bool CanUndo
        {
            get
            {
                // UndoRedoService.CanUndo is called!
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is modified.
        /// </summary>
        public bool IsModified
        {
            get
            {
                return this.isModified;
            }

            set
            {
                this.isModified = value;
                this.NotifyOfPropertyChange(() => this.IsModified);
                this.NotifyOfPropertyChange(() => this.ActualTitle);
            }
        }

        /// <summary>
        /// Gets or sets the measurements.
        /// </summary>
        /// <value>The measurements.</value>
        public UndoableCollection<Measurement> Measurements { get; set; }

        /// <summary>
        /// Gets or sets the output.
        /// </summary>
        /// <value>The output.</value>
        public string Output
        {
            get
            {
                return this.output;
            }

            set
            {
                this.output = value;
                this.NotifyOfPropertyChange(() => this.Output);
            }
        }

        /// <summary>
        /// Gets or sets the index of the selected.
        /// </summary>
        /// <value>The index of the selected.</value>
        public int SelectedIndex { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        public string Title { get; set; }

        /// <summary>
        /// Adds an item.
        /// </summary>
        public void AddItem()
        {
            this.Measurements.Add(new Measurement());
            this.IsModified = true;
            Trace.WriteLine("Added item.");
            this.SelectedIndex = this.Measurements.Count - 1;
            this.NotifyOfPropertyChange(() => this.CanDeleteItem);
            this.NotifyOfPropertyChange(() => this.CanModifyItem);
        }

        /// <summary>
        /// Deletes the selected item.
        /// </summary>
        public void DeleteItem()
        {
            int index = this.SelectedIndex;
            this.Measurements.RemoveAt(this.SelectedIndex);
            Trace.WriteLine("Deleted item.");

            if (index >= this.Measurements.Count)
            {
                index = this.Measurements.Count - 1;
            }

            if (index >= 0)
            {
                this.SelectedIndex = index;
            }

            this.NotifyOfPropertyChange(() => this.CanDeleteItem);
            this.NotifyOfPropertyChange(() => this.CanModifyItem);
        }

        /// <summary>
        /// Exits the application.
        /// </summary>
        public void Exit()
        {
            this.TryClose();
        }

        /// <summary>
        /// Modifies the item.
        /// </summary>
        public void ModifyItem()
        {
            var item = this.Measurements[this.SelectedIndex];

            // item.BeginEdit();
            item.Value = item.Value + 1;
            item.Comments = "The value is now " + item.Value;

            // item.EndEdit();
        }

        /// <summary>
        /// Redo the last undo operation.
        /// </summary>
        public void Redo()
        {
            // UndoRedoService.Redo is called!
            throw new NotImplementedException();
        }

        /// <summary>
        /// Undo the last change.
        /// </summary>
        public void Undo()
        {
            // UndoRedoService.Undo is called!
            throw new NotImplementedException();
        }

        /// <summary>
        /// Appends the trace message.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="newLine">Add a new line if set to <c>true</c>.</param>
        private void AppendTraceMessage(string text, bool newLine)
        {
            this.outputBuilder.Append(text);
            if (newLine)
            {
                this.outputBuilder.AppendLine();
            }

            this.Output = this.outputBuilder.ToString();
        }

    }
}