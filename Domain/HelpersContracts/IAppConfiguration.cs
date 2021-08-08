using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.HelpersContracts
{
    public interface IAppConfiguration
    {
        /// <summary>
        /// Returns the conn string from appsettings.json
        /// </summary>
        /// <returns>DB connection string</returns>
        string GetConnectionString();


        string GetToken();
        bool SaveToken(string token);
        bool ResetToken();
    }
}
