using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoffeeShop.Enums;

namespace CoffeeShop.Model
{
    internal class MachineInfo
    {
        public Guid OrderId { get; init; }
        public Guid MachineId { get; init; }
        public MachineStatus MachineStatus { get; set; }

    }
}
