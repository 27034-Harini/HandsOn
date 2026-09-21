using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoffeeShop.Enums;
using CoffeeShop.Helper;
using CoffeeShop.Model;
using CoffeeShop.Repository;

namespace CoffeeShop.Service
{
    internal class OrderService
    {
        private Validator _validator;
        private CoffeeRepository _coffeeRepository;

        public OrderService(Validator validator, CoffeeRepository coffeeRepository)
        {
            this._validator = validator;
            this._coffeeRepository = coffeeRepository;
        }

        internal List<CoffeeInfo> GetMenu()
        {
            return this._coffeeRepository.GetMenu();
        }

        internal bool ValidateCoffeeType(string coffeeName, out CoffeeTypes coffeeType)
        {
            return this._validator.ValidateCoffeeType(coffeeName, out coffeeType);
        }

        internal bool ValidateQuantity(string coffeeQuantity, out int quantity)
        {
            return this._validator.ValidateCoffeeQuantity(coffeeQuantity, out quantity);
        }
    }
}
