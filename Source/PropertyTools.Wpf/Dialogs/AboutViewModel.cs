// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AboutViewModel.cs" company="PropertyTools">
//   http://propertytools.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Reflection;
    using System.Text;
    using System.Windows.Media;

    /// <summary>
    /// Represents a viewmodel for the about dialog.
    /// </summary>
    public class AboutViewModel
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AboutViewModel"/> class.
        /// </summary>
        /// <param name="a">
        /// An assembly.
        /// </param>
        public AboutViewModel(Assembly a)
        {
            this.SystemInfoText = "System Info...";
            this.CopyReportText = "Copy report";

            if (a == null)
            {
                throw new InvalidOperationException();
            }

            if (a.Location == null)
            {
                throw new InvalidOperationException();
            }

            this.FileVersionInfo = FileVersionInfo.GetVersionInfo(a.Location);
            this.FileInfo = new FileInfo(this.FileVersionInfo.FileName);

            var va = (AssemblyVersionAttribute[])a.GetCustomAttributes(typeof(AssemblyVersionAttribute), true);
            if (va != null && va.Length > 0)
            {
                this.AssemblyVersion = va[0].Version;
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets AssemblyVersion.
        /// </summary>
        public string AssemblyVersion { get; private set; }

        /// <summary>
        /// Gets BuildTime.
        /// </summary>
        public string BuildTime
        {
            get
            {
                return this.FileInfo.LastWriteTime.ToString();
            }
        }

        /// <summary>
        /// Gets CLRversion.
        /// </summary>
        public string CLRversion
        {
            get
            {
                return Environment.Version.ToString();
            }
        }

        /// <summary>
        /// Gets Comments.
        /// </summary>
        public string Comments
        {
            get
            {
                return this.FileVersionInfo.Comments;
            }
        }

        /// <summary>
        /// Gets Company.
        /// </summary>
        public string Company
        {
            get
            {
                return this.FileVersionInfo.CompanyName;
            }
        }

        /// <summary>
        ///   Gets or sets the copy report text.
        /// </summary>
        /// <value>The copy report text.</value>
        public string CopyReportText { get; set; }

        /// <summary>
        /// Gets Copyright.
        /// </summary>
        public string Copyright
        {
            get
            {
                return this.FileVersionInfo.LegalCopyright;
            }
        }

        /// <summary>
        /// Gets Domain.
        /// </summary>
        public string Domain
        {
            get
            {
                return Environment.UserDomainName;
            }
        }

        /// <summary>
        ///   Gets or sets the file info.
        /// </summary>
        /// <value>The file info.</value>
        public FileInfo FileInfo { get; set; }

        /// <summary>
        /// Gets FileName.
        /// </summary>
        public string FileName
        {
            get
            {
                return Path.GetFullPath(this.FileVersionInfo.FileName);
            }
        }

        /// <summary>
        /// Gets FileVersion.
        /// </summary>
        public string FileVersion
        {
            get
            {
                return this.FileVersionInfo.FileVersion;
            }
        }

        /// <summary>
        ///   Gets or sets the file version info.
        /// </summary>
        /// <value>The file version info.</value>
        public FileVersionInfo FileVersionInfo { get; set; }

        /// <summary>
        /// Gets or sets Image.
        /// </summary>
        public ImageSource Image { get; set; }

        /// <summary>
        /// Gets MachineName.
        /// </summary>
        public string MachineName
        {
            get
            {
                return Environment.MachineName;
            }
        }

        /// <summary>
        /// Gets OSVersion.
        /// </summary>
        public string OSVersion
        {
            get
            {
                return Environment.OSVersion.Version.ToString();
            }
        }

        /// <summary>
        /// Gets Platform.
        /// </summary>
        public string Platform
        {
            get
            {
                return Environment.OSVersion.Platform.ToString();
            }
        }

        /// <summary>
        /// Gets Processors.
        /// </summary>
        public int Processors
        {
            get
            {
                return Environment.ProcessorCount;
            }
        }

        /// <summary>
        /// Gets ProductName.
        /// </summary>
        public string ProductName
        {
            get
            {
                return this.FileVersionInfo.ProductName;
            }
        }

        /// <summary>
        /// Gets ServicePack.
        /// </summary>
        public string ServicePack
        {
            get
            {
                return Environment.OSVersion.ServicePack;
            }
        }

        /// <summary>
        ///   Gets or sets the system info text.
        /// </summary>
        /// <value>The system info text.</value>
        public string SystemInfoText { get; set; }

        /// <summary>
        /// Gets or sets UpdateStatus.
        /// </summary>
        public string UpdateStatus { get; set; }

        /// <summary>
        /// Gets User.
        /// </summary>
        public string User
        {
            get
            {
                return Environment.UserName;
            }
        }

        /// <summary>
        /// Gets Version.
        /// </summary>
        public string Version
        {
            get
            {
                return this.FileVersionInfo.ProductVersion;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the report.
        /// </summary>
        /// <returns>
        /// The get report.
        /// </returns>
        public string GetReport()
        {
            var sb = new StringBuilder();
            sb.AppendFormat("Product: {0}", this.ProductName);
            sb.AppendLine();
            sb.AppendFormat("Product Version: {0}", this.Version);
            sb.AppendLine();
            sb.AppendFormat("Copyright: {0}", this.Copyright);
            sb.AppendLine();
            sb.AppendFormat("Company: {0}", this.Company);
            sb.AppendLine();
            sb.AppendFormat("Comments: {0}", this.Comments);
            sb.AppendLine();
            sb.AppendFormat("Assembly version: {0}", this.AssemblyVersion);
            sb.AppendLine();
            sb.AppendFormat("File version: {0}", this.FileVersion);
            sb.AppendLine();
            sb.AppendFormat("Build time: {0}", this.BuildTime);
            sb.AppendLine();
            sb.AppendFormat("FileName: {0}", this.FileName);
            sb.AppendLine();
            sb.AppendFormat("Platform: {0}", this.Platform);
            sb.AppendLine();
            sb.AppendFormat("OS version: {0}", this.OSVersion);
            sb.AppendLine();
            sb.AppendFormat("Service Pack: {0}", this.ServicePack);
            sb.AppendLine();
            sb.AppendFormat("CLR version: {0}", this.CLRversion);
            sb.AppendLine();
            sb.AppendFormat("Machine name: {0}", this.MachineName);
            sb.AppendLine();
            sb.AppendFormat("Processors: {0}", this.Processors);
            sb.AppendLine();
            sb.AppendFormat("User: {0}", this.User);
            sb.AppendLine();
            sb.AppendFormat("Domain: {0}", this.Domain);
            return sb.ToString();
        }

        #endregion
    }
}