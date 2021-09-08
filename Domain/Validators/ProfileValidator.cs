using System.Text;
using System.Text.RegularExpressions;
using Domain.Exceptions;
using Domain.Models;
using Domain.ValidatorsContracts;

namespace Domain.Validators
{
    public class ProfileValidator : IValidator<Models.Profile>
    {
        public static void Validate(Profile profile)
        {
            StringBuilder exceptionMessage = new();

            if (!Regex.IsMatch(profile.DisplayName, @"^[A-Za-z0-9 _.,!'/$]*$"))
            {
                exceptionMessage.Append("Display name cannot contain non-ASCII characters. ");
            }

            if (profile.DisplayName.Length > 255)
            {
                exceptionMessage.Append("Display name cannot be longer than 255 characters. ");
            }

            if (profile.DisplayName.Length == 0)
            {
                exceptionMessage.Append("Display name cannot be empty. ");
            }
            
            if (!Regex.IsMatch(profile.StatusMessage, @"^[A-Za-z0-9 _.,!'/$]*$"))
            {
                exceptionMessage.Append("About cannot contain non-ASCII characters. ");
            }

            if (profile.StatusMessage.Length > 1024)
            {
                exceptionMessage.Append("About cannot be longer than 1024 characters. ");
            }

            if (exceptionMessage.Length != 0)
            {
                throw new InvalidEntityException(exceptionMessage.ToString());
            }
        }
    }
}