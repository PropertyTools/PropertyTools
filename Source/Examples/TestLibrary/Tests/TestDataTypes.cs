// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestDataTypes.cs" company="PropertyTools">
//   The MIT License (MIT)
//   
//   Copyright (c) 2014 PropertyTools contributors
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
    using System;
    using System.ComponentModel.DataAnnotations;

    [PropertyGridExample]
    public class TestDataTypes : TestBase
    {
        //// http://msdn.microsoft.com/en-us/library/dd901590(v=vs.95).aspx

        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime DateTime { get; set; }

        [DataType(DataType.Duration)]
        public TimeSpan Duration { get; set; }

        [DataType(DataType.EmailAddress)]
        public string EmailAddress { get; set; }

        [DataType(DataType.Html)]
        public string Html { get; set; }

        [DataType(DataType.ImageUrl)]
        public Uri ImageUrl { get; set; }

        [DataType(DataType.MultilineText)]
        public string MultilineText { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [DataType(DataType.Text)]
        public string Text { get; set; }

        [DataType(DataType.Time)]
        public DateTime Time { get; set; }

        [DataType(DataType.Url)]
        public Uri Url { get; set; }

        public TestDataTypes()
        {
            Date = DateTime.Now;
            DateTime = DateTime.Now;
            Duration = new TimeSpan(0, 0, 0, 9, 580);
            EmailAddress = "email@address.org";
            ImageUrl = new Uri("http://www.google.com/images/srpr/logo3w.png");
            Html = @"<html><body><h1>Title</h1><p>Paragrapgh</p><p><a href=""http://www.google.com"">google.com</a></body></html>";
            MultilineText = "Line1\nLine2";
            Password = "S3cr3t";
            PhoneNumber = "510-123-4567";
            Text = "A text, within literary theory, is a coherent set of symbols that transmits some kind of informative message.[citation needed] This set of symbols is considered in terms of the informative message's content, rather than in terms of its physical form or the medium in which it is represented. In the most basic terms established by structuralist criticism, therefore, a text is any object that can be read, whether this object is a work of literature, a street sign, an arrangement of buildings on a city block, or styles of clothing.";
            Time = DateTime.Now;
            Url = new Uri("http://www.google.com");
        }

        public override string ToString()
        {
            return "Data types";
        }
    }
}