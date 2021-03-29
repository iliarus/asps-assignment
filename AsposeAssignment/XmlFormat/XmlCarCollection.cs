using AsposeAssignment.Design;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Schema;

namespace AsposeAssignment.XmlFormat
{
    public class XmlCarCollection : CarCollectionBase
    {
        string RootName => "Document";
        string ItemTagName => "Car";
        const string DateFormat = "dd.MM.yyyy";

        public static string FileExtension = "xml";
        protected override string FileFormat => FileExtension;

        public XmlCarCollection(string path) : base(path) { }

        public override void Create(IList<CarModel> items)
        {
            if (items == null) throw new ArgumentNullException(nameof(items));

            var doc = new XDocument(new XElement(RootName));
            foreach (var item in items)
            {
                var xItem = MapToXml(item);
                doc.Root.Add(xItem);
            }

            doc.Save(_path);
        }

        public override void AddItem(CarModel item, int position)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            if (position < 0) throw new ArgumentOutOfRangeException(nameof(position), "Non negative position is expected");

            var xItem = MapToXml(item);

            var doc = XDocument.Load(_path);
            if (position == 0)
            {
                doc.Root.AddFirst(xItem);
            }
            else
            {
                var elementAfter = doc.Descendants(ItemTagName).Skip(position).FirstOrDefault();

                if (elementAfter != null) elementAfter.AddBeforeSelf(xItem);
                else doc.Root.Add(xItem);
            }

            doc.Save(_path);
            doc = null;
        }

        public override void DeleteItem(int position)
        {
            if (position < 0) throw new ArgumentOutOfRangeException(nameof(position), "Non negative position is expected");

            var doc = XDocument.Load(_path);
            doc.Descendants(ItemTagName).Skip(position).Take(1).Remove();
            doc.Save(_path);
            doc = null;
        }

        public override List<CarModel> GetAllItems()
        {
            var doc = XDocument.Load(_path);
            var result = doc.Descendants(ItemTagName).Select(item => MapToModel(item)).ToList();
            doc = null;
            return result;
        }

        public override CarModel GetItem(int position)
        {
            if (position < 0) throw new ArgumentOutOfRangeException(nameof(position), "Non negative position is expected");

            var doc = XDocument.Load(_path);
            var car = doc.Descendants(ItemTagName)
                .Skip(position)
                .Take(1)
                .Select(item => MapToModel(item))
                .SingleOrDefault();
            doc = null;

            return car;
        }

        public override void ReplaceItem(CarModel item, int position)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            if (position < 0) throw new ArgumentOutOfRangeException(nameof(position), "Non negative position is expected");

            var doc = XDocument.Load(_path);
            var elementToUpdate = doc.Descendants(ItemTagName).Skip(position).FirstOrDefault();
            if (elementToUpdate == null) throw new IndexOutOfRangeException($"Element at position {position} not found");
            elementToUpdate.ReplaceWith(MapToXml(item));
            doc.Save(_path);
            doc = null;
        }

        public override int Count()
        {
            var doc = XDocument.Load(_path);
            var count = doc.Descendants(ItemTagName).Count();
            doc = null;
            return count;
        }

        static void ValidationEventHandler(object sender, ValidationEventArgs e)
        {
            XmlSeverityType type = XmlSeverityType.Warning;
            if (Enum.TryParse<XmlSeverityType>("Error", out type))
            {
                if (type == XmlSeverityType.Error) throw new Exception(e.Message);
            }
        }

        CarModel MapToModel(XElement node)
        {
            var dateNode = node.Element(nameof(CarModel.Date));
            var brandNameNode = node.Element(nameof(CarModel.BrandName));
            var priceNode = node.Element(nameof(CarModel.Price));

            if (dateNode == null || brandNameNode == null || priceNode == null)
                throw new CarCollectionInvalidFormatException();

            try
            {
                return new CarModel()
                {
                    Date = DateTime.ParseExact(dateNode.Value, DateFormat, null, DateTimeStyles.None),
                    BrandName = brandNameNode.Value,
                    Price = Int32.Parse(priceNode.Value)
                };
            }
            catch (FormatException ex)
            {
                throw new CarCollectionInvalidFormatException(ex);
            }
        }

        XElement MapToXml(CarModel model)
        {
            var xItem = new XElement(ItemTagName);
            xItem.Add(new XElement(nameof(CarModel.Date), model.Date.ToString(DateFormat)));
            xItem.Add(new XElement(nameof(CarModel.BrandName), model.BrandName));
            xItem.Add(new XElement(nameof(CarModel.Price), model.Price));

            return xItem;
        }
    }
}
