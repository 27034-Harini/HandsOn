

using System.Text.RegularExpressions;
using CoffeeShop.Enums;

namespace CoffeeShop.Helper
{
    internal class Validator
    {
        internal bool ValidateCoffeeQuantity(string coffeeQuantity, out int quantity)
        {
            return int.TryParse(coffeeQuantity, out quantity);
        }

        internal bool ValidateCoffeeType(string coffeeName, out CoffeeTypes coffeeType)
        {
            return Enum.TryParse(coffeeName, out coffeeType) && Enum.IsDefined(coffeeType);
        }

        internal bool ValidateEmail(string email)
        {
            return Regex.IsMatch(email, @"[a-zA-Z0-9_-]+@[a-zA-Z_]+\.[a-zA-Z]{2,}");
        }

        internal bool ValidateName(string userName)
        {
            return !string.IsNullOrWhiteSpace(userName);
        }

        internal bool ValidatePassword(string password)
        {
            return !string.IsNullOrWhiteSpace(password) && password.Length >= 8;
        }

        internal bool ValidatePhoneNumber(string phoneNumber)
        {
            return !string.IsNullOrWhiteSpace(phoneNumber) && phoneNumber.Length == 10;
        }
    }
}
