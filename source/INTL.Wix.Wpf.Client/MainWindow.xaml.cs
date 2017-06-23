using System;
using System.Configuration;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.HockeyApp;
using Microsoft.HockeyApp.Gui;

namespace INTL.Wix.Wpf.Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IAppVersion _appVersion;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            UpdateButtonClickAsync().ConfigureAwait(false);
        }

        private async Task UpdateButtonClickAsync()
        {
           //await HockeyClient.Current.CheckForUpdatesAsync(true, () => true);

            await HockeyClient.Current.CheckForUpdatesAsync(false, () => true, MandatoryUpdateAvailableAction);
        }

        private void MandatoryUpdateAvailableAction(IAppVersion appVersion)
        {
            MandatoryUpdateAvailableActionAsync(appVersion).Wait();
        }

        private static async Task MandatoryUpdateAvailableActionAsync(IAppVersion appVersion)
        {
            await appVersion.DownloadMsi(information => false,appVersion.InstallVersion);
        }
    }
}
