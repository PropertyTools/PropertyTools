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
        public event PropertyChangedEventHandler PropertyChanged;
    }
}