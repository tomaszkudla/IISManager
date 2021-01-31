using IISManager.Utils;
using IISManager.ViewModels;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.ServiceProcess;
using System.Threading.Tasks;

namespace IISManager.Implementations
{
    public sealed class IISServerManager
    {
        private readonly ILogger<IISServerManager> logger;
        private readonly UserMessage userMessage;

        public IISServerManager(ILoggerFactory loggerFactory, UserMessage userMessage)
        {
            logger = loggerFactory.CreateLogger<IISServerManager>();
            this.userMessage = userMessage;
        }

        public void Start()
        {
            GetIISResetProcess("/start").Start();
        }

        public void Stop()
        {
            Task.Run(() =>
            {
                logger.LogTrace("IIS stop requested");
                try
                {
                    var process = GetIISResetProcess("/stop");
                    process.Start();
                    process.WaitForExit();
                    var output = process.StandardOutput.ReadToEnd().TrimEnd();
                    if (process.ExitCode != 0)
                    {
                        logger.LogError($"Failed to stop IIS. Console output:{output}");
                    }
                    else
                    {
                        logger.LogTrace($"IIS stopped. Console output:{output}");
                    }
                }
                catch (Exception ex)
                {
                    if (IsIISStopped())
                    {
                        logger.LogTrace($"Exception during IIS stop request. {ex}");
                    }
                    else
                    {
                        logger.LogError($"Failed to stop IIS. {ex}");
                    }
                }
            });
        }

        public void Reset()
        {
            GetIISResetProcess().Start();
        }

        public bool IsIISStopped()
        {
            using (var sc = new ServiceController("W3SVC"))
            {
                return sc.Status.Equals(ServiceControllerStatus.Stopped) || sc.Status.Equals(ServiceControllerStatus.StopPending);
            }
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
