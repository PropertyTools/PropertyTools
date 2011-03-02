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

        #region PropertyChanged Block

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string property)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(property));
            }
        }

        #endregion
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

        #region PropertyChanged Block

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string property)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(property));
            }
        }

        #endregion
    }
}