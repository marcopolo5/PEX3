using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.AccountContracts
{
    public interface ITokenFileSaver
    {
        bool SaveToken(string token);
        bool DeleteToken();
        string GetToken(string token);
    }
}
