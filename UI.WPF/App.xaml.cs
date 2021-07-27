using Domain.AccountContracts;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
namespace MainWindoww
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            // initializing dependency injection helper
            DependencyInjectionHelper.Initialize();
        }

        /// <summary>
        /// This is executed when the app is started.
        /// This method receives the MainWindow object from the dependency container and displays it
        /// </summary>
        private void OnStartup(object sender, StartupEventArgs e)
        {
            var mainWindow = DependencyInjectionHelper.ServiceProvider.GetService<MainWindow>();
            mainWindow.Show();
        }

    }
}
