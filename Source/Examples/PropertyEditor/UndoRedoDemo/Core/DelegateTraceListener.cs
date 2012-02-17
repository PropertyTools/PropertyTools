namespace UndoRedoDemo
{
    using System;
    using System.Diagnostics;

    public class DelegateTraceListener : TraceListener
    {
        public event Action<string, bool> OnAppend;

        public override void Write(string message)
        {
            if (this.OnAppend != null)
                this.OnAppend.Invoke(message, false);
        }

        public override void WriteLine(string message)
        {
            if (this.OnAppend != null)
                this.OnAppend.Invoke(message, true);
        }

        public DelegateTraceListener()
        {
            Trace.Listeners.Add(this);
        }

        ~DelegateTraceListener()
        {
            Trace.Listeners.Remove(this);
        }
    }
}