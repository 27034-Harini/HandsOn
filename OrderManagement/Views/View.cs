using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Views
{
    internal class View
    {
        internal string DisplayAndGetMenuChoice()
        {
            Console.WriteLine("1. Place Order\n" +
                "2. View All Orders\n" +
                "3. View Order Details\n" +
                "4. Cancel Order" +
                "5. Reports\n" +
                "6. Exit\n" +
                "Choose an option :");
            return Console.ReadLine().Trim() ?? string.Empty;
        }

        internal string GetOrderDetail(string v)
        {
            throw new NotImplementedException();
        }
    }
}
