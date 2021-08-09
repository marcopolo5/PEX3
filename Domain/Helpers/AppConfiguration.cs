﻿using Domain.HelpersContracts;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Domain.Helpers
{
    public class AppConfiguration : IAppConfiguration
    {
        private const string FileName = "applicationsettings.json";
        private const int Key = 220;
        private const string EmptyToken = "0";

        [Serializable]
        class AppSettings
        {
            public string ConnectionString;
            public string EncryptedToken;
        }
        public AppConfiguration()
        {
            var appSettings = GetAppSettings();
            if (appSettings.EncryptedToken == "")
                SetTokenToZero(appSettings);
        }

        public string GetConnectionString()
        {
            var appSettings = GetAppSettings();
            return appSettings.ConnectionString;
        }


        public string GetToken()
        {
            var appSettings = GetAppSettings();
            var decrypted = EncryptDecrypt(appSettings.EncryptedToken);
            return decrypted;
        }

        /// <summary>
        /// Reset the token from the json file by setting it to "0"
        /// </summary>
        /// <returns>True if the token was reset, false if the token was already set to "0"</returns>
        public bool ResetToken()
        {
            var appSettings = GetAppSettings();
            var token = EncryptDecrypt(appSettings.EncryptedToken);
            if (token == EmptyToken)
            {
                return false;
            }
            SetTokenToZero(appSettings);
            return true;
        }

        /// <summary>
        /// Save the token to the json file
        /// </summary>
        /// <param name="newToken">token</param>
        /// <returns>True if token was saved, false if there was already a saved token stored in the file</returns>
        public bool SaveToken(string newToken)
        {
            var appSettings = GetAppSettings();
            var lastToken = EncryptDecrypt(appSettings.EncryptedToken);

            // check if a token is already stored
            if (lastToken != EmptyToken)
            {
                return false;
            }
            // if not encrypt the new token and save it
            var encryptedToken = EncryptDecrypt(newToken);
            appSettings.EncryptedToken = encryptedToken;
            SetAppSettings(appSettings);
            return true;
        }

        /// <summary>
        /// Helper method that encrypts the value "0" and stores it into the file
        /// </summary>
        /// <param name="appSettings">Settings that need to be saved</param>
        private void SetTokenToZero(AppSettings appSettings)
        {
            var encryptedToken = EncryptDecrypt(EmptyToken);
            appSettings.EncryptedToken = encryptedToken;
            SetAppSettings(appSettings);
        }

        /// <summary>
        /// Gets the information from the json file and creates an AppSettings object
        /// </summary>
        /// <returns>The AppSettings</returns>
        private AppSettings GetAppSettings()
        {
            var rawJson = File.ReadAllText(FileName);
            var result = JsonConvert.DeserializeObject<AppSettings>(rawJson);
            return result;
        }

        /// <summary>
        /// Used to update AppSettings stored in the json file
        /// </summary>
        /// <param name="appSettings">The new application settings</param>
        private void SetAppSettings(AppSettings appSettings)
        {
            var rawJson = JsonConvert.SerializeObject(appSettings);
            File.WriteAllText(FileName, rawJson);
        }


        /// <summary>
        /// Simple method used to encrypt and decrypt files.
        /// OBS: maybe change this to something more secure
        /// </summary>
        /// <param name="szPlainText">Text that needs to be encrypted/decrypted</param>
        /// <returns></returns>
        private string EncryptDecrypt(string szPlainText)
        {
            StringBuilder szInputStringBuild = new StringBuilder(szPlainText);
            StringBuilder szOutStringBuild = new StringBuilder(szPlainText.Length);
            char Textch;
            for (int iCount = 0; iCount < szPlainText.Length; iCount++)
            {
                Textch = szInputStringBuild[iCount];
                Textch = (char)(Textch ^ Key);
                szOutStringBuild.Append(Textch);
            }
            return szOutStringBuild.ToString();
        }


    }
}