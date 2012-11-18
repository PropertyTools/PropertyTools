// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestFilePathAttribute.cs" company="PropertyTools">
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
// --------------------------------------------------------------------------------------------------------------------
namespace TestLibrary
{
    using System.IO;

    using PropertyTools.DataAnnotations;

    public class TestFilePathAttribute : TestBase
    {
        [FilePath(".txt", true)]
        [FilterProperty("Filter")]
        public string OpenFilePath { get; set; }

        [FilePath(".txt", false)]
        [FilterProperty("Filter")]
        public string SaveFilePath { get; set; }

        [FilePath(".txt", true)]
        [BasePathProperty("BasePath")]
        public string RelativePath { get; set; }

        public string BasePath { get; private set; }

        public string Filter { get; private set; }

        public TestFilePathAttribute()
        {
            BasePath = Directory.GetCurrentDirectory();
            Filter= "Text files (*.txt)|*.txt|Csv files (*.csv)|*.csv|All files (*.*)|*.*";
        }

        public override string ToString()
        {
            return "FilePath attribute";
        }
    }
}