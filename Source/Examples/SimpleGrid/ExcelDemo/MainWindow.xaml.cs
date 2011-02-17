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
            Data = new string[20, 20];
            // for (int i = 0; i < 20; i++) for (int j = 0; j < 20; j++) Data[i, j] = "";

            CurrentCell = new CellRef();
            DataContext = this;
        }

        public string[,] Data { get; set; }
        private CellRef currentCell;

        public CellRef CurrentCell
        {
            get { return currentCell; }
            set
            {
                currentCell = value;
                RaisePropertyChanged("CurrentCell");
                RaisePropertyChanged("CurrentValue");
            }
        }

        public string CurrentValue
        {
            get
            {
                if (CurrentCell.Row < Data.GetUpperBound(0) && CurrentCell.Column < Data.GetUpperBound(1)) return Data[CurrentCell.Row, CurrentCell.Column];
                return null;
            }
            set { Data[CurrentCell.Row, CurrentCell.Column] = value; }
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
}