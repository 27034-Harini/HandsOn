using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoffeeShop.Enums;

namespace CoffeeShop.Model
{
    internal class OrderInfo
    {
        public OrderInfo(CoffeeInfo coffeeInfo, Guid userId, Guid orderId, int quantity, decimal totalAmount, OrderStatus orderStatus, DateTime orderTime)
        {
            CoffeeInfo = coffeeInfo;
            UserId = userId;
            OrderId = orderId;
            Quantity = quantity;
            TotalAmount = totalAmount;
            OrderStatus = orderStatus;
            OrderTime = orderTime;
        }

        public CoffeeInfo CoffeeInfo {  get; set; }
        public Guid UserId { get; init; }
        public Guid OrderId { get; init; }
        public int Quantity { get; set; }
        public decimal TotalAmount { get; set;}
        public OrderStatus OrderStatus { get; set; }
        public DateTime OrderTime {  get; set; }

    }
}
