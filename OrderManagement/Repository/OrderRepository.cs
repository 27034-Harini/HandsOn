using System;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using OrderManagement.Model;

namespace OrderManagement.Repository
{
    internal class OrderRepository
    {
        private string pendingOrdersFilePath;
        private string placedOrdersFilePath;
        private string loggerFilePath;
        private string stocksFilePath;
        private ConcurrentQueue<OrderInfo> placedOrders;
        private ConcurrentDictionary<string, int> stocks;
        private ConcurrentQueue<OrderInfo> pendingOrders;

        public OrderRepository(string pendingOrdersFilePath, string placedOrdersFilePath, string loggerFilePath, string stocksFilePath)
        {
            this.pendingOrdersFilePath = pendingOrdersFilePath;
            this.placedOrdersFilePath = placedOrdersFilePath;
            this.loggerFilePath = loggerFilePath;
            this.stocksFilePath = stocksFilePath;
        }

        public async Task<ConcurrentDictionary<string,int>> GetStockDetails()
        {
            string stockDetail = await File.ReadAllTextAsync(stocksFilePath);

            return JsonSerializer.Deserialize<ConcurrentDictionary<string, int>>(stockDetail) ?? new();
        }

        public async Task<ConcurrentQueue<OrderInfo>> GetOrderDetails(string filePath)
        {
            string orderDetail = await File.ReadAllTextAsync(filePath);
            return JsonSerializer.Deserialize<ConcurrentQueue<OrderInfo>>(orderDetail) ?? new();
        }

        internal async Task<ConcurrentQueue<OrderInfo>> GetPlacedOrderDetails()
        {
            return await this.GetOrderDetails(placedOrdersFilePath);
        }

        internal async Task<ConcurrentQueue<OrderInfo>> GetPendingOrderDetails()
        {
            return await this.GetOrderDetails(pendingOrdersFilePath);
        }

        internal async Task SavePendingOrdersAsync(ConcurrentQueue<OrderInfo> pendingOrders)
        {
            await this.SaveOrdersAsync(pendingOrders, pendingOrdersFilePath);
        }

        internal async Task SaveOrdersAsync(ConcurrentQueue<OrderInfo> Orders, string filePath)
        {
            string orderDetail = JsonSerializer.Serialize(Orders,
                new JsonSerializerOptions
                {
                    WriteIndented = true,
                });
            await File.WriteAllTextAsync(filePath, orderDetail);
        }

        internal async Task SavePlacedOrdersAsync(ConcurrentQueue<OrderInfo> placedOrders)
        {
            await this.SaveOrdersAsync(placedOrders, placedOrdersFilePath);
        }

        public async Task SaveStockAsync(ConcurrentDictionary<string, int> stocks)
        {
            string stockDetail = JsonSerializer.Serialize(
                stocks,
                new JsonSerializerOptions
                {
                    WriteIndented = true
                });

            await File.WriteAllTextAsync(
                stocksFilePath,
                stockDetail);
        }
    }
}
