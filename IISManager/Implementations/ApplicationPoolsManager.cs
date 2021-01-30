using IISManager.Utils;
using IISManager.ViewModels;
using Microsoft.Extensions.Logging;
using Microsoft.Web.Administration;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IISManager.Implementations
{
    public sealed class ApplicationPoolsManager
    {
        private readonly ILogger<ApplicationPoolsManager> logger;
        private readonly ProcessDiagnostics processDiagnostics;
        private readonly IISServerManager iisServerManager;
        private readonly UserMessage userMessage;
        private readonly ApplicationPoolsList applicationPools = new ApplicationPoolsList();

        public ApplicationPoolsManager(ILoggerFactory loggerFactory, ProcessDiagnostics processDiagnostics, IISServerManager iisServerManager, UserMessage userMessage)
        {
            logger = loggerFactory.CreateLogger<ApplicationPoolsManager>();
            this.processDiagnostics = processDiagnostics;
            this.iisServerManager = iisServerManager;
            this.userMessage = userMessage;
            Refresh();
        }

        public ApplicationPoolsList ApplicationPools
        {
            get
            {
                return applicationPools;
            }
        }

        public Observable<bool> AllSelected { get; } = new Observable<bool>();

        public void Refresh()
        {
            try
            {
                using (var serverManager = new ServerManager())
                {
                    var isIISStopped = iisServerManager.IsIISStopped();
                    if (!isIISStopped)
                    {
                        processDiagnostics.Refresh(serverManager.WorkerProcesses.Select(p => p.ProcessId));
                    }

                    var applications = GetApplications(serverManager.Sites);
                    applicationPools.Value = serverManager.ApplicationPools.Select(p => new ViewModels.ApplicationPool(p, applications.Where(a => a.ApplicationPoolName == p.Name).ToList(), processDiagnostics)).ToList();
                    
                    if (isIISStopped && applicationPools.Value.All(p => p.State == ApplicationPoolState.Stopped))
                    {
                        userMessage.SetWarn(UserMessageText.IISIsNotRunning);
                    }
                    else if (userMessage.Message == UserMessageText.IISIsNotRunning)
                    {
                        userMessage.SetInfo(UserMessageText.IISStarted);
                    }
                }
            }
            catch (UnauthorizedAccessException)
            {
                userMessage.SetError("Please run the application as an administrator");
            }
            catch (Exception ex)
            {
                logger.LogError($"Error during refresh. {ex}");
            }
        }

        public void Select(string name)
        {
            applicationPools.SelectedApplicationPools.Add(name);
        }

        public void Unselect(string name)
        {
            applicationPools.SelectedApplicationPools.Remove(name);
            AllSelected.Value = false;
        }

        public void SelectAll()
        {
            applicationPools.SelectedApplicationPools = new HashSet<string>(applicationPools.Value.Select(p => p.Name));
        }

        public void UnselectAll()
        {
            applicationPools.SelectedApplicationPools = new HashSet<string>();
        }

        public void StartSelected()
        {
            var selectedPoolNames = applicationPools.Value.Where(p => p.IsSelected).Select(p => p.Name);
            using (var serverManager = new ServerManager())
            {
                var allPools = serverManager.ApplicationPools;
                var selectedPools = selectedPoolNames.Select(n => allPools.FirstOrDefault(p => p.Name == n)).Where(n => n != null);
                foreach (var appPool in selectedPools)
                {
                    if (appPool.State != ObjectState.Started)
                    {
                        appPool.Start();
                    }
                }
            }
        }

        public void StopSelected()
        {
            var selectedPoolNames = applicationPools.Value.Where(p => p.IsSelected).Select(p => p.Name);
            using (var serverManager = new ServerManager())
            {
                var allPools = serverManager.ApplicationPools;
                var selectedPools = selectedPoolNames.Select(n => allPools.FirstOrDefault(p => p.Name == n)).Where(n => n != null);
                foreach (var appPool in selectedPools)
                {
                    if (appPool.State != ObjectState.Stopped)
                    {
                        appPool.Stop();
                    }
                }
            }
        }

        public void RecycleSelected()
        {
            var selectedPoolNames = applicationPools.Value.Where(p => p.IsSelected).Select(p => p.Name);
            using (var serverManager = new ServerManager())
            {
                var allPools = serverManager.ApplicationPools;
                var selectedPools = selectedPoolNames.Select(n => allPools.FirstOrDefault(p => p.Name == n)).Where(n => n != null);
                foreach (var appPool in selectedPools)
                {
                    if (appPool.State != ObjectState.Stopped)
                    {
                        appPool.Recycle();
                    }
                }
            }
        }

        private List<ViewModels.Application> GetApplications(SiteCollection sites)
        {
            var applicationList = new List<ViewModels.Application>();
            foreach (var site in sites)
            {
                var rootDirPath = site.Applications["/"].VirtualDirectories["/"].PhysicalPath;
                var rootWebPath = "http://localhost:" + site.Bindings.FirstOrDefault(b => b.Protocol.ToLower() == "http").EndPoint.Port;
                foreach (var application in site.Applications)
                {
                    var applicationPoolName = application.ApplicationPoolName;
                    var applicationPath = application.Path;
                    var dirPath = application.VirtualDirectories["/"].PhysicalPath;
                    var webPath = rootWebPath + applicationPath;
                    applicationList.Add(new ViewModels.Application(applicationPoolName, applicationPath, dirPath, webPath));
                }
            }

            return applicationList;
        }
    }
}
