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
        public string GetConnectionString()
        {
            throw new NotImplementedException();
        }

        public string GetToken()
        {
            throw new NotImplementedException();
        }

        public bool RemoveToken()
        {
            throw new NotImplementedException();
        }

        public bool SaveToken()
        {
            throw new NotImplementedException();
        }
    }
}
