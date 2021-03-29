using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsposeAssignment.Tests
{
    internal class Constants
    {
        public static readonly List<CarModel> Cars = new List<CarModel>
        {
            new CarModel() { Date = DateTime.Today, BrandName = "Renault Arkana", Price = 1500000 },
            new CarModel() { Date = DateTime.Today, BrandName = "Toyota Land Cruiser", Price = 3500000 },
            new CarModel() { Date = DateTime.Today, BrandName = "Alfa Romeo Brera", Price = 5500000 },
        };
    }
}
