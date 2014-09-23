// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataErrorsChangedEventArgs.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools
{
    using System;

    public class DataErrorsChangedEventArgs : EventArgs
    {
        public DataErrorsChangedEventArgs(string propertyName)
        {
            this.PropertyName = propertyName;
        }

        public string PropertyName { get; set; }
    }
}