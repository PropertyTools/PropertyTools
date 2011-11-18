namespace TestLibrary
{
    using System.ComponentModel;

    /// <summary>
    /// Base class for test objects.
    /// </summary>
    /// <remarks>
    /// This class depends on the NotifyPropertyWeaver task.
    /// All INotifyPropertyChanged events will be weaved into the code as a post-compile build step.
    /// </remarks>
    public class TestBase : INotifyPropertyChanged
    {
#pragma warning disable 67
        public event PropertyChangedEventHandler PropertyChanged;
#pragma warning restore 67
    }
}