using AsposeAssignment.Design;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsposeAssignment.Tests
{
    public class CarCollectionBaseTest<T> where T : CarCollectionBase
    {
        string _path;
        T _carCollection;

        public CarCollectionBaseTest(string path, Func<string, T> creator)
        {
            _path = path;
            _carCollection = creator(path);
        }

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Create()
        {
            _carCollection.Create(Constants.Cars);
            var car = _carCollection.GetAllItems();

            var cars = _carCollection.GetAllItems();

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
        public void AddItem_First()
        {
            _carCollection.Create(new List<CarModel> { Constants.Cars[0] });
            _carCollection.AddItem(Constants.Cars[1], 0);

            var car = _carCollection.GetItem(0);

            Assert.AreEqual(Constants.Cars[1].Date, car.Date);
            Assert.AreEqual(Constants.Cars[1].BrandName, car.BrandName);
            Assert.AreEqual(Constants.Cars[1].Price, car.Price);
        }

        [Test]
        public void AddItem_Last()
        {
            _carCollection.Create(new List<CarModel> { Constants.Cars[0] });
            _carCollection.AddItem(Constants.Cars[1], 1);

            var car = _carCollection.GetItem(1);

            Assert.AreEqual(Constants.Cars[1].Date, car.Date);
            Assert.AreEqual(Constants.Cars[1].BrandName, car.BrandName);
            Assert.AreEqual(Constants.Cars[1].Price, car.Price);
        }

        [Test]
        public void AddItem_Middle()
        {
            _carCollection.Create(Constants.Cars);
            _carCollection.AddItem(Constants.Cars[1], 1);

            var car = _carCollection.GetItem(1);

            Assert.AreEqual(Constants.Cars[1].Date, car.Date);
            Assert.AreEqual(Constants.Cars[1].BrandName, car.BrandName);
            Assert.AreEqual(Constants.Cars[1].Price, car.Price);
        }

        [Test]
        public void UpdateItem_First()
        {
            _carCollection.Create(new List<CarModel> { Constants.Cars[0] });
            _carCollection.AddItem(Constants.Cars[1], 1);

            _carCollection.ReplaceItem(Constants.Cars[2], 0);

            var car = _carCollection.GetItem(0);

            Assert.AreEqual(Constants.Cars[2].Date, car.Date);
            Assert.AreEqual(Constants.Cars[2].BrandName, car.BrandName);
            Assert.AreEqual(Constants.Cars[2].Price, car.Price);
        }

        [Test]
        public void UpdateItem_Last()
        {
            _carCollection.Create(Constants.Cars);

            _carCollection.ReplaceItem(Constants.Cars[2], 1);

            var car = _carCollection.GetItem(1);

            Assert.AreEqual(Constants.Cars[2].Date, car.Date);
            Assert.AreEqual(Constants.Cars[2].BrandName, car.BrandName);
            Assert.AreEqual(Constants.Cars[2].Price, car.Price);
        }

        [Test]
        public void UpdateItem_Middle()
        {
            _carCollection.Create(Constants.Cars);

            _carCollection.ReplaceItem(Constants.Cars[0], 1);

            var car = _carCollection.GetItem(1);

            Assert.AreEqual(Constants.Cars[0].Date, car.Date);
            Assert.AreEqual(Constants.Cars[0].BrandName, car.BrandName);
            Assert.AreEqual(Constants.Cars[0].Price, car.Price);
        }

        [Test]
        public void DeleteItem_First()
        {
            _carCollection.Create(Constants.Cars);

            _carCollection.DeleteItem(0);

            var cars = _carCollection.GetAllItems();

            Assert.AreEqual(2, cars.Count);

            Assert.AreEqual(Constants.Cars[1].Date, cars[0].Date);
            Assert.AreEqual(Constants.Cars[1].BrandName, cars[0].BrandName);
            Assert.AreEqual(Constants.Cars[1].Price, cars[0].Price);

            Assert.AreEqual(Constants.Cars[2].Date, cars[1].Date);
            Assert.AreEqual(Constants.Cars[2].BrandName, cars[1].BrandName);
            Assert.AreEqual(Constants.Cars[2].Price, cars[1].Price);
        }

        [Test]
        public void DeleteItem_Last()
        {
            _carCollection.Create(Constants.Cars);

            _carCollection.DeleteItem(2);

            var cars = _carCollection.GetAllItems();

            Assert.AreEqual(2, cars.Count);

            Assert.AreEqual(Constants.Cars[0].Date, cars[0].Date);
            Assert.AreEqual(Constants.Cars[0].BrandName, cars[0].BrandName);
            Assert.AreEqual(Constants.Cars[0].Price, cars[0].Price);

            Assert.AreEqual(Constants.Cars[1].Date, cars[1].Date);
            Assert.AreEqual(Constants.Cars[1].BrandName, cars[1].BrandName);
            Assert.AreEqual(Constants.Cars[1].Price, cars[1].Price);
        }

        [Test]
        public void DeleteItem_Middle()
        {
            _carCollection.Create(Constants.Cars);

            _carCollection.DeleteItem(1);

            var cars = _carCollection.GetAllItems();

            Assert.AreEqual(2, cars.Count);

            Assert.AreEqual(Constants.Cars[0].Date, cars[0].Date);
            Assert.AreEqual(Constants.Cars[0].BrandName, cars[0].BrandName);
            Assert.AreEqual(Constants.Cars[0].Price, cars[0].Price);

            Assert.AreEqual(Constants.Cars[2].Date, cars[1].Date);
            Assert.AreEqual(Constants.Cars[2].BrandName, cars[1].BrandName);
            Assert.AreEqual(Constants.Cars[2].Price, cars[1].Price);
        }

        [Test]
        public void DeleteItem_Single()
        {
            _carCollection.Create(new List<CarModel> { Constants.Cars[0] });

            _carCollection.DeleteItem(0);

            var cars = _carCollection.GetAllItems();

            Assert.AreEqual(0, cars.Count);
        }

        [Test]
        public void GetAllItems()
        {
            _carCollection.Create(Constants.Cars);

            var cars = _carCollection.GetAllItems();

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
        public void Empty()
        {
            _carCollection.Create(new List<CarModel> { Constants.Cars[0] });
            _carCollection.DeleteItem(0);
            var cars = _carCollection.GetAllItems();
            var count = _carCollection.Count();

            Assert.AreEqual(0, cars.Count());
            Assert.AreEqual(0, count);
        }

        [Test]
        public void Count()
        {
            _carCollection.Create(Constants.Cars);

            var count = _carCollection.Count();

            Assert.AreEqual(3, count);
        }
    }
}
