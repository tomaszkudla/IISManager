using System.Diagnostics;

namespace IISManager.Implementations
{
    public class CurrentProcessWrapper
    {
        public CurrentProcessWrapper()
        {
            CurrentProcess = Process.GetCurrentProcess();
        }

        public Process CurrentProcess { get; }
    }
}
