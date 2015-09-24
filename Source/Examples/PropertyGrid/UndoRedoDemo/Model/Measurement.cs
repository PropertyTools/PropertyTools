// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Measurement.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace UndoRedoDemo
{
    using System;
    using System.Globalization;

    using PropertyTools.DataAnnotations;

    public class Measurement : UndoableObject
    {
        private DateTime time;
        private double value;
        private string comments;

        public DateTime Time
        {
            get { return this.time; }
            set { this.SetValue(ref this.time, value, "Time"); }
        }
        public double Value
        {
            get { return this.value; }
            set { this.SetValue(ref this.value, value, "Value"); }
        }

        [Height(60)]
        public string Comments
        {
            get { return this.comments; }
            set { this.SetValue(ref this.comments, value, "Comments"); }
        }
        public Measurement()
        {
            BeginInit();
            Time = DateTime.Now;
            Value = 12.34;
            EndInit();
        }

        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "{0} = {1}", this.Time, this.Value);
        }
    }
}