// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FilePathAttributeExample.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleLibrary
{
    using System.IO;

    using PropertyTools.DataAnnotations;

    [PropertyGridExample]
    public class FilePathAttributeExample : Example
    {
        private string openFilePath;
        private string saveFilePath;
        private string inputFilePath;
        private string outputFilePath;
        private string testPath;
        private string relativePath;

        [InputFilePath(".txt")]
        [FilterProperty("Filter")]
        [AutoUpdateText]
        public string OpenFilePath { get => this.openFilePath; set { this.openFilePath = value; this.RaisePropertyChanged(nameof(OpenFilePath)); } }

        [OutputFilePath(".txt")]
        [FilterProperty("Filter")]
        public string SaveFilePath { get => this.saveFilePath; set { this.saveFilePath = value; this.RaisePropertyChanged(nameof(SaveFilePath)); } }

        [InputFilePath(".txt")]
        public string InputFilePath { get => this.inputFilePath; set { this.inputFilePath = value; this.RaisePropertyChanged(nameof(InputFilePath)); } }

        [OutputFilePath(".html")]
        public string OutputFilePath { get => this.outputFilePath; set { this.outputFilePath = value; this.RaisePropertyChanged(nameof(OutputFilePath)); } }

        [InputFilePath]
        [DefaultExtensionProperty("TestPathExtension")]
        [FilterProperty("TestPathFilter")]
        public string TestPath { get => this.testPath; set { this.testPath = value; this.RaisePropertyChanged(nameof(TestPath)); } }
        [Browsable(false)]
        public string TestPathFilter { get { return "CSV files (*.csv)|*.csv"; } }
        [Browsable(false)]
        public string TestPathExtension { get { return ".csv"; } }

        [InputFilePath(".txt")]
        [BasePathProperty("BasePath")]
        public string RelativePath { get => this.relativePath; set { this.relativePath = value; this.RaisePropertyChanged(nameof(RelativePath)); } }

        public string BasePath { get; private set; }

        [Browsable(false)]
        public string Filter { get; private set; }

        public FilePathAttributeExample()
        {
            this.BasePath = Directory.GetCurrentDirectory();
            this.Filter = "Text files (*.txt)|*.txt|Csv files (*.csv)|*.csv|All files (*.*)|*.*";
        }        
    }
}