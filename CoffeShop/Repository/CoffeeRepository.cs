using CoffeeShop.Model;

namespace CoffeeShop.Repository
{
    internal class CoffeeRepository
    {
        public List<CoffeeInfo> coffeeInfos = new List<CoffeeInfo>();
        public void AddingCoffee()
        {
            coffeeInfos.Add(new CoffeeInfo(Enums.CoffeeTypes.Espresso, 100, TimeSpan.FromSeconds(30)));
            coffeeInfos.Add(new CoffeeInfo(Enums.CoffeeTypes.Americano, 100, TimeSpan.FromSeconds(50)));
            coffeeInfos.Add(new CoffeeInfo(Enums.CoffeeTypes.Latte, 100, TimeSpan.FromSeconds(80)));
            coffeeInfos.Add(new CoffeeInfo(Enums.CoffeeTypes.Mocha, 100, TimeSpan.FromSeconds(60)));
            coffeeInfos.Add(new CoffeeInfo(Enums.CoffeeTypes.Cappuccino, 100, TimeSpan.FromSeconds(100)));
        }

        internal List<CoffeeInfo> GetMenu()
        {
            return coffeeInfos;
        }
    }
}