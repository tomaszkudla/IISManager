using System.Diagnostics;

namespace IISManager.Utils
{
    public static class ProcessUtils
    {
        public static void KillProcess(int processId)
        {
            Process.GetProcessById(processId).Kill();
        }
    }
}
