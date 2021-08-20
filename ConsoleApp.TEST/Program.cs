﻿using ChatModule;
using Domain.ChatContracts;
using Domain.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;


namespace ConsoleApp.TEST
{
    public class ApiResult
    {
        public string Ip { get; set; }
        [JsonProperty(PropertyName = "continent_name")]
        public string ContinentName { get; set; }
        [JsonProperty(PropertyName = "country_name")]
        public string CountryName { get; set; }
        [JsonProperty(PropertyName = "region_name")]
        public string RegionName { get; set; }
        public string City { get; set; }
        public string Zip { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public override string ToString()
        {
            return $"Ip: {Ip} | Continent: {ContinentName} | Country: {CountryName} | Region: {RegionName} | City: {City} | Coords: {Latitude} : {Longitude}";
        }

    }
    public class Program
    {

        private static TextChat chat;
        public static void Main(string[] args)
        {
            var result = CallApi();
            Console.WriteLine(result.ToString());
            //MainAsync().GetAwaiter().GetResult();
        }
        private static ApiResult CallApi()
        {
            ApiResult result = null;
            string URL = "http://api.ipstack.com/check";
            string urlParameters = "?access_key=745f0ee9cb257e0329e00017545b6ea0";

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(URL);

            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            
            HttpResponseMessage response = client.GetAsync(urlParameters).Result;  // Blocking call! Program will wait here until a response is received or a timeout occurs.
            if (response.IsSuccessStatusCode)
            {
                // Parse the response body.
                result = response.Content.ReadAsAsync<ApiResult>().Result;  //Make sure to add a reference to System.Net.Http.Formatting.dll
            }
            else
            {
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }
            client.Dispose();
            return result; // only for testing
        }
        
    private static async Task MainAsync()
        {
            var userId = Login();
            await StartChat(userId);
            while (true)
            {
                Console.WriteLine("WannaExit?");
                var ans = Console.ReadLine();
                if (ans.ToLower() == "y")
                    break;

                Console.WriteLine("Enter ur next msg: ");
                var msg = Console.ReadLine();
                await SendMessage(msg);
            }
            Console.WriteLine("conversation ended");

        }

        static int Login()
        {
            Console.WriteLine("UserId:");
            int result = int.Parse(Console.ReadLine());
            return result;
        }
        static async Task SendMessage(string msg)
        {
            //Message message = new()
            //{
            //    SenderId = 1,
            //    TextMessage = msg,
            //    ConversationId = 1,
            //    Id = 1
            //};
            await chat.SendMessageAsync(1, msg);
        }

        static void MessageReceived(Message message)
        {
            Console.WriteLine(message.TextMessage);
        }

        static void StatusChanged(StatusModel status)
        {
            Console.WriteLine(status.FriendId + " " + status.NewStatus);
        }

        static async Task StartChat(int userId)
        {
            chat = new TextChat();
            await chat.InitializeConnectionAsync(userId, (new Guid()).ToString());
            chat.MessageReceived += MessageReceived;
            chat.StatusChanged += StatusChanged;
        }
    }
}
