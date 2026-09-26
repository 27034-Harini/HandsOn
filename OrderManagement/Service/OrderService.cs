using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderManagement.Repository;
using OrderManagement.Model;
using OrderManagement.Enums;

namespace OrderManagement.Service
{
    internal class OrderService
    {
        private readonly CancellationTokenSource _cancellationTokenSource;
        private readonly CancellationToken _cancellationToken;
        private ConcurrentQueue<OrderInfo> placedOrders = new();
        private ConcurrentDictionary<string, int> stocks;
        private ConcurrentQueue<OrderInfo> pendingOrders = new();
        public List<string> ReservationLog = new();
        private OrderRepository _orderRepository;
        public OrderService(OrderRepository orderRepository)
        {
            this._cancellationTokenSource = new CancellationTokenSource();
            this._cancellationToken = this._cancellationTokenSource.Token;
            this._orderRepository = orderRepository;
        }
        public bool TryReserve(string productId, int quantity)
        {
            if (stocks[productId] >= quantity)
            {
                Thread.Sleep(50); // simulates database latency
                stocks[productId] -= quantity;
                ReservationLog.Add($"{DateTime.Now}: Reserved {quantity} of {productId}");
                return true;
            }
            return false;
        }

        internal async Task PlaceOrder(string productId, string customerName, int quantity)
        {
            OrderInfo order = new OrderInfo(
                Guid.NewGuid(), 
                customerName, 
                productId, 
                quantity, 
                DateTime.Now, 
                OrderStatus.Null, 
                OrderStatus.Placed, 
                true);
            this.placedOrders.Enqueue(order);
            await this._orderRepository.SavePlacedOrdersAsync(placedOrders);
        }

        private async Task ProcessOrder(OrderInfo order, int workerId)
        {
            await this.ValidateOrder(order);
            if (order.CurrentState == OrderStatus.Reserved)
            {
                await this.ChangeOrderStatus(order, OrderStatus.Packing);
                await this.PackOrder(order);
                await this.ShipOrder(order);
                await this.ChangeOrderStatus(order, OrderStatus.Shipped);
                pendingOrders = new ConcurrentQueue<OrderInfo>(pendingOrders
                    .Where(remainingOrder => remainingOrder.OrderId != order.OrderId));
                await this._orderRepository.SavePendingOrdersAsync(pendingOrders);
            }
        }

        private async Task ShipOrder(OrderInfo order)
        {
            await Task.Delay(10000, _cancellationToken);
        }

        private async Task PackOrder(OrderInfo order)
        {

            await Task.Delay(5000, _cancellationToken);
        }

        private async Task ValidateOrder(OrderInfo order)
        {
            if (order.CurrentState == OrderStatus.Cancelled)
            {
                return;
            }
            await this.ChangeOrderStatus(order, OrderStatus.Validating);
            if (stocks.TryGetValue(order.ProductId, out var stock))
            {
                if (stock >= order.Quantity)
                {
                    stocks[order.ProductId] -= order.Quantity;
                    await this._orderRepository.SaveStockAsync(stocks);
                    pendingOrders.Enqueue(order);
                    await this.ChangeOrderStatus(order, OrderStatus.Reserved);
                }
                else
                {
                    await this.ChangeOrderStatus(order, OrderStatus.Rejected);
                }
            }
            else
            {
                await this.ChangeOrderStatus(order, OrderStatus.Rejected);
            }
        }

        private async Task ChangeOrderStatus(OrderInfo order, OrderStatus updatedStatus)
        {
            lock(order.LockObject)
            {
                order.PreviousState = order.CurrentState;
                order.CurrentState = updatedStatus;
            }
            await this._orderRepository.SavePendingOrdersAsync(pendingOrders);
            await this._orderRepository.SavePlacedOrdersAsync(placedOrders);
        }

        internal void StartWorkers()
        {
            Task.Run(() => WorkerLoop(1));
            Task.Run(() => WorkerLoop(2));
            Task.Run(() => WorkerLoop(3));

        }

        private async Task WorkerLoop(int workerId)
        {
            while (!_cancellationToken.IsCancellationRequested)
            {
                if(placedOrders.TryDequeue(out OrderInfo? order))
                {
                    await this._orderRepository.SavePlacedOrdersAsync(placedOrders);
                    await ProcessOrder(order, workerId);
                }
                else
                {
                    await Task.Delay(100, _cancellationToken);
                }
            }
        }

        internal async Task LoadFiles()
        {
            placedOrders = await this._orderRepository.GetPlacedOrderDetails();
            pendingOrders = await this._orderRepository.GetPendingOrderDetails();
            stocks = await this._orderRepository.GetStockDetails();
        }

        internal async Task CancelOrder(string productId)
        {
            OrderInfo? orderFound = placedOrders
                .FirstOrDefault(order =>
                    order.ProductId == productId &&
                    order.IsCancelable == true);

            if (orderFound == null)
            {
                orderFound = pendingOrders
                    .FirstOrDefault(order =>
                        order.ProductId == productId &&
                        order.IsCancelable == true);
            }

            if (orderFound == null)
            {
                return;
            }

            await ChangeOrderStatus(orderFound, OrderStatus.Cancelled);
        }
    }
}
