using System.Text;
using System.Text.RegularExpressions;
using Domain.Exceptions;
using Domain.Models;
using Domain.ValidatorsContracts;

namespace Domain.Validators
{
    public class ConversationValidator : IValidator<Models.Conversation>
    {
        public static void Validate(Conversation conversation)
        {
            StringBuilder exceptionMessage = new();
            
            if (!Regex.IsMatch(conversation.Title, @"^[A-Za-z0-9 _.,!'/$]*$"))
            {
                exceptionMessage.Append("Conversation title cannot contain non-ASCII characters. ");
            }

            if (conversation.Title.Length > 20)
            {
                exceptionMessage.Append("Conversation title cannot be longer than 20 characters. ");
            }

            if (conversation.Title.Length == 0)
            {
                exceptionMessage.Append("Conversation title cannot be empty. ");
            }

            if (exceptionMessage.Length != 0)
            {
                throw new InvalidEntityException(exceptionMessage.ToString());
            }
        }
    }
}