using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

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

        public static void SendGetRequest(string url)
        {
            Task.Run(() =>
            {
                using (var client = new HttpClient())
                {
                    _ = client.GetAsync(url).Result;
                }
            });
        }
    }
}
