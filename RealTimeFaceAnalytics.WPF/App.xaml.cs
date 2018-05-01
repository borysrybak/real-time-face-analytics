using System.Windows;
using RealTimeFaceAnalytics.Core.Properties;

namespace RealTimeFaceAnalytics.WPF
{
    public partial class App
    {
        public App()
        {
            InitializeComponent();
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            Settings.Default.Save();
        }
    }
}