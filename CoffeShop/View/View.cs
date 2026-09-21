using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoffeeShop.Constants;
using CoffeeShop.Enums;
using CoffeeShop.Model;

namespace CoffeeShop.Views
{
    internal class View
    {
        public string DisplayAndGetChoice<T>(string message)
            where T : Enum
        {
            StringBuilder optionsMenu = new StringBuilder();
            foreach (T option in Enum.GetValues(typeof(T)))
            {
                optionsMenu.Append(Convert.ToInt32(option));
                optionsMenu.Append(".");
                foreach (char c in option.ToString())
                {
                    if (char.IsUpper(c))
                    {
                        optionsMenu.Append(" ");
                    }
                    optionsMenu.Append(c);
                }

                optionsMenu.Append("\n");
            }
            Console.WriteLine(optionsMenu);
            Console.WriteLine(ConsoleMessages.Separator);
            Console.WriteLine(message);
            Console.WriteLine(ConsoleMessages.Separator);
            return Console.ReadLine() ?? String.Empty;
        }

        internal void DisplayAttemptsLeft(int attempts)
        {
            Console.WriteLine($"Only {attempts} attempts left.");
        }

        internal void DisplayErrorMessage(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        internal void DisplayList(List<CoffeeInfo> coffeeInfo)
        {
            foreach(CoffeeInfo coffee in coffeeInfo)
            {
                Console.WriteLine($"Coffee name : {coffee.CoffeeName}\n" +
                    $"Coffee Price : {coffee.Price}\n");
            }
        }

        internal void DisplaySuccessMessage(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        internal string GetCoffeeDetail(string getCoffeeType)
        {
            throw new NotImplementedException();
        }

        internal string GetUserDetail(string getDetail)
        {
            Console.WriteLine(getDetail);
            return Console.ReadLine() ?? String.Empty.Trim();
        }
    }
}
