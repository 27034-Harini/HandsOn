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
        ConcurrentQueue<OrderInfo> pendingOrders;

        public OrderService(Validator validator, CoffeeRepository coffeeRepository)
        {
            this._validator = validator;
            this._coffeeRepository = coffeeRepository;
            pendingOrders = this._coffeeRepository.GetAllPendingOrders();
        }

        internal List<CoffeeInfo> GetMenu()
        {
            return this._coffeeRepository.GetMenu();
        }

        internal ConcurrentQueue<OrderInfo> GetMyOrders()
        {
            return this._coffeeRepository.GetMyPendingOrders();
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
            OrderInfo newOrder = new OrderInfo(coffeeFound, CurrentSession.CurrentUser.UserId, Guid.NewGuid(), quantity, totalAmount, OrderStatus.Ordered, DateTime.Now);
            this._coffeeRepository.AddOrder(newOrder);
            pendingOrders.Enqueue(newOrder);
        }

        public void ProcessOrders(MachineInfo machine)
        {
            while (true)
            {
                if (machine.MachineStatus == MachineStatus.Busy)
                {
                    Thread.Sleep(1000);
                    continue;
                }
                if (pendingOrders.TryDequeue(out OrderInfo? order))
                {
                    this._coffeeRepository.SavePendingOrders(pendingOrders.ToList());
                    ProcessOrder(order, machine);
                }
                Thread.Sleep(1000);
            }
        }

        private void ProcessOrder(OrderInfo order, MachineInfo machine)
        {
            machine.MachineStatus = MachineStatus.Busy;
            machine.OrderId = order.OrderId;
            order.OrderStatus = OrderStatus.Preparing;
            this._coffeeRepository.UpdateOrder(order);
            this._coffeeRepository.UpdatePendingOrder(order);
            Console.WriteLine($"Processing the order: {order.OrderId}");
            Thread.Sleep(order.CoffeeInfo.TimeTaken);
            order.OrderStatus = OrderStatus.Ready;
            this._coffeeRepository.RemovePendingOrder(order);
            this._coffeeRepository.UpdateOrder(order);
            machine.OrderId = Guid.Empty;
            machine.MachineStatus = MachineStatus.Available;
            Console.WriteLine($"Coffee no {order.OrderId} is ready!!");
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
