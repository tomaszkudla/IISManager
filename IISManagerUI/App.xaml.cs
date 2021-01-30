using IISManager.Implementations;
using IISManager.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace IISManagerUI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
    {
        private readonly ServiceProvider serviceProvider;
        private readonly ILogger<App> logger;

        public App()
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            serviceProvider = serviceCollection.BuildServiceProvider();
            logger = serviceProvider.GetService<ILoggerFactory>().CreateLogger<App>();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<SelfDiagnostics>();
            services.AddSingleton<UserMessage>();
            services.AddSingleton<CurrentProcessWrapper>();
            services.AddSingleton<ProcessDiagnostics>();
            services.AddSingleton<ApplicationPoolsManager>();
            services.AddSingleton<IISServerManager>();
            services.AddSingleton<RefreshingTimer>();
            services.AddSingleton<AppPoolsControl>();
            services.AddSingleton<TopPanel>();
            services.AddSingleton<BottomPanel>();
            services.AddSingleton<MainWindow>();

            var config = new NLog.Config.LoggingConfiguration();
            var logfile = new NLog.Targets.FileTarget("logfile") { FileName = "log.txt" };
            config.AddRule(NLog.LogLevel.Trace, NLog.LogLevel.Fatal, logfile);

            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.ClearProviders();
                loggingBuilder.SetMinimumLevel(LogLevel.Trace);
                loggingBuilder.AddNLog(config);
            });
        }

        private void OnStartup(object sender, StartupEventArgs e)
        {
            SetupExceptionHandling();
            var mainWindow = serviceProvider.GetService<MainWindow>();
            mainWindow.Show();
        }

        private void SetupExceptionHandling()
        {
            AppDomain.CurrentDomain.UnhandledException += (s, e) =>
                logger.LogError((Exception)e.ExceptionObject, "AppDomain.CurrentDomain.UnhandledException");

            DispatcherUnhandledException += (s, e) =>
            {
                logger.LogError(e.Exception, "Application.Current.DispatcherUnhandledException");
                e.Handled = true;
            };

            TaskScheduler.UnobservedTaskException += (s, e) =>
            {
                logger.LogError(e.Exception, "TaskScheduler.UnobservedTaskException");
                e.SetObserved();
            };
        }
    }
}
