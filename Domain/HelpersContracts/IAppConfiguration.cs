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

        /// <summary>
        /// Returns token from file
        /// </summary>
        /// <returns>The token</returns>
        string GetToken();

        /// <summary>
        /// Saves the token to a file
        /// </summary>
        /// <param name="token">Token that needs to be saved</param>
        /// <returns>True if token was saved, false otherwise</returns>
        bool SaveToken(string token);

        /// <summary>
        /// Reset the token from the json file by setting it to "0"
        /// </summary>
        /// <returns>True if the token was reset, false if the token was already set to "0"</returns>
        bool ResetToken();

        /// <summary>
        /// Saves the id to a file
        /// </summary>
        /// <param name="id">User's id</param>
        /// <returns>True if id was saved, false otherwise</returns>
        bool SaveId(int id);

        /// <summary>
        /// Reset the id from the file by setting it to 0
        /// </summary>
        /// <returns>True if id was removed, false otherwise</returns>
        bool ResetId();

        /// <summary>
        /// Returns user's id from the file
        /// </summary>
        /// <returns>User's id</returns>
        int GetId();

    }
}
