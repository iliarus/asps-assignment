using AsposeAssignment.BinFormat;
using AsposeAssignment.Design;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;

namespace AsposeAssignment.Tests
{
    public class BinCarCollectionTest : CarCollectionBaseTest<BinCarCollection>
    {
        public BinCarCollectionTest() : base(@"TestFiles\cars.bin", (x) => new BinCarCollection(x))
        {
        }
    }
}