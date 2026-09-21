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
        public CoffeeInfo CoffeeInfo {  get; set; }
        public Guid UserId { get; init; }
        public Guid OrderId { get; init; }
        public int Quantity { get; set; }
        public decimal TotalAmount { get; set;}
        public OrderStatus OrderStatus { get; set; }
        public DateTime OrderTime {  get; set; }

    }
}
