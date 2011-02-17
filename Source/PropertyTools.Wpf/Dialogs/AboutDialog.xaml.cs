using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace PropertyTools.Wpf
{
    /// <summary>
    /// A basic About Dialog (inspired by Google)
    /// </summary>
    public partial class AboutDialog : Window
    {
        private readonly AboutViewModel vm;

        public AboutDialog(Window owner)
        {
            this.Owner = owner;
            this.Icon = owner.Icon;

            InitializeComponent();
            vm = new AboutViewModel(Assembly.GetCallingAssembly());
            DataContext = vm;
        }

        /// <summary>
        /// Sets the image used in the about dialog.
        /// Example:
        ///  d.Image = new BitmapImage(new Uri(@"pack://application:,,,/AssemblyName;component/Images/about.png"));           
        /// </summary>
        /// <value>The image.</value>
        public ImageSource Image
        {
            set { vm.Image = value; }
        }

        /// <summary>
        /// Sets the update status.
        /// </summary>
        /// <value>The update status.</value>
        public string UpdateStatus
        {
            set { vm.UpdateStatus = value; }
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
            Clipboard.SetText(vm.GetReport());
        }
    }

    public class AboutViewModel
    {
        public Assembly Assembly { get; set; }
        public FileVersionInfo FileVersionInfo { get; set; }
        public FileInfo FileInfo { get; set; }

        public AboutViewModel(Assembly a)
        {
            if (a == null)
                throw new InvalidOperationException();
            if (a.Location == null)
                throw new InvalidOperationException();

            FileVersionInfo = FileVersionInfo.GetVersionInfo(a.Location);
            FileInfo = new FileInfo(FileVersionInfo.FileName);

            var va = (AssemblyVersionAttribute[])a.GetCustomAttributes(typeof(AssemblyVersionAttribute),true);
            if (va != null && va.Length > 0)
            {
                AssemblyVersion = va[0].Version;
            }

        }

        public ImageSource Image { get; set; }
        public string AssemblyVersion { get; private set; }
        public string ProductName { get { return FileVersionInfo.ProductName; } }
        public string Version { get { return FileVersionInfo.ProductVersion; } }
        public string Copyright { get { return FileVersionInfo.LegalCopyright; } }
        public string Comments { get { return FileVersionInfo.Comments; } }
        public string Company { get { return FileVersionInfo.CompanyName; } }

        public string FileVersion { get { return FileVersionInfo.FileVersion; } }
        public string BuildTime { get { return FileInfo.LastWriteTime.ToString(); } }
        public string FileName { get { return Path.GetFullPath(FileVersionInfo.FileName); } }

        public string Platform { get { return Environment.OSVersion.Platform.ToString(); } }
        public string OSVersion { get { return Environment.OSVersion.Version.ToString(); } }
        public string ServicePack { get { return Environment.OSVersion.ServicePack; } }
        public string CLRversion { get { return Environment.Version.ToString(); } }
        public string MachineName { get { return Environment.MachineName; } }
        public int Processors { get { return Environment.ProcessorCount; } }
        public string User { get { return Environment.UserName; } }
        public string Domain { get { return Environment.UserDomainName; } }

        public string UpdateStatus { get; set; }

        public string GetReport()
        {
            var sb = new StringBuilder();
            sb.AppendFormat("Product: {0}", ProductName);
            sb.AppendLine();
            sb.AppendFormat("Product Version: {0}", Version);
            sb.AppendLine();
            sb.AppendFormat("Copyright: {0}", Copyright);
            sb.AppendLine();
            sb.AppendFormat("Company: {0}", Company);
            sb.AppendLine();

            sb.AppendFormat("Assembly version: {0}", AssemblyVersion);
            sb.AppendLine();
            sb.AppendFormat("File version: {0}", FileVersion);
            sb.AppendLine();
            sb.AppendFormat("Build time: {0}", BuildTime);
            sb.AppendLine();
            sb.AppendFormat("FileName: {0}", FileName);
            sb.AppendLine();
            sb.AppendFormat("Platform: {0}", Platform);
            sb.AppendLine();
            sb.AppendFormat("OS version: {0}", OSVersion);
            sb.AppendLine();
            sb.AppendFormat("Service Pack: {0}", ServicePack);
            sb.AppendLine();
            sb.AppendFormat("CLR version: {0}", CLRversion);
            sb.AppendLine();
            sb.AppendFormat("Machine name: {0}", MachineName);
            sb.AppendLine();
            sb.AppendFormat("Processors: {0}", Processors);
            sb.AppendLine();
            sb.AppendFormat("User: {0}", User);
            sb.AppendLine();
            sb.AppendFormat("Domain: {0}", Domain);
            return sb.ToString();
        }
          
    }
}