using AccountModule.Controllers;
using Domain.AccountContracts;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace MainWindoww
{
    public static class DependencyInjectionHelper
    {
        public static IServiceProvider ServiceProvider;

        public static void Initialize()
        {
            // check if service provider wasnt already initialized
            if (ServiceProvider != null)
            {
                throw new Exception("DependencyInjectionHelper was already initialized.");
            }

            // setting up the dependency injection:
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            ServiceProvider = serviceCollection.BuildServiceProvider();
        }

        /// <summary>
        /// You can add new dependencies in this method
        /// </summary>
        /// <param name="services">Use this argument to add the dependencies</param>
        private static void ConfigureServices(IServiceCollection services)
        {

            // Adding the main window as a singleton
            services.AddSingleton<MainWindow>();

            // Adding  ApplicationUserController as scoped
            services.AddScoped<IAccountService, ApplicationUserController>();

        }
    }
}
