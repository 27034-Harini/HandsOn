using CoffeeShop.Repository;
using CoffeeShop.Service;
using CoffeeShop.Helper;
using CoffeeShop.Views;
using CoffeeShop.Controller;

namespace CoffeeShop
{
    internal class Program
    {
        static void Main(string[] args)
        {
            UserRepository userRepository = new UserRepository();
            Validator validator = new Validator();
            View view = new View();
            UserService userService = new UserService(validator, userRepository);
            CoffeeRepository coffeeRepository = new CoffeeRepository();
            OrderService orderService = new OrderService(validator, coffeeRepository);
            CoffeeController coffeeController = new CoffeeController(view, validator, orderService);
            AuthenticationController authenticationController = new AuthenticationController(view, userService, coffeeController);
            authenticationController.UserAccessOption();
        }
    }
}
