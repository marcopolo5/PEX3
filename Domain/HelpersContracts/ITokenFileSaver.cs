using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.HelpersContracts
{
    public interface ITokenFileSaver
    {
        /// <summary>
        /// Returns true if there s already a saved token or false if there isnt
        /// </summary>
        bool IsTokenSaved { get; }

        /// <summary>
        /// Saves the current user's token to a file
        /// </summary>
        /// <param name="token">Token that needs to be saved</param>
        /// <returns>True if the token was saved successfuly, false otherwise</returns>
        bool SaveToken(string token);

        /// <summary>
        /// Deletes the token from the file (by overwritting with "0")
        /// </summary>
        void DeleteToken();

        /// <summary>
        /// Returns the token from the file. 
        /// Warning: you should check if the token exists first by using IsTokenSaved
        /// </summary>
        /// <returns>The token from the file</returns>
        string GetToken();  
    }
}
