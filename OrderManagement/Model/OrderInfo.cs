using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using OrderManagement.Enums;

namespace OrderManagement.Model
{
    internal class OrderInfo
    {
        public OrderInfo(Guid orderId, string customerName, string productId, int quantity, DateTime createdAt, OrderStatus previousState, OrderStatus currentState, bool isCancelable)
        {
            this.OrderId = orderId;
            this.CustomerName = customerName;
            this.ProductId = productId;
            this.Quantity = quantity;
            this.CreatedAt = createdAt;
            this.PreviousState = previousState;
            this.CurrentState = currentState;
            this.IsCancelable = isCancelable;
        }

        public Guid OrderId { get; init; }
        public string CustomerName { get; set; } = string.Empty;
        public string ProductId { get; init; } = string.Empty;
        public int Quantity { get; set; }
        public DateTime CreatedAt { get; set; }
        public OrderStatus PreviousState { get; set; } = OrderStatus.Null;
        public OrderStatus CurrentState { get; set; }
        public bool IsCancelable { get; set; }

        [JsonIgnore]
        public object LockObject { get; } = new();
    }
}
