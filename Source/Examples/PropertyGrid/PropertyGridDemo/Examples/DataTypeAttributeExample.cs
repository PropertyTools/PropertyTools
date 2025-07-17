// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataTypeAttributeExample.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleLibrary
{
    using System;
    using System.ComponentModel.DataAnnotations;

    [PropertyGridExample]
    public class DataTypeAttributeExample : Example
    {
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

        public DataTypeAttributeExample()
        {
            this.Date = DateTime.Now;
            this.DateTime = DateTime.Now;
            this.Duration = new TimeSpan(0, 0, 0, 9, 580);
            this.EmailAddress = "email@address.org";
            this.ImageUrl = new Uri("https://www.google.com/images/srpr/logo3w.png");
            this.Html = @"<html><body><h1>Title</h1><p>Paragrapgh</p><p><a href=""https://www.google.com"">google.com</a></body></html>";
            this.MultilineText = "Line1\nLine2";
            this.Password = "S3cr3t";
            this.PhoneNumber = "510-123-4567";
            this.Text = "A text, within literary theory, is a coherent set of symbols that transmits some kind of informative message.[citation needed] This set of symbols is considered in terms of the informative message's content, rather than in terms of its physical form or the medium in which it is represented. In the most basic terms established by structuralist criticism, therefore, a text is any object that can be read, whether this object is a work of literature, a street sign, an arrangement of buildings on a city block, or styles of clothing.";
            this.Time = DateTime.Now;
            this.Url = new Uri("https://www.google.com");
        }
    }
}