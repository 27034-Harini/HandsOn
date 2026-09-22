using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoffeeShop.Enums;
using CoffeeShop.Helper;
using CoffeeShop.Model;
using CoffeeShop.Repository;
using CoffeeShop.Utils;
using System.Collections.Concurrent;

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

        internal ConcurrentQueue<OrderInfo> GetMyOrders()
        {
            return this._coffeeRepository.GetMyOrders();
        }

        internal void PlaceOrder(CoffeeTypes coffeeType, int quantity)
        {
            List<CoffeeInfo> coffeeInfos = this.GetMenu();
            CoffeeInfo? coffeeFound = coffeeInfos.FirstOrDefault(coffee => coffee.CoffeeName.Equals(coffeeType));
            if (coffeeFound == null)
            {
                return;
            }
            decimal totalAmount = coffeeFound.Price * quantity;
            this._coffeeRepository.AddOrder(new OrderInfo(coffeeFound, CurrentSession.CurrentUser.UserId, Guid.NewGuid(), quantity, totalAmount, OrderStatus.Ordered, DateTime.Now));
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
