using CoffeeShop.Repository;
using CoffeeShop.Service;
using CoffeeShop.Helper;
using CoffeeShop.Views;
using CoffeeShop.Controller;
using System.Runtime.CompilerServices;
using CoffeeShop.Model;

namespace CoffeeShop
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string _userFilePath = "users.json";
            string _orderHistoryFilePath = "orderHistory.json";
            string _pendingOrders = "pendingOrders.json";
            UserRepository userRepository = new UserRepository(_userFilePath);
            Validator validator = new Validator();
            View view = new View();
            UserService userService = new UserService(validator, userRepository);
            CoffeeRepository coffeeRepository = new CoffeeRepository(_orderHistoryFilePath, _pendingOrders);
            OrderService orderService = new OrderService(validator, coffeeRepository);
            MachineInfo machine1 = new MachineInfo{ OrderId = Guid.Empty, MachineId = Guid.NewGuid(), MachineStatus = Enums.MachineStatus.Available };
            MachineInfo machine2 = new MachineInfo { OrderId = Guid.Empty, MachineId = Guid.NewGuid(), MachineStatus = Enums.MachineStatus.Available };
            Task.Run(() => orderService.ProcessOrders(machine1));
            Task.Run(() => orderService.ProcessOrders(machine2));
            CoffeeController coffeeController = new CoffeeController(view, validator, orderService);
            AuthenticationController authenticationController = new AuthenticationController(view, userService, coffeeController);
            authenticationController.UserAccessOption();
        }
    }
}
