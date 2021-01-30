using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace IISManager.Implementations
{
    public sealed class IISServerManager
    {
        private readonly ILogger<IISServerManager> logger;

        public IISServerManager(ILoggerFactory loggerFactory)
        {
            logger = loggerFactory.CreateLogger<IISServerManager>();
        }

        public void Start()
        {
            GetIISResetProcess("/start").Start();
        }

        public void Stop()
        {
            GetIISResetProcess("/stop").Start();
        }

        public void Reset()
        {
            GetIISResetProcess().Start();
        }

        private Process GetIISResetProcess(string arguments = null)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = "iisreset.exe";
            startInfo.Arguments = arguments;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;
            startInfo.UseShellExecute = false;
            startInfo.CreateNoWindow = true;

            Process process = new Process();
            process.StartInfo = startInfo;
            process.EnableRaisingEvents = true;

            return process;
        }
    }
}
