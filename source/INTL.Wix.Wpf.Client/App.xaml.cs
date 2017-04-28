using System;
using System.Configuration;
using System.Diagnostics;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.HockeyApp;
using Microsoft.HockeyApp.Model;

namespace INTL.Wix.Wpf.Client
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            OnStartupAsync(e).ConfigureAwait(false);
        }

        protected async Task OnStartupAsync(StartupEventArgs e)
        {
            base.OnStartup(e);

            #region Task

            TaskScheduler.UnobservedTaskException += (object sender, UnobservedTaskExceptionEventArgs eventArgs) =>
            {
                eventArgs.SetObserved();
                eventArgs.Exception.Handle(ex =>
                {
                    Debug.WriteLine("Exception type: {0} {1}", ex.GetType(), ex.Message);
                    return true;
                });
            };
            
            #endregion

            #region HOCKEYAPP

            var hockeyAppAppId = ConfigurationManager.AppSettings["hockeyAppAppId"];
            var hockeyAppVersion = ConfigurationManager.AppSettings["hockeyAppVersion"];

            HockeyClient.Current.Configure(hockeyAppAppId);

            var hockeyClient = (HockeyClient)HockeyClient.Current;
            var platformHelper = (HockeyPlatformHelperWPF)hockeyClient.PlatformHelper;
            platformHelper.AppVersion = hockeyAppVersion;

            await HockeyClient.Current.SendCrashesAsync(true);

            var helper = new HockeyPlatformHelperWPF();
            var crashLogInfo = new CrashLogInformation()
            {
                PackageName = helper.AppPackageName,
                OperatingSystem = helper.OSPlatform,
                Windows = hockeyClient.OsVersion,
                Manufacturer = helper.Manufacturer,
                Model = helper.Model,
                ProductID = helper.ProductID,
                Version = Assembly.GetExecutingAssembly().GetName().Version.ToString()
            };

            var field = typeof(HockeyClient).GetField("_crashLogInfo", BindingFlags.NonPublic | BindingFlags.Instance);

            field?.SetValue(hockeyClient, crashLogInfo);

            await HockeyClient.Current.CheckForUpdatesAsync(false, () =>
            {
                Current.MainWindow?.Close();
                return true;
            });

            #endregion
        }
    }
}
