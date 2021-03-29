using AsposeAssignment.Design;
using AsposeAssignment.XmlFormat;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;

namespace AsposeAssignment.Tests
{
    public class XmlCarCollectionTest : CarCollectionBaseTest<XmlCarCollection>
    {
        public XmlCarCollectionTest() : base(@"TestFiles\cars.xml", (x) => new XmlCarCollection(x))
        {
        }
    }
}