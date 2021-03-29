using AsposeAssignment.BinFormat;
using AsposeAssignment.Design;
using AsposeAssignment.XmlFormat;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsposeAssignment.Tests
{
    public class CarCollectionConverterTest
    {
        [SetUp]
        public void Setup() { }

        [Test]
        public void Convert_XmlToBin()
        {
            var xmlSource = @"TestFiles\cars_source.xml";
            var binTarget = @"TestFiles\cars_target.bin";

            var xmlCollection = new XmlCarCollection(xmlSource);
            xmlCollection.Create(Constants.Cars);

            CarCollectionConverter.Convert(xmlSource, binTarget);

            var binCollection = new BinCarCollection(binTarget);
            var cars = binCollection.GetAllItems();

            Assert.AreEqual(3, cars.Count);

            Assert.AreEqual(Constants.Cars[0].Date, cars[0].Date);
            Assert.AreEqual(Constants.Cars[0].BrandName, cars[0].BrandName);
            Assert.AreEqual(Constants.Cars[0].Price, cars[0].Price);

            Assert.AreEqual(Constants.Cars[1].Date, cars[1].Date);
            Assert.AreEqual(Constants.Cars[1].BrandName, cars[1].BrandName);
            Assert.AreEqual(Constants.Cars[1].Price, cars[1].Price);

            Assert.AreEqual(Constants.Cars[2].Date, cars[2].Date);
            Assert.AreEqual(Constants.Cars[2].BrandName, cars[2].BrandName);
            Assert.AreEqual(Constants.Cars[2].Price, cars[2].Price);
        }

        [Test]
        public void Convert_BinToXml()
        {
            var binSource = @"TestFiles\cars_source.bin";
            var xmlTarget = @"TestFiles\cars_target.xml";

            var binCollection = new BinCarCollection(binSource);
            binCollection.Create(Constants.Cars);

            CarCollectionConverter.Convert(binSource, xmlTarget);

            var xmlCollection = new XmlCarCollection(xmlTarget);
            var cars = xmlCollection.GetAllItems();

            Assert.AreEqual(3, cars.Count);

            Assert.AreEqual(Constants.Cars[0].Date, cars[0].Date);
            Assert.AreEqual(Constants.Cars[0].BrandName, cars[0].BrandName);
            Assert.AreEqual(Constants.Cars[0].Price, cars[0].Price);

            Assert.AreEqual(Constants.Cars[1].Date, cars[1].Date);
            Assert.AreEqual(Constants.Cars[1].BrandName, cars[1].BrandName);
            Assert.AreEqual(Constants.Cars[1].Price, cars[1].Price);

            Assert.AreEqual(Constants.Cars[2].Date, cars[2].Date);
            Assert.AreEqual(Constants.Cars[2].BrandName, cars[2].BrandName);
            Assert.AreEqual(Constants.Cars[2].Price, cars[2].Price);
        }
    }
}
