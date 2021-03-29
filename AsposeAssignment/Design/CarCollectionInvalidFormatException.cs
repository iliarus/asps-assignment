using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsposeAssignment.Design
{
    public class CarCollectionInvalidFormatException : Exception
    {
        public CarCollectionInvalidFormatException() : base("Incorrect file format") { }

        public CarCollectionInvalidFormatException(Exception ex) : base("Incorrect file format", ex) { }
    }
}
