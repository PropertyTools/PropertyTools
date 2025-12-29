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
        private DateTime date;
        private DateTime dateTime;
        private TimeSpan duration;
        private string emailAddress;
        private string html;
        private Uri imageUrl;
        private string multilineText;
        private string password;
        private string phoneNumber;
        private string text;
        private DateTime time;
        private Uri url;

        [DataType(DataType.Date)]
        public DateTime Date { get => this.date; set { this.date = value; this.RaisePropertyChanged(nameof(Date)); } }

        [DataType(DataType.DateTime)]
        public DateTime DateTime { get => this.dateTime; set { this.dateTime = value; this.RaisePropertyChanged(nameof(DateTime)); } }

        [DataType(DataType.Duration)]
        public TimeSpan Duration { get => this.duration; set { this.duration = value; this.RaisePropertyChanged(nameof(Duration)); } }

        [DataType(DataType.EmailAddress)]
        public string EmailAddress { get => this.emailAddress; set { this.emailAddress = value; this.RaisePropertyChanged(nameof(EmailAddress)); } }

        [DataType(DataType.Html)]
        public string Html { get => this.html; set { this.html = value; this.RaisePropertyChanged(nameof(Html)); } }

        [DataType(DataType.ImageUrl)]
        public Uri ImageUrl { get => this.imageUrl; set { this.imageUrl = value; this.RaisePropertyChanged(nameof(ImageUrl)); } }

        [DataType(DataType.MultilineText)]
        public string MultilineText { get => this.multilineText; set { this.multilineText = value; this.RaisePropertyChanged(nameof(MultilineText)); } }

        [DataType(DataType.Password)]
        public string Password { get => this.password; set { this.password = value; this.RaisePropertyChanged(nameof(Password)); } }

        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get => this.phoneNumber; set { this.phoneNumber = value; this.RaisePropertyChanged(nameof(PhoneNumber)); } }

        [DataType(DataType.Text)]
        public string Text { get => this.text; set { this.text = value; this.RaisePropertyChanged(nameof(Text)); } }

        [DataType(DataType.Time)]
        public DateTime Time { get => this.time; set { this.time = value; this.RaisePropertyChanged(nameof(Time)); } }

        [DataType(DataType.Url)]
        public Uri Url { get => this.url; set { this.url = value; this.RaisePropertyChanged(nameof(Url)); } }

        public DataTypeAttributeExample()
        {
            this.Date = System.DateTime.Now;
            this.DateTime = System.DateTime.Now;
            this.Duration = new TimeSpan(0, 0, 0, 9, 580);
            this.EmailAddress = "email@address.org";
            this.ImageUrl = new Uri("https://www.google.com/images/srpr/logo3w.png");
            this.Html = @"<html><body><h1>Title</h1><p>Paragrapgh</p><p><a href=""https://www.google.com"">google.com</a></body></html>";
            this.MultilineText = "Line1\nLine2";
            this.Password = "S3cr3t";
            this.PhoneNumber = "510-123-4567";
            this.Text = "A text, within literary theory, is a coherent set of symbols that transmits some kind of informative message.[citation needed] This set of symbols is considered in terms of the informative message's content, rather than in terms of its physical form or the medium in which it is represented. In the most basic terms established by structuralist criticism, therefore, a text is any object that can be read, whether this object is a work of literature, a street sign, an arrangement of buildings on a city block, or styles of clothing.";
            this.Time = System.DateTime.Now;
            this.Url = new Uri("https://www.google.com");
        }
    }
}