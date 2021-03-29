using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsposeAssignment.Design
{
    public abstract class CarCollectionBase
    {
        protected string _path;
        protected abstract string FileFormat { get; }

        public CarCollectionBase(string path)
        {
            if (string.IsNullOrEmpty(path)) throw new ArgumentNullException(nameof(path));

            var extension = Path.GetExtension(path).TrimStart('.').ToLower();
            if (extension != FileFormat.ToLower())
                throw new ArgumentOutOfRangeException(nameof(path), $"Format '{extension}' is invalid");

            _path = path;
        }

        /// <summary>
        /// Creates a new or overwrite an existing file with list of CarModel items with specified format
        /// </summary>
        /// <param name="items">List of CarModel items</param>
        public abstract void Create(IList<CarModel> items);
        /// <summary>
        /// Adds an CarModel item to the file at specified position.
        /// </summary>
        /// <param name="item">CarModel item</param>
        /// <param name="position">Position in a file, positions start at 0</param>
        public abstract void AddItem(CarModel item, int position);
        /// <summary>
        /// Replaces a CarModel item at position with a new one in a file
        /// </summary>
        /// <param name="item">New CarModel item</param>
        /// <param name="position">Position in a file, positions start at 0</param>
        public abstract void ReplaceItem(CarModel item, int position);
        /// <summary>
        /// Deletes a CarModel item from a file at specified position
        /// </summary>
        /// <param name="position">Position in a file, positions start at 0</param>
        public abstract void DeleteItem(int position);
        /// <summary>
        /// Return a CarModel item at specified position in a file
        /// </summary>
        /// <param name="position">Position in a file, positions start at 0</param>
        /// <returns>CarModel item</returns>
        public abstract CarModel GetItem(int position);
        /// <summary>
        /// Returns all CarModel items from a file
        /// </summary>
        /// <returns>CarModel items</returns>
        public abstract IList<CarModel> GetAllItems();
        /// <summary>
        /// Returns count of CarModel items in a file
        /// </summary>
        /// <returns>Count</returns>
        public abstract int Count();
    }
}
