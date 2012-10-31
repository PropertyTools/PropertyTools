// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="PropertyTools">
//   The MIT License (MIT)
//
//   Copyright (c) 2012 Oystein Bjorke
//
//   Permission is hereby granted, free of charge, to any person obtaining a
//   copy of this software and associated documentation files (the
//   "Software"), to deal in the Software without restriction, including
//   without limitation the rights to use, copy, modify, merge, publish,
//   distribute, sublicense, and/or sell copies of the Software, and to
//   permit persons to whom the Software is furnished to do so, subject to
//   the following conditions:
//
//   The above copyright notice and this permission notice shall be included
//   in all copies or substantial portions of the Software.
//
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS
//   OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
//   MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
//   IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
//   CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
//   TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
//   SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// <summary>
//   Interaction logic for MainWindow.xaml
// </summary>
// --------------------------------------------------------------------------------------------------------------------
using System.ComponentModel;
using System.Windows;
using PropertyTools.Wpf;

namespace ExcelDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public MainWindow()
        {
            InitializeComponent();
            int rows = 256;
            int columns = 256;
            Strings = new string[rows, columns];

            // todo: the templates for editing cells are not finished
            Cells = new Cell[rows, columns];
            for (int i = 0; i < rows; i++) for (int j = 0; j < columns; j++) Cells[i, j] = new Cell { Alignment = HorizontalAlignment.Center, Value = "ABC", IsBold = true, IsItalic = true };

            CurrentCellRef = new CellRef();
            DataContext = this;
        }

        public Cell[,] Cells { get; set; }
        public string[,] Strings { get; set; }
        private CellRef currentCellRef;

        public CellRef CurrentCellRef
        {
            get { return currentCellRef; }
            set
            {
                currentCellRef = value;
                SelectionCellRef = value;
                RaisePropertyChanged("CurrentCellRef");
                RaisePropertyChanged("CurrentCell");
                RaisePropertyChanged("CurrentValue");
            }
        }

        private CellRef selectionCellRef;

        public CellRef SelectionCellRef
        {
            get { return selectionCellRef; }
            set
            {
                selectionCellRef = value;
                RaisePropertyChanged("SelectionCellRef");
            }
        }

        public string CurrentValue
        {
            get
            {
                if (CurrentCellRef.Row < Strings.GetUpperBound(0) && CurrentCellRef.Column < Strings.GetUpperBound(1)) return Strings[CurrentCellRef.Row, CurrentCellRef.Column];
                return null;
            }
            set { Strings[CurrentCellRef.Row, CurrentCellRef.Column] = value; }
        }

        public Cell CurrentCell
        {
            get
            {
                if (CurrentCellRef.Row < Cells.GetUpperBound(0) && CurrentCellRef.Column < Cells.GetUpperBound(1)) return Cells[CurrentCellRef.Row, CurrentCellRef.Column];
                return null;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string property)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(property));
            }
        }

    }

    public class Cell : INotifyPropertyChanged
    {
        public override string ToString()
        {
            return Value;
        }

        private string value;
        public string Value
        {
            get { return value; }
            set { this.value = value; RaisePropertyChanged("Value"); }
        }

        private HorizontalAlignment alignment;
        public HorizontalAlignment Alignment
        {
            get { return alignment; }
            set { alignment = value; RaisePropertyChanged("Alignment"); }
        }

        private bool isBold;
        public bool IsBold
        {
            get { return isBold; }
            set { isBold = value; RaisePropertyChanged("IsBold"); RaisePropertyChanged("FontWeight"); }
        }

        private bool isItalic;
        public bool IsItalic
        {
            get { return isItalic; }
            set { isItalic = value; RaisePropertyChanged("IsItalic"); RaisePropertyChanged("FontStyle"); }
        }

        public FontWeight FontWeight { get { return IsBold ? FontWeights.Bold : FontWeights.Normal; } }
        public FontStyle FontStyle { get { return IsItalic ? FontStyles.Italic : FontStyles.Normal; } }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string property)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(property));
            }
        }

    }
}