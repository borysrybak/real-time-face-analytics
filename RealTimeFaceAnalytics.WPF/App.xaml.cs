using System.Windows;

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
            Core.Properties.Settings.Default.Save();
        }
    }
}
