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
        public DateTime Time { get; set; }

        public double Value { get; set; }

        [Height(60)]
        public string Comments { get; set; }

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