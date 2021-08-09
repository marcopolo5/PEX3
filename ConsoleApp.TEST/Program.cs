using ChatModule;
using Domain.ChatContracts;
using Domain.Models;
using System;
using System.Threading.Tasks;

namespace ConsoleApp.TEST
{
    public class Program
    {
        private static TextChat chat;
        public static void Main(string[] args)
        {
            MainAsync().GetAwaiter().GetResult();
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
        static async Task StartChat(int userId)
        {
            chat = new TextChat();
            await chat.InitializeConnectionAsync(userId, (new Guid()).ToString());
            chat.MessageReceived += MessageReceived;
        }
    }
}
