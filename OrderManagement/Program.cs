using OrderManagement.Controller;
using OrderManagement.Repository;
using OrderManagement.Service;
using OrderManagement.Views;

namespace OrderManagement
{
    internal class Program
    {
        public static async Task Main(string[] args)
        {
            string stocksFilePath = "stocks.json";
            string pendingOrdersFilePath = "pendingOrders.json";
            string placedOrdersFilePath = "placedOrders.json";
            string loggerFilePath = "logger.json";
            OrderRepository orderRepository = new OrderRepository(
            pendingOrdersFilePath,
            placedOrdersFilePath,
            loggerFilePath,
            stocksFilePath);
            View view = new View();
            OrderService orderService = new OrderService(orderRepository);
            orderService.StartWorkers();
            await orderService.LoadFiles();
            OrderController orderController = new OrderController(orderService, view);
            orderController.HandleMenuOperations();
    }
    }
}
