using System.Diagnostics;

namespace IISManager.Implementations
{
    public static class ProcessUtils
    {
        public static void KillProcess(int processId)
        {
            Process.GetProcessById(processId).Kill();
        }
    }
}
