using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoffeeShop.Enums;

namespace CoffeeShop.Model
{
    internal class CoffeeInfo
    {
        public CoffeeInfo(CoffeeTypes coffeeName, int price, TimeSpan timeTaken)
        {
            this.CoffeeName = coffeeName;
            this.Price = price;
            this.TimeTaken = timeTaken;
        }

        public CoffeeTypes CoffeeName {  get; set; }
        public decimal Price { get; set; }
        public TimeSpan TimeTaken { get; set; }

    }
}
