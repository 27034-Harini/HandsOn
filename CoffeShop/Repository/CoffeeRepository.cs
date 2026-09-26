using CoffeeShop.Model;
using CoffeeShop.Utils;
using System.Collections.Concurrent;
using System.Globalization;
using System.Text.Json;
namespace CoffeeShop.Repository
{
    internal class CoffeeRepository
    {
        public List<CoffeeInfo> coffeeInfos = new List<CoffeeInfo>();
        private readonly string _orderHistoryFilePath;
        private readonly string _pendingOrdersFilePath;
        private readonly object _orderHistoryLock = new object();
        private readonly object _pendingOrdersLock = new object();
        public CoffeeRepository(string orderHistoryFilePath, string pendingOrders)
        {
            this._orderHistoryFilePath = orderHistoryFilePath;
            this._pendingOrdersFilePath = pendingOrders;
            this.AddingCoffee();
        }

        public void AddingCoffee()
        {
            coffeeInfos.Add(new CoffeeInfo(Enums.CoffeeTypes.Espresso, 100, TimeSpan.FromSeconds(30)));
            coffeeInfos.Add(new CoffeeInfo(Enums.CoffeeTypes.Americano, 100, TimeSpan.FromSeconds(50)));
            coffeeInfos.Add(new CoffeeInfo(Enums.CoffeeTypes.Latte, 100, TimeSpan.FromSeconds(80)));
            coffeeInfos.Add(new CoffeeInfo(Enums.CoffeeTypes.Mocha, 100, TimeSpan.FromSeconds(60)));
            coffeeInfos.Add(new CoffeeInfo(Enums.CoffeeTypes.Cappuccino, 100, TimeSpan.FromSeconds(100)));
        }

        internal void AddOrder(OrderInfo orderInfo)
        {
            lock (_orderHistoryLock)
            {
                List<OrderInfo> orderHistory = this.GetOrders(_orderHistoryFilePath);
                orderHistory.Add(orderInfo);
                this.SaveOrders(orderHistory, _orderHistoryFilePath);
            }

            lock (_pendingOrdersLock)
            {
                List<OrderInfo> pendingOrders = this.GetOrders(_pendingOrdersFilePath);
                pendingOrders.Add(orderInfo);
                this.SaveOrders(pendingOrders, _pendingOrdersFilePath);
            }
        }

        internal List<CoffeeInfo> GetMenu()
        {
            return coffeeInfos;
        }

        internal ConcurrentQueue<OrderInfo> GetMyPendingOrders()
        {
            List<OrderInfo> pendingOrdersList = this.GetOrders(_pendingOrdersFilePath);
            List<OrderInfo> pendingOrders = pendingOrdersList.Where(order => order.UserId == CurrentSession.CurrentUser.UserId).ToList();
            return new ConcurrentQueue<OrderInfo>(pendingOrders);
        }
        internal ConcurrentQueue<OrderInfo> GetAllPendingOrders()
        {
            List<OrderInfo> pendingOrdersList = this.GetOrders(_pendingOrdersFilePath);
            return new ConcurrentQueue<OrderInfo>(pendingOrdersList);
        }

        internal List<OrderInfo> GetMyOrders()
        {
            List<OrderInfo> myOrders = this.GetOrders(_orderHistoryFilePath);
            return myOrders.Where(order => order.UserId == CurrentSession.CurrentUser.UserId).ToList();
        }

        private List<OrderInfo> GetOrders(string ordersFilePath)
        {
            if (!File.Exists(ordersFilePath))
            {
                return new List<OrderInfo>();
            }

            string data = File.ReadAllText(ordersFilePath);
            if (string.IsNullOrWhiteSpace(data))
            {
                return new List<OrderInfo>();
            }

            return JsonSerializer.Deserialize<List<OrderInfo>>(data) ?? new List<OrderInfo>();
        }

        private void SaveOrders(List<OrderInfo> orders, string ordersFilePath)
        {
            string data = JsonSerializer.Serialize(
                orders,
                new JsonSerializerOptions
                {
                    WriteIndented = true,
                });
            File.WriteAllText(ordersFilePath, data);
        }

        internal void SavePendingOrders(List<OrderInfo> orderInfos)
        {
            lock (_pendingOrdersLock)
            {
                this.SaveOrders(orderInfos, _pendingOrdersFilePath);
            }
        }

        internal void UpdatePendingOrder(OrderInfo orderToBeUpdated)
        {
            lock (_pendingOrdersLock)
            {
                List<OrderInfo> totalOrders = this.GetOrders(this._pendingOrdersFilePath);
                int index = totalOrders.FindIndex(order => order.OrderId == orderToBeUpdated.OrderId);
                if (index != -1)
                {
                    totalOrders[index] = orderToBeUpdated;
                    this.SaveOrders(totalOrders, _pendingOrdersFilePath);
                }
            }
        }

        internal void UpdateOrder(OrderInfo orderToBeUpdated)
        {
            lock (_orderHistoryLock)
            {
                List<OrderInfo> totalOrders = this.GetOrders(this._orderHistoryFilePath);
                int index = totalOrders.FindIndex(order => order.OrderId == orderToBeUpdated.OrderId);
                if (index != -1)
                {
                    totalOrders[index] = orderToBeUpdated;
                    this.SaveOrders(totalOrders, _orderHistoryFilePath);
                }
            }
        }

        internal void RemovePendingOrder(OrderInfo orderToBeUpdated)
        {
            lock (_pendingOrdersLock)
            {
                List<OrderInfo> totalOrders = this.GetOrders(this._pendingOrdersFilePath);
                int index = totalOrders.FindIndex(order => order.OrderId == orderToBeUpdated.OrderId);
                if (index != -1)
                {
                    totalOrders.RemoveAt(index);
                    this.SaveOrders(totalOrders, _pendingOrdersFilePath);
                }
            }
        }
    }
}