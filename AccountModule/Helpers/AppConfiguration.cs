using Domain.HelpersContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountModule.Helpers
{
    public class AppConfiguration : IAppConfiguration
    {
        private readonly string _connString = " ";
        private readonly string _tokenLocation = "token.txt";
        public string GetConnectionString()
        {
            throw new NotImplementedException();
        }

        public string GetTokenFileLocation()
        {
            return _tokenLocation;
        }
    }
}
