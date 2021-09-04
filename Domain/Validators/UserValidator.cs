using System.Text;
using System.Text.RegularExpressions;
using Domain.Exceptions;
using Domain.Models;
using Domain.ValidatorsContracts;

namespace Domain.Validators
{
    public class UserValidator : IValidator<User>
    {
        public static void Validate(UserRegisterModel user)
        {
            StringBuilder exceptionMessage = new();

            //Firstname
            if (!Regex.IsMatch(user.FirstName, @"^[a-zA-Z]+$"))
            {
                exceptionMessage.Append("First name should contain only letters. ");
            }

            if (user.FirstName.Length > 20)
            {
                exceptionMessage.Append("First name cannot be longer than 20 characters. ");
            }


            //Lastname
            if (!Regex.IsMatch(user.LastName, @"^[a-zA-Z]+$"))
            {
                exceptionMessage.Append("Last name should contain only letters. ");
            }

            if (user.LastName.Length > 20)
            {
                exceptionMessage.Append("Last name cannot be longer than 20 characters. ");
            }


            //Password
            if (!Regex.IsMatch(user.Password,
                @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@#$!%*?&])[A-Za-z\d@#$!%*?&]{8,}$"))
            {
                exceptionMessage.Append(
                    "Password must contain minimum eight characters, at least one uppercase letter, one lowercase letter, one number and one special character. ");
            }

            if (user.Password is {Length: > 255})
            {
                exceptionMessage.Append("Password cannot be longer than 255 characters. ");
            }


            //Email
            if (!Regex.IsMatch(user.Email,
                @"^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$"))
            {
                exceptionMessage.Append("Invalid email address. ");
            }

            if (user.Email.Length > 255)
            {
                exceptionMessage.Append("Email cannot be longer than 255 characters");
            }

            if (exceptionMessage.Length != 0)
            {
                throw new InvalidEntityException(exceptionMessage.ToString());
            }
        }

        public static void ValidateLogin(string email, string password)
        {
            StringBuilder exceptionMessage = new();

            if (email.Length == 0)
            {
                exceptionMessage.Append("Enter email address. ");
            }

            if (password is {Length: 0})
            {
                exceptionMessage.Append("Enter password. ");
            }

            if (exceptionMessage.Length != 0)
            {
                throw new InvalidEntityException(exceptionMessage.ToString());
            }
        }

        public static void ValidatePasswords(string password, string retypedPassword)
        {
            StringBuilder exceptionMessage = new();

            if (password is null && retypedPassword is null) //Third-party authentication
            {
                return;
            }

            if (password.Length == 0 || retypedPassword.Length == 0)
            {
                exceptionMessage.Append("The password cannot be empty");
            }
            else if (password != retypedPassword)
            {
                exceptionMessage.Append("Passwords don't match");
            }
            else if (!Regex.IsMatch(password, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@#$!%*?&])[A-Za-z\d@#$!%*?&]{8,}$")
            )
            {
                exceptionMessage.Append(
                    "Password must contain minimum eight characters, at least one uppercase letter, one lowercase letter, one number and one special character");
            }

            if (exceptionMessage.Length != 0)
            {
                throw new InvalidEntityException(exceptionMessage.ToString());
            }
        }
    }
}