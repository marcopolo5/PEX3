using Domain;
using Domain.HelpersContracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AccountModule.Helpers
{
    public class FakeAppConfig : IAppConfiguration
    {
        private readonly AppSettings _appSettings;
        public FakeAppConfig()
        {
            _appSettings = new AppSettings() { ConnectionString = " ", FileLocation = "token.txt" };

        }

        public string GetConnectionString()
        {
            return _appSettings.ConnectionString;
        }

        public string GetTokenFileLocation()
        {
            return _appSettings.FileLocation;
        }
    }
}
