using ChatServerModule.TokenValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatServerModule.Mocks
{
    public class FakeTokenValidator : ITokenValidator
    {
        public bool IsValid(int userId, string token)
        {
            Console.WriteLine($"user: {userId}, token: {token}");
            return true;
        }
    }
}
