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
        [InputFilePath(".txt")]
        [FilterProperty("Filter")]
        [AutoUpdateText]
        public string OpenFilePath { get; set; }

        [OutputFilePath(".txt")]
        [FilterProperty("Filter")]
        public string SaveFilePath { get; set; }

        [InputFilePath(".txt")]
        public string InputFilePath { get; set; }

        [OutputFilePath(".html")]
        public string OutputFilePath { get; set; }

        [InputFilePath]
        [DefaultExtensionProperty("TestPathExtension")]
        [FilterProperty("TestPathFilter")]
        public string TestPath { get; set; }
        [Browsable(false)]
        public string TestPathFilter { get { return "CSV files (*.csv)|*.csv"; } }
        [Browsable(false)]
        public string TestPathExtension { get { return ".csv"; } }

        [InputFilePath(".txt")]
        [BasePathProperty("BasePath")]
        public string RelativePath { get; set; }

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