using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.HelpersContracts
{
    public interface ITokenFileSaver
    {
        bool IsTokenSaved { get; }
        bool SaveToken(string token);
        void DeleteToken();
        string GetToken(string token);  
    }
}
