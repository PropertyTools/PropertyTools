// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FormatStringAttributeExample.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleLibrary
{
    using System;

    using PropertyTools.DataAnnotations;

    [PropertyGridExample]
    public class FormatStringAttributeExample : Example
    {
        private double @double;
        private int integer;
        private TimeSpan timeSpan1;
        private TimeSpan timeSpan2;
        private DateTime dateTime1;
        private DateTime dateTime2;
        private DateTime dateTime3;
        private DateTime dateTime4;
        private DateTime date;
        private DateTime date2;
        private DateTime time;

        [Category("Double")]
        [FormatString("0.00")]
        public double Double { get => this.@double; set { this.@double = value; this.RaisePropertyChanged(nameof(Double)); } }

        [Category("Int")]
        [FormatString("000")]
        public int Integer { get => this.integer; set { this.integer = value; this.RaisePropertyChanged(nameof(Integer)); } }

        [Category("TimeSpan")]
        [FormatString("hh:mm")]
        [Description("hh:mm")]
        public TimeSpan TimeSpan1 { get => this.timeSpan1; set { this.timeSpan1 = value; this.RaisePropertyChanged(nameof(TimeSpan1)); } }

        [FormatString("mm:ss")]
        [Description("mm:ss")]
        public TimeSpan TimeSpan2 { get => this.timeSpan2; set { this.timeSpan2 = value; this.RaisePropertyChanged(nameof(TimeSpan2)); } }

        [Category("DateTime")]
        [FormatString("yyyy-MM-dd hh:mmt")]
        [Description("yyyy-MM-dd hh:mmt")]
        public DateTime DateTime1 { get => this.dateTime1; set { this.dateTime1 = value; this.RaisePropertyChanged(nameof(DateTime1)); } }

        [FormatString("MM/dd/yyyy hh.mm.sstt")]
        [Description("MM/dd/yyyy hh.mm.sstt")]
        public DateTime DateTime2 { get => this.dateTime2; set { this.dateTime2 = value; this.RaisePropertyChanged(nameof(DateTime2)); } }

        [FormatString("yyyy-MM-dd HH:mm")]
        [Description("yyyy-MM-dd HH:mm")]
        public DateTime DateTime3 { get => this.dateTime3; set { this.dateTime3 = value; this.RaisePropertyChanged(nameof(DateTime3)); } }

        [FormatString("yyyy-MM-dd")]
        [Description("yyyy-MM-dd")]
        public DateTime Date { get => this.date; set { this.date = value; this.RaisePropertyChanged(nameof(Date)); } }

        [FormatString("dd/MM/yyyy")]
        [Description("dd/MM/yyyy")]
        public DateTime Date2 { get => this.date2; set { this.date2 = value; this.RaisePropertyChanged(nameof(Date2)); } }

        [FormatString("hh:mm")]
        public DateTime Time { get => this.time; set { this.time = value; this.RaisePropertyChanged(nameof(Time)); } }

        public FormatStringAttributeExample()
        {
            this.Double = Math.PI;
            this.Integer = 1;

            this.TimeSpan1 = new TimeSpan(0, 12, 39, 0);
            this.TimeSpan2 = new TimeSpan(0, 0, 12, 39);
            this.Date = this.Date2 = this.Time = this.DateTime1 = this.DateTime2 = this.DateTime3 = new DateTime(2012, 3, 5, 21, 34, 14);
        }
    }
}
