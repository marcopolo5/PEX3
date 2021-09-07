using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Diagnostics;
using Domain.Exceptions;

namespace AccountModule.Controllers
{
    public class GoogleAuthenticatorController
    {
        private const string clientID = "502643571231-s4fkb65mkpr2288cprsk373ka3sel74s.apps.googleusercontent.com";
        private const string clientSecret = "94F6AnVlzd7aG0Dn5xSlKRFE";
        private const string authorizationEndpoint = "https://accounts.google.com/o/oauth2/v2/auth";


        /// <summary>
        /// Searches for an unopened port.
        /// </summary>
        /// <returns>Unopened port</returns>
        private static int GetAvailablePort()
        {
            var listener = new TcpListener(IPAddress.Loopback, 0);
            listener.Start();
            var port = ((IPEndPoint)listener.LocalEndpoint).Port;
            listener.Stop();
            return port;
        }


        /// <summary>
        /// Performs and validates OAuth flow for a Google account.
        /// Scopes: profile, email
        /// </summary>
        /// <returns>Dictionary containing user's Google email address, first name and last name</returns>
        public async Task<Dictionary<string,string>> GetGoogleAccountInfo()
        {
            var state = RandomDataBase64url(32);
            var code_verifier = RandomDataBase64url(32);
            var code_challenge = Base64urlencodeNoPadding(SHA256(code_verifier));
            const string code_challenge_method = "S256";

            var redirectURI = string.Format("http://{0}:{1}/", IPAddress.Loopback, GetAvailablePort());

            var http = new HttpListener();
            http.Prefixes.Add(redirectURI);
            http.Start();

            var authorizationRequest = string.Format("{0}?response_type=code&scope=openid%20profile%20email&redirect_uri={1}&client_id={2}&state={3}&code_challenge={4}&code_challenge_method={5}",
                authorizationEndpoint,
                System.Uri.EscapeDataString(redirectURI),
                clientID,
                state,
                code_challenge,
                code_challenge_method);

            var psi = new ProcessStartInfo
            {
                FileName = authorizationRequest,
                UseShellExecute = true
            };
            Process.Start(psi);

            var context = await http.GetContextAsync();
            var response = context.Response;
            var responseString = string.Format("<html><head><meta http-equiv='refresh' content='10;url=https://google.com'></head><body>Please return to the app.</body></html>");
            var buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
            response.ContentLength64 = buffer.Length;
            var responseOutput = response.OutputStream;
            
            var responseTask = responseOutput.WriteAsync(buffer, 0, buffer.Length).ContinueWith((task) =>
            {
                responseOutput.Close();
                http.Stop();
            });

            if (context.Request.QueryString.Get("error") != null)
            {
                throw new GoogleAuthenticationException($"OAuth authorization error {context.Request.QueryString.Get("error")}");
            }
            if (context.Request.QueryString.Get("code") == null || context.Request.QueryString.Get("state") == null)
            {
                throw new GoogleAuthenticationException($"Malformed authorization response. {context.Request.QueryString}");
            }

            var code = context.Request.QueryString.Get("code");
            var incoming_state = context.Request.QueryString.Get("state");

            if (incoming_state != state)
            {
                throw new GoogleAuthenticationException($"Received request with invalid state. Expected {state} but received {incoming_state}");
            }

            return await PerformCodeExchange(code, code_verifier, redirectURI);
        }


        /// <summary>
        /// Performs code exchange between the application and GoogleAPIs. Continues OAuth flow.
        /// </summary>
        /// <returns>Dictionary containing user's Google email address, first name and last name</returns>
        private async Task<Dictionary<string,string>> PerformCodeExchange(string code, string code_verifier, string redirectURI)
        {

            var tokenRequestURI = "https://www.googleapis.com/oauth2/v4/token";
            var tokenRequestBody = string.Format("code={0}&redirect_uri={1}&client_id={2}&code_verifier={3}&client_secret={4}&scope=&grant_type=authorization_code",
                code,
                System.Uri.EscapeDataString(redirectURI),
                clientID,
                code_verifier,
                clientSecret
                );

            var tokenRequest = (HttpWebRequest)WebRequest.Create(tokenRequestURI);
            tokenRequest.Method = "POST";
            tokenRequest.ContentType = "application/x-www-form-urlencoded";
            tokenRequest.Accept = "Accept=text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            var _byteVersion = Encoding.ASCII.GetBytes(tokenRequestBody);
            tokenRequest.ContentLength = _byteVersion.Length;
            var stream = tokenRequest.GetRequestStream();
            await stream.WriteAsync(_byteVersion, 0, _byteVersion.Length);
            stream.Close();

            try
            {
                var tokenResponse = await tokenRequest.GetResponseAsync();
                using (var reader = new StreamReader(tokenResponse.GetResponseStream()))
                {
                    var responseText = await reader.ReadToEndAsync();

                    var tokenEndpointDecoded = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseText);

                    var access_token = tokenEndpointDecoded["access_token"];
                    return await UserinfoCall(access_token);
                }
            }
            catch (WebException ex)
            {
                if (ex.Status == WebExceptionStatus.ProtocolError)
                {
                    var response = ex.Response as HttpWebResponse;
                    if (response != null)
                    {
                        using (var reader = new StreamReader(response.GetResponseStream()))
                        {
                            var responseText = await reader.ReadToEndAsync();
                        }
                    }
                }
                return null;
            }
        }


        /// <summary>
        /// Sends a GET method to receive user informations declared in the scope.
        /// </summary>
        /// <returns>Dictionary containing user's Google email address, first name and last name</returns>
        private async Task<Dictionary<string, string>> UserinfoCall(string access_token)
        {

            var userinfoRequestURI = "https://openidconnect.googleapis.com/v1/userinfo";

            var userinfoRequest = (HttpWebRequest)WebRequest.Create(userinfoRequestURI);
            userinfoRequest.Method = "GET";
            userinfoRequest.Headers.Add(string.Format("Authorization: Bearer {0}", access_token));
            userinfoRequest.ContentType = "application/x-www-form-urlencoded";
            userinfoRequest.Accept = "Accept=text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";

            var userinfoResponse = await userinfoRequest.GetResponseAsync();
            using (var userinfoResponseReader = new StreamReader(userinfoResponse.GetResponseStream()))
            {
                var userinfoResponseText = await userinfoResponseReader.ReadToEndAsync();
                var userInfo = JsonConvert.DeserializeObject<Dictionary<string, string>>(userinfoResponseText);
                return userInfo;
            }
        }


        /// <summary>
        /// Returns URI-safe data with a given input length.
        /// </summary>
        /// <param name="length">Input length (nb. output will be longer)</param>
        private static string RandomDataBase64url(uint length)
        {
            var rng = new RNGCryptoServiceProvider();
            var bytes = new byte[length];
            rng.GetBytes(bytes);
            return Base64urlencodeNoPadding(bytes);
        }


        /// <summary>
        /// Returns the SHA256 hash of the input string.
        /// </summary>
        /// <param name="inputStirng"></param>
        private static byte[] SHA256(string inputStirng)
        {
            var bytes = Encoding.ASCII.GetBytes(inputStirng);
            var sha256 = new SHA256Managed();
            return sha256.ComputeHash(bytes);
        }


        /// <summary>
        /// Base64url no-padding encodes the given input buffer.
        /// </summary>
        /// <param name="buffer"></param>
        private static string Base64urlencodeNoPadding(byte[] buffer)
        {
            var base64 = Convert.ToBase64String(buffer);

            base64 = base64.Replace("+", "-");
            base64 = base64.Replace("/", "_");
            base64 = base64.Replace("=", "");

            return base64;
        }
    }
}
