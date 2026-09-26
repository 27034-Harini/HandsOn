using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Enums
{
    internal enum OrderStatus
    {
        Null,
        Placed,
        Validating,
        Reserved,
        Packing,
        Shipped,
        Rejected,
        Cancelled,
    }
}
