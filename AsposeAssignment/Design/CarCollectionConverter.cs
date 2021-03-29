using AsposeAssignment.BinFormat;
using AsposeAssignment.XmlFormat;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AsposeAssignment.Design
{
    public class CarCollectionConverter
    {
        static Dictionary<string, Type> _formats = new Dictionary<string, Type>()
        {
            { BinCarCollection.FileExtension.ToLower(), typeof(BinCarCollection) },
            { XmlCarCollection.FileExtension.ToLower(), typeof(XmlCarCollection) }
        };

        /// <summary>
        /// Converts one file format to another format
        /// </summary>
        /// <param name="sourcePath">Path to the file, which will be converted</param>
        /// <param name="targetPath">Path to the file, which will be created </param>
        public static void Convert(string sourcePath, string targetPath)
        {
            if (!File.Exists(sourcePath)) throw new FileNotFoundException(nameof(sourcePath));

            var sourceExtension = Path.GetExtension(sourcePath).TrimStart('.').ToLower();
            if (!_formats.ContainsKey(sourceExtension)) 
                throw new ArgumentOutOfRangeException(nameof(sourcePath), $"Format '{sourceExtension}' is not supported for convertion");
            var sourceType = _formats[sourceExtension];

            var targetExtension = Path.GetExtension(targetPath).TrimStart('.').ToLower();
            if (!_formats.ContainsKey(targetExtension))
                throw new ArgumentOutOfRangeException(nameof(targetPath), $"Format '{targetExtension}' is not supported for convertion");
            var targetType = _formats[targetExtension];

            var sourceCtor = sourceType.GetConstructor(new[] { typeof(string) });
            var sourceCarCollection = (CarCollectionBase)sourceCtor.Invoke(new object[] { sourcePath });

            var targetCtor = targetType.GetConstructor(new[] { typeof(string) });
            var targetCarCollection = (CarCollectionBase)targetCtor.Invoke(new object[] { targetPath });

            var cars = sourceCarCollection.GetAllItems();
            targetCarCollection.Create(cars);
        }
    }
}
