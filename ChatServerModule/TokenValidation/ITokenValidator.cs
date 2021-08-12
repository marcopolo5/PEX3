using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatServerModule.TokenValidation
{
    public interface ITokenValidator
    {
        /// <summary>
        /// Checks the token
        /// </summary>
        /// <param name="userId">User's Id</param>
        /// <param name="token">Token</param>
        /// <returns>True if token is valid, false otherwise</returns>
        bool IsValid(int userId, string token);
    }
}
