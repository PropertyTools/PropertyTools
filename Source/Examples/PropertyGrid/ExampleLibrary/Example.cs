// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Example.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Base class for examples.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleLibrary
{
    using System.ComponentModel;

    /// <summary>
    /// Base class for examples.
    /// </summary>
    /// <remarks>This class depends on the NotifyPropertyWeaver task.
    /// All INotifyPropertyChanged events will be weaved into the code as a post-compile build step.</remarks>
    public class Example : INotifyPropertyChanged
    {
#pragma warning disable 67
        public event PropertyChangedEventHandler PropertyChanged;
#pragma warning restore 67

        public override string ToString()
        {
            return this.GetType().Name.Replace("Example", "");
        }
    }
}