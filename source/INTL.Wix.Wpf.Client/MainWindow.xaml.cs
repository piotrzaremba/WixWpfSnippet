using System;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.HockeyApp;

namespace INTL.Wix.Wpf.Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
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
            await HockeyClient.Current.CheckForUpdatesAsync(true, () => true);
        }
    }
}
