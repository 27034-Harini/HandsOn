using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShop.Enums
{
    internal enum OrderStatus
    {
        Ordered = 1,
        Preparing,
        Ready,
        Delivered,
        Cancelled,
    }
}
