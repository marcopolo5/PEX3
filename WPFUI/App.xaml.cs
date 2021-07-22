using AccountModule.Controllers;
using Domain;
using Domain.AccountContracts;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace WPFUI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        // Dependency Injection Provider:
        private readonly ServiceProvider _serviceProvider;

        public App()
        {
            // setting up the dependency injection:
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            _serviceProvider = serviceCollection.BuildServiceProvider();
        }

        /// <summary>
        /// You can add new dependencies in this method
        /// </summary>
        /// <param name="services">Use this argument to add the dependencies</param>
        private void ConfigureServices(IServiceCollection services)
        {
            // Adding the main window as a singleton
            services.AddSingleton<MainWindow>();
            services.AddScoped<IAccountService, ApplicationUserController>();
        }


        /// <summary>
        /// This is executed when the app is started.
        /// This method receives the MainWindow object from the dependency container and displays it
        /// </summary>
        private void OnStartup(object sender, StartupEventArgs e)
        {
            var mainWindow = _serviceProvider.GetService<MainWindow>();
            mainWindow.Show();
        }
    }
}
