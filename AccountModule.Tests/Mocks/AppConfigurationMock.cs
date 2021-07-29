using Domain.HelpersContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountModule.Tests.Mocks
{
    public class AppConfigurationMock : IAppConfiguration
    {
        public string GetConnectionString()
        {
            return "";
        }

        public string GetTokenFileLocation()
        {
            return "token.txt";
        }
    }
}
