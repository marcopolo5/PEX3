using Domain.HelpersContracts;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AccountModule.Helpers
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

        private void SetTokenToZero(AppSettings appSettings)
        {
            var encryptedToken = EncryptDecrypt(EmptyToken);
            appSettings.EncryptedToken = encryptedToken;
            SetAppSettings(appSettings);
        }

        private AppSettings GetAppSettings()
        {
            var rawJson = File.ReadAllText(FileName);
            var result = JsonConvert.DeserializeObject<AppSettings>(rawJson);
            return result;
        }

        private void SetAppSettings(AppSettings appSettings)
        {
            var rawJson = JsonConvert.SerializeObject(appSettings);
            File.WriteAllText(FileName, rawJson);
        }

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
