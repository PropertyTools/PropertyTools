using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace PropertyEditorLibrary
{
    /// <summary>
    /// About Dialog
    /// </summary>
    public partial class AboutDialog : Window
    {
        private Assembly _assembly;

        public AboutDialog()
        {
            InitializeComponent();
            Assembly = Assembly.GetCallingAssembly();
        }

        public ImageSource Image
        {
            set { Image1.Source = value; }
        }

        public Assembly Assembly
        {
            get { return _assembly; }
            set
            {
                _assembly = value;
                AssemblyChanged();
            }
        }

        private void AssemblyChanged()
        {
            UpdateContent(Assembly);
        }

        private int _row;

        private void UpdateContent(Assembly a)
        {
            if (a == null) return;
            if (a.Location==null) return;

            var fvi = FileVersionInfo.GetVersionInfo(a.Location);
            var fi = new FileInfo(fvi.FileName);

            Title = String.Format("{0} {1}.{2}", fvi.ProductName, fvi.ProductMajorPart, fvi.ProductMinorPart);
            Add("Product", fvi.ProductName);
            Add("Description", fvi.Comments);
            Add("Copyright", fvi.LegalCopyright);
            Add("Trademarks", fvi.LegalTrademarks);
            Add("Company", fvi.CompanyName);
            AddSeparator();

            // Assembly version
            var va = (AssemblyVersionAttribute[])a.GetCustomAttributes(typeof(AssemblyVersionAttribute), false);
            if (va != null && va.Length > 0)
            {
                Add("Assembly version", va[0].Version);
            }

            // Add("Product version", fvi.ProductVersion);
            Add("Product version", String.Format("{0}.{1}, build {2}", fvi.ProductMajorPart, fvi.ProductMinorPart, fvi.ProductBuildPart));
            //Add("Build", fvi.ProductBuildPart);
            //Add("Revision", fvi.ProductPrivatePart);

            // Add("Debug version", fvi.IsDebug);            
            // Add("File version", fvi.FileVersion);
            // Add("Build", fvi.FileBuildPart);
            Add("Build time", fi.LastWriteTime);
            // Add("Last access", fi.LastAccessTime);
            // Add("Created", fi.CreationTime);
            // Add("Filename", System.IO.Path.GetFileName(fvi.FileName));

            AddSeparator();
            Add("Platform", Environment.OSVersion.Platform);
            Add("OS version", Environment.OSVersion.Version);
            Add("", Environment.OSVersion.ServicePack);
            Add("CLR version", Environment.Version);
            AddSeparator();
            Add("Machine name", Environment.MachineName);
            Add("Processors", Environment.ProcessorCount);
            Add("User", Environment.UserName);
            Add("Domain", Environment.UserDomainName);
            // CPU speed
            // Available memory
            // Available disk space
            // Hyperlinks
        }

        private readonly StringBuilder _content = new StringBuilder();

        public void Add(string label, object value)
        {
            if (value == null) return;
            string valueString = value.ToString().Trim();
            if (valueString.Length == 0) return;

            var tb = new TextBlock { Text = label, FontWeight = FontWeights.Bold, Margin = new Thickness(0, 0, 8, 0) };
            var tv = new TextBlock { Text = valueString };

            Grid.SetRow(tb, _row);
            Grid.SetRow(tv, _row);
            Grid.SetColumn(tv, 1);
            _row++;
            var rd = new RowDefinition { Height = GridLength.Auto };
            Grid1.RowDefinitions.Add(rd);

            Grid1.Children.Add(tb);
            Grid1.Children.Add(tv);
            _content.Append(label + ":\t" + valueString + "\r\n");
        }

        private void AddSeparator()
        {
            _row++;
            var rd = new RowDefinition { Height = new GridLength(8) };
            Grid1.RowDefinitions.Add(rd);
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void SystemInfo_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("MsInfo32.exe");
        }

        private void Copy_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(_content.ToString());
        }
    }
}