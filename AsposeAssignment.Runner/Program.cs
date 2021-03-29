using AsposeAssignment.BinFormat;
using AsposeAssignment.Design;
using AsposeAssignment.XmlFormat;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AsposeAssignment.Runner
{
    class Program
    {
        static void Main(string[] args)
        {
            var binPath = @"TestFiles\Cars.bin";
            var xmlPath = @"TestFiles\Cars.xml";

            var carCollection = new BinCarCollection(binPath);
            var firstCar = new CarModel()
            {
                Date = DateTime.Today,
                BrandName = "Alfa Romeo New",
                Price = 11000
            };
            var secondCar = new CarModel()
            {
                Date = DateTime.Today,
                BrandName = "Toyota Land Cruiser",
                Price = 15000
            };

            //create a bin file
            carCollection.Create(new List<CarModel> { firstCar });
            
            //add a car to the bin file
            carCollection.AddItem(secondCar, 0);

            //delete second car from the bin file
            carCollection.DeleteItem(1);

            //convert the bin file to xml file
            CarCollectionConverter.Convert(binPath, xmlPath);
        }
    }
}
