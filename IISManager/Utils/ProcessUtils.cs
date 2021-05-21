using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

namespace IISManager.Utils
{
    public class ProcessUtils
    {
        private readonly ILogger<ProcessUtils> logger;
        public ProcessUtils(ILoggerFactory loggerFactory)
        {
            logger = loggerFactory.CreateLogger<ProcessUtils>();
        }

        public void KillProcess(int processId)
        {
            logger.LogTrace($"Kill process {processId} requested");

            try
            {
                Process.GetProcessById(processId).Kill();
            }
            catch (Exception ex)
            {
                logger.LogError($"Failed to kill process {ex}");
            }
        }

        public void GoToPath(string path)
        {
            logger.LogTrace($"Go to path {path} requested");

            try
            {
                Process.Start(new ProcessStartInfo("explorer", path) { CreateNoWindow = true });
            }
            catch (Exception ex)
            {
                logger.LogError($"Failed to open path {path} {ex}");
            }
        }

        public void SendGetRequest(string url)
        {
            logger.LogTrace($"Send get request {url} requested");

            Task.Run(() =>
            {
                try
                {
                    using (var client = new HttpClient())
                    {
                        _ = client.GetAsync(url).GetAwaiter().GetResult();
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError($"Failed to send GET request to {url} {ex}");
                }
            });
        }
    }
}
