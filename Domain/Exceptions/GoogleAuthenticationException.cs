using System;

namespace Domain.Exceptions
{
    public class GoogleAuthenticationException : Exception
    {
        public GoogleAuthenticationException() { }

        public GoogleAuthenticationException(string message) : base(message)  { }

        public GoogleAuthenticationException(string message, Exception inner) : base(message, inner) { }
    }
}
