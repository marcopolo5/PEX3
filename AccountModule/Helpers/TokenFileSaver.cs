using Domain.HelpersContracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountModule.Helpers
{
    public class TokenFileSaver : ITokenFileSaver
    {
        //TODO
        public bool IsTokenSaved { get; private set; }
        private readonly IAppConfiguration _appConfiguration;

        public TokenFileSaver(IAppConfiguration appConfiguration)
        {
            _appConfiguration = appConfiguration;
            var token = File.ReadAllText(_appConfiguration.GetTokenFileLocation());
            if (token == "0")
            {
                IsTokenSaved = false;
            }
            else
            {
                IsTokenSaved = true;
            }
        }

        public void DeleteToken()
        {
            File.WriteAllText(_appConfiguration.GetTokenFileLocation(), "0");
            IsTokenSaved = false;
        }

        public string GetToken()
        {
            var result = File.ReadAllText(_appConfiguration.GetTokenFileLocation());
            return result;
        }

        public bool SaveToken(string token)
        {
            var path = _appConfiguration.GetTokenFileLocation();
            if (IsTokenSaved == true ||
                token ==  "0" ||
                File.ReadAllText(path) != "0")
            {
                return false;
            }

            File.WriteAllText(path, token);
            IsTokenSaved = true;
            return true;
        }
    }
}
