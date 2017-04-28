using System;
using Microsoft.HockeyApp;

namespace INTL.Wix.Wpf.Client.Extensions
{
    public static class HockeyAppExtension
    {
        public static void TrackExceptionWpf(this IHockeyClient hockeyClient, Exception exception)
        {
            var client = hockeyClient as HockeyClient;
            client?.HandleException(exception);
        }
    }
}