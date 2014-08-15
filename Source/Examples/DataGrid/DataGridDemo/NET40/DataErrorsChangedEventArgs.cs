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