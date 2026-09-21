using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoffeeShop.Enums;
using CoffeeShop.Helper;
using CoffeeShop.Repository;
using CoffeeShop.Views;
using CoffeeShop.Constants;
using CoffeeShop.Model;
using CoffeeShop.Service;

namespace CoffeeShop.Controller
{
    internal class CoffeeController
    {
        private int _maxNoOfTries = 3;
        private View _view;
        private Validator _validator;
        private OrderService _orderService;

        public CoffeeController(View view, Validator validator, OrderService orderService)
        {
            this._view = view;
            this._validator = validator;
            this._orderService = orderService;
        }

        public void GetMenuChoice()
        {
            bool shallBreak = false;
            while (!shallBreak)
            {
                string option = this._view.DisplayAndGetChoice<MenuOperations>(ConsoleMessages.ChooseOption);
                switch (option)
                {
                    case "1":
                        this.ViewCoffeeMenu();
                        break;
                    case "2":
                        this.PlaceOrder();
                        break;
                    case "3":
                        break;
                    case "4":
                        break;
                    case "5":
                        shallBreak = true;
                        break;
                    default:
                        this._view.DisplayErrorMessage(ConsoleMessages.InvalidInput);
                        break;
                }
            }

        }

        private void PlaceOrder()
        {
            CoffeeTypes coffeeType = 0;
            int quantity = 0;
            bool gotCoffeeType = this.GetCoffeeType(out coffeeType);
            if (!gotCoffeeType)
            {
                return;
            }
            bool gotQuantity = this.GetQuantity(out quantity);
            if (!gotQuantity)
            {
                return;
            }
            this._orderService.PlaceOrder(coffeeType, quantity);
        }

        private bool GetQuantity(out int quantity)
        {
            quantity = 0;
            int coffeeQuantityInputAttempt = this._maxNoOfTries;
            while (coffeeQuantityInputAttempt > 0)
            {
                string coffeeQuantity = this._view.GetCoffeeDetail(ConsoleMessages.GetCoffeeQuantity);
                if (!this._orderService.ValidateQuantity(coffeeQuantity, out quantity))
                {
                    this._view.DisplayErrorMessage(ConsoleMessages.InvalidInput);
                    this._view.DisplayAttemptsLeft(--coffeeQuantityInputAttempt);
                    continue;
                }

                return true;
            }

            this._view.DisplayErrorMessage(ConsoleMessages.MaxAttemptsReached);
            return false;
        }

        private bool GetCoffeeType(out CoffeeTypes coffeeType)
        {
            coffeeType = 0;
            int coffeeTypeInputAttempt = this._maxNoOfTries;
            while (coffeeTypeInputAttempt > 0)
            {
                string coffeeName = this._view.GetCoffeeDetail(ConsoleMessages.GetCoffeeType);
                if (!this._orderService.ValidateCoffeeType(coffeeName, out coffeeType))
                {
                    this._view.DisplayErrorMessage(ConsoleMessages.InvalidInput);
                    this._view.DisplayAttemptsLeft(--coffeeTypeInputAttempt);
                    continue;
                }

                return true;
            }

            this._view.DisplayErrorMessage(ConsoleMessages.MaxAttemptsReached);
            return false;
        }

        private void ViewCoffeeMenu()
        {
            List<CoffeeInfo> coffeeInfo = this._orderService.GetMenu();
            this._view.DisplayList(coffeeInfo);
        }
    }
}
