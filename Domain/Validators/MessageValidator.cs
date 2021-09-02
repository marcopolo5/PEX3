using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Domain.Exceptions;
using Domain.Models;
using Domain.ValidatorsContracts;

namespace Domain.Validators
{
    public class MessageValidator : IValidator<Models.Message>
    {
        public static void Validate(Message message)
        {
            StringBuilder exceptionMessage = new();

            if (!Regex.IsMatch(message.TextMessage, @"/^[\x00-\x7F]+$/"))
            {
                exceptionMessage.Append("Message content cannot contain non-ASCII characters. ");
            }

            if (message.TextMessage.Length > 1024)
            {
                exceptionMessage.Append("Message content cannot be longer than 1024 characters. ");
            }

            if (exceptionMessage.Length != 0)
            {
                throw new InvalidEntityException(exceptionMessage.ToString());
            }
        }
    }
}