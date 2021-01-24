using System.Diagnostics;

namespace IISManager.Utils
{
    public static class ProcessUtils
    {
        public static void KillProcess(int processId)
        {
            Process.GetProcessById(processId).Kill();
        }

        public static void GoToPath(string path)
        {
            Process.Start(new ProcessStartInfo("explorer", path) { CreateNoWindow = true });
        }
    }
}
