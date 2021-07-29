using Domain.HelpersContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountModule.Helpers
{
    public class FakeTokenFileSaver : ITokenFileSaver
    {
        public bool IsTokenSaved { get; private set; }
        private readonly IAppConfiguration _appConfiguration;

        public FakeTokenFileSaver(IAppConfiguration appConfiguration)
        {
            _appConfiguration = appConfiguration;
        }

        public void DeleteToken()
        {
            IsTokenSaved = false;
        }

        public string GetToken()
        {
            var result = "tokenu meu";
            return result;
        }

        public bool SaveToken(string token)
        {
            IsTokenSaved = true;
            return true;
        }

    }
}
