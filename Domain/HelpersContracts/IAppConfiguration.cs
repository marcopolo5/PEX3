using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.HelpersContracts
{
    public interface IAppConfiguration
    {
        string GetConnectionString();
        string GetTokenFileLocation();
    }
}
