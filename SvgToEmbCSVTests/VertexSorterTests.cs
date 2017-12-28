using NUnit.Framework;
using System;
using System.Collections.Generic;
using ShapeLib;

namespace SvgToEmbCSVTests
{
    [TestFixture()]
    public class VertexSorterTests
    {
        [Test()]
        public void IdentifiesTopLeftPoint1()
        {

            var points = new List<Point>
                {
                    new Point(0, 1),
                    new Point(1, 1),
                    new Point(1, 0),
                    new Point(0, 0),
                };

            Point tl = VertexSorter.GetTopLeft(points);

            Assert.AreEqual(0, tl.X);
            Assert.AreEqual(1, tl.Y);
        }

        [Test()]
        public void IdentifiesTopLeftIndex1()
        {
            var points = new List<Point>
                {
                    new Point(0, 0),
                    new Point(0, 1),
                    new Point(1, 1),
                    new Point(1, 0),
                };

            int tl = VertexSorter.GetTopLeftIndex(points);

            Assert.AreEqual(1, tl);
        }

        [Test()]
        public void IdentifiesTopLeftPoint2()
        {
            var points = new List<Point>
                {
                    new Point(1, 0),
                    new Point(1, 1),
                    new Point(0, 0),
                    new Point(0, 1),
                };

            Point tl = VertexSorter.GetTopLeft(points);

            Assert.AreEqual(0, tl.X);
            Assert.AreEqual(1, tl.Y);
        }

        [Test()]
        public void IdentifiesTopLeftPoint3()
        {
            var points = new List<Point>
                {
                    new Point(0, 1),
                    new Point(-1, 0),
                    new Point(1, 0),
                };

            Point tl = VertexSorter.GetTopLeft(points);

            Assert.AreEqual(0, tl.X);
            Assert.AreEqual(1, tl.Y);
        }

        [Test()]
        public void IdentifiesTopLeftIndex3()
        {
            var points =new List<Point>
                {
                    new Point(0, 1),
                    new Point(-1, 0),
                    new Point(1, 0),
                };

            int tl = VertexSorter.GetTopLeftIndex(points);

            Assert.AreEqual(0, tl);
        }

        [Test()]
        public void IdentifiesTopLeftPoint4()
        {
            var points = new List<Point>
                {
                    new Point(1, 1),
                    new Point(-1, 0),
                    new Point(1, 0),
                };

            Point tl = VertexSorter.GetTopLeft(points);

            Assert.AreEqual(1, tl.X);
            Assert.AreEqual(1, tl.Y);
        }
    }
}

