using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderManagement.Service;
using OrderManagement.Views;

namespace OrderManagement.Controller
{
    internal class OrderController
    {
        private OrderService _orderService;
        private View _view;

        public OrderController(OrderService orderService, View view)
        {
            this._orderService = orderService;
            this._view = view;
        }

        internal async Task HandleMenuOperations()
        {
            bool shallExit = false;
            while (!shallExit)
            {
                string choice = this._view.DisplayAndGetMenuChoice();
                switch(choice)
                {
                    case "1":
                        await this.PlaceOrder();
                        break;
                    case "2":
                        //this.ViewAllOrders();
                        break;
                    case "3":
                        //this.GetOrderDetails();
                        break;
                    case "4":
                        await this.CancelOrder();
                        break;
                    case "5":
                        //this.GetReport();
                        break;
                    case "6":
                        shallExit = true;
                        break;
                }
            }
        }

        private async Task CancelOrder()
        {
            string productId = this._view.GetOrderDetail("Enter the product Id");
            await this._orderService.CancelOrder(productId);
        }

        private async Task PlaceOrder()
        {
            string productId = this._view.GetOrderDetail("Enter the product Id");
            string customerName = this._view.GetOrderDetail("enter the customer name");
            string quantityEntered = this._view.GetOrderDetail("Enter the quantity: ");
            int quantity = int.Parse(quantityEntered);
            await this._orderService.PlaceOrder(productId, customerName, quantity);
        }
    }
}
