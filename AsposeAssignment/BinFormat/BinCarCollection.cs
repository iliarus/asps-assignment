using AsposeAssignment.Design;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace AsposeAssignment.BinFormat
{
    public class BinCarCollection : CarCollectionBase
    {
        const short HEADER_VALUE = 0x2526;
        const short HEADER_LENGTH = 2;
        const short COUNT_LENGTH = 4;
        const short DATE_LENGTH = 8;
        const short PRICE_LENGTH = 4;

        public static string FileExtension = "bin";
        protected override string FileFormat => FileExtension;

        public BinCarCollection(string path) : base(path) { }

        public override void AddItem(CarModel item, int position)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            if (position < 0) throw new ArgumentOutOfRangeException(nameof(position), "Non negative value is expected");
            if (!Validate()) throw new CarCollectionInvalidFormatException();

            long positionPtr = 0;
            int count = 0;
            var tail = new List<byte>();

            using (var stream = File.OpenRead(_path))
            using (var reader = new BinaryReader(stream))
            {
                count = ReadCount(stream, reader);

                if (position > count) throw new ArgumentOutOfRangeException(nameof(position));
                if (position == count) { positionPtr = stream.Length; }
                else
                {
                    int i = 0;
                    while (i < position)
                    {
                        SeekEndItem(stream, reader);
                        i++;
                    }
                    positionPtr = stream.Position;
                }

                //store the bytes after
                stream.Seek(positionPtr, SeekOrigin.Begin);
                while (stream.Position < stream.Length)
                {
                    tail.Add(reader.ReadByte());
                }

                reader.Close();
                stream.Close();
            }

            var bin = MapToBin(item);
            using (var stream = File.OpenWrite(_path))
            using (var writer = new BinaryWriter(stream))
            {
                WriteHeader(writer, ++count);
                stream.Seek(positionPtr, SeekOrigin.Begin);
                WriteItem(writer, bin);

                //write storing bytes
                writer.Write(tail.ToArray());
                writer.Close();
                stream.Close();
            }
        }

        public override void DeleteItem(int position)
        {
            if (position < 0) throw new ArgumentOutOfRangeException(nameof(position), "Non negative value is expected");
            if (!Validate()) throw new CarCollectionInvalidFormatException();

            long startItemPtr = 0;
            long endItemPtr = 0;
            int count = 0;
            var bytes = new List<byte>();

            using (var stream = File.OpenRead(_path))
            using (var reader = new BinaryReader(stream))
            {
                count = ReadCount(stream, reader);

                if (position >= count) throw new ArgumentOutOfRangeException(nameof(position));

                int i = 0;
                while (i <= position)
                {
                    if (i == position) startItemPtr = stream.Position;
                    SeekEndItem(stream, reader);
                    i++;
                }

                endItemPtr = stream.Position;

                //store the bytes w/o the deleted item
                stream.Seek(HEADER_LENGTH + COUNT_LENGTH, SeekOrigin.Begin);
                while (stream.Position < startItemPtr)
                {
                    bytes.Add(reader.ReadByte());
                }

                stream.Seek(endItemPtr, SeekOrigin.Begin);
                while (stream.Position < stream.Length)
                {
                    bytes.Add(reader.ReadByte());
                }

                reader.Close();
                stream.Close();
            }

            using (var stream = File.Create(_path))
            using (var writer = new BinaryWriter(stream))
            {
                WriteHeader(writer, --count);
                writer.Write(bytes.ToArray());
                writer.Close();
                stream.Close();
            }
        }

        public override List<CarModel> GetAllItems()
        {
            if (!Validate()) throw new CarCollectionInvalidFormatException();

            var allItems = new List<CarModel>();

            using (var stream = File.OpenRead(_path))
            using (var reader = new BinaryReader(stream))
            {
                var count = ReadCount(stream, reader);

                for (int i = 0; i < count; i++)
                {
                    var bin = ReadItem(reader);
                    allItems.Add(MapToModel(bin));
                }

                reader.Close();
                stream.Close();
            }

            return allItems;
        }

        public override CarModel GetItem(int position)
        {
            if (position < 0) throw new ArgumentOutOfRangeException(nameof(position), "Non negative value is expected");
            if (!Validate()) throw new CarCollectionInvalidFormatException();

            BinCarModel binItem;

            using (var stream = File.OpenRead(_path))
            using (var reader = new BinaryReader(stream))
            {
                var count = ReadCount(stream, reader);

                if (position > count) throw new ArgumentOutOfRangeException(nameof(position));

                int i = 0;
                while (i < position)
                {
                    SeekEndItem(stream, reader);
                    i++;
                }

                binItem = ReadItem(reader);
                reader.Close();
                stream.Close();
            }

            return MapToModel(binItem);
        }

        public override void ReplaceItem(CarModel item, int position)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            if (position < 0) throw new ArgumentOutOfRangeException(nameof(position), "Non negative value is expected");
            if (!Validate()) throw new CarCollectionInvalidFormatException();

            DeleteItem(position);
            AddItem(item, position);
        }

        public override void Create(IList<CarModel> items)
        {
            if (items == null) throw new ArgumentNullException(nameof(items));

            using (var stream = File.Create(_path))
            using (var writer = new BinaryWriter(stream))
            {
                WriteHeader(writer, items.Count);

                foreach (var item in items)
                {
                    WriteItem(writer, MapToBin(item));
                }

                writer.Close();
                stream.Close();
            }
        }

        public override int Count()
        {
            int count;
            using (var stream = File.OpenRead(_path))
            using (var reader = new BinaryReader(stream))
            {
                count = ReadCount(stream, reader);
                reader.Close();
                stream.Close();
            }

            return count;
        }

        private BinCarModel MapToBin(CarModel item)
        {
            return new BinCarModel()
            {
                Date = item.Date.Ticks,
                BrandName = Encoding.Unicode.GetBytes(item.BrandName),
                Price = item.Price
            };
        }

        private CarModel MapToModel(BinCarModel item)
        {
            return new CarModel()
            {
                Date = new DateTime(item.Date),
                BrandName = Encoding.Unicode.GetString(item.BrandName),
                Price = item.Price
            };
        }

        private static void SeekEndItem(Stream stream, BinaryReader br)
        {
            stream.Seek(DATE_LENGTH, SeekOrigin.Current);
            var brandNameLength = br.ReadInt16();
            stream.Seek(brandNameLength, SeekOrigin.Current);
            stream.Seek(PRICE_LENGTH, SeekOrigin.Current);
        }

        private static void WriteItem(BinaryWriter writer, BinCarModel item)
        {
            writer.Write(item.Date); //Date
            writer.Write(item.BrandNameLength); //Brand Name length
            writer.Write(item.BrandName); //Brand Name
            writer.Write(item.Price); //Price
        }

        private static void WriteHeader(BinaryWriter writer, int count)
        {
            writer.Write(HEADER_VALUE);
            writer.Write(count);
        }

        private int ReadCount(Stream stream, BinaryReader reader)
        {
            stream.Seek(HEADER_LENGTH, SeekOrigin.Begin);
            return reader.ReadInt32();
        }

        private static BinCarModel ReadItem(BinaryReader reader)
        {
            var bin = new BinCarModel();
            bin.Date = reader.ReadInt64();
            var brandNameLength = reader.ReadInt16();
            bin.BrandName = reader.ReadBytes(brandNameLength);
            bin.Price = reader.ReadInt32();
            return bin;
        }

        private bool Validate()
        {
            short headerValue;
            using (var stream = File.OpenRead(_path))
            using (var reader = new BinaryReader(stream))
            {
                headerValue = reader.ReadInt16();
                reader.Close();
                stream.Close();
            }

            return headerValue == HEADER_VALUE; 
        }
    }
}
