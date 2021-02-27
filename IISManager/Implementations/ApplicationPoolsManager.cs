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
        private string lastRefreshExceptionMessage;

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
                    if (isIISStopped)
                    {
                        processDiagnostics.Refresh(Enumerable.Empty<int>());
                    }
                    else
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

                lastRefreshExceptionMessage = null;
            }
            catch (UnauthorizedAccessException ex)
            {
                if (ex.Message.Contains("Cannot read configuration file due to insufficient permissions"))
                {
                    userMessage.SetError(UserMessageText.RunAsAdmin);
                }
            }
            catch (Exception ex)
            {
                userMessage.SetError(UserMessageText.ErrorDuringRefresh);
                if (ex.Message != lastRefreshExceptionMessage)
                {
                    logger.LogError($"Error during refresh. {ex}");
                    lastRefreshExceptionMessage = ex.Message;
                }
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
            if (!selectedPoolNames.Any() || iisServerManager.IsIISStopped())
            {
                return;
            }

            logger.LogTrace($"Start requested for the following application pools:{Environment.NewLine}{string.Join(Environment.NewLine, selectedPoolNames)}".TrimEnd());

            try
            {
                using (var serverManager = new ServerManager())
                {
                    var allPools = serverManager.ApplicationPools;
                    var selectedPools = selectedPoolNames.Select(n => allPools.FirstOrDefault(p => p.Name == n)).Where(n => n != null);
                    foreach (var appPool in selectedPools)
                    {
                        if (appPool.State != ObjectState.Started && appPool.State != ObjectState.Starting)
                        {
                            try
                            {
                                appPool.Start();
                            }
                            catch (Exception ex)
                            {
                                logger.LogError($"Failed to start an application pool {appPool.Name} {ex}");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError($"Failed to start selected application pools {ex}");
            }
        }

        public void StopSelected()
        {
            var selectedPoolNames = applicationPools.Value.Where(p => p.IsSelected).Select(p => p.Name);
            if (!selectedPoolNames.Any() || iisServerManager.IsIISStopped())
            {
                return;
            }

            logger.LogTrace($"Stop requested for the following application pools:{Environment.NewLine}{string.Join(Environment.NewLine, selectedPoolNames)}".TrimEnd());

            try
            {
                using (var serverManager = new ServerManager())
                {
                    var allPools = serverManager.ApplicationPools;
                    var selectedPools = selectedPoolNames.Select(n => allPools.FirstOrDefault(p => p.Name == n)).Where(n => n != null);
                    foreach (var appPool in selectedPools)
                    {
                        if (appPool.State != ObjectState.Stopped && appPool.State != ObjectState.Stopping)
                        {
                            try
                            {
                                appPool.Stop();
                            }
                            catch (Exception ex)
                            {
                                logger.LogError($"Failed to stop an application pool {appPool.Name} {ex}");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError($"Failed to stop selected application pools {ex}");
            }
        }

        public void RecycleSelected()
        {
            var selectedPoolNames = applicationPools.Value.Where(p => p.IsSelected).Select(p => p.Name);
            if (!selectedPoolNames.Any() || iisServerManager.IsIISStopped())
            {
                return;
            }

            logger.LogTrace($"Recycle requested for the following application pools:{Environment.NewLine}{string.Join(Environment.NewLine, selectedPoolNames)}".TrimEnd());

            try
            {
                using (var serverManager = new ServerManager())
                {
                    var allPools = serverManager.ApplicationPools;
                    var selectedPools = selectedPoolNames.Select(n => allPools.FirstOrDefault(p => p.Name == n)).Where(n => n != null);
                    foreach (var appPool in selectedPools)
                    {
                        if (appPool.State != ObjectState.Stopped && appPool.State != ObjectState.Stopping && appPool.State != ObjectState.Starting)
                        {
                            try
                            {
                                appPool.Recycle();
                            }
                            catch (Exception ex)
                            {
                                logger.LogError($"Failed to recycle an application pool {appPool.Name} {ex}");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError($"Failed to recycle selected application pools {ex}");
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
