using NUnit.Framework;
using System;
using ShapeLib;

namespace SvgToEmbCSVTests
{
    [TestFixture()]
    public class LineWidenerTests
    {
        [Test()]
        public void NormalLengthIsOne()
        {
            Point p1 = new Point(1.234, -3.456);
            Point p2 = new Point(34, 456);
            var lw = new LineWidener(p1, p2);

            Point normal = lw.FindNormal();

            Assert.AreEqual(1.0, Math.Round(normal.Length(), 6));
        }

        [Test()]
        public void NormalLengthIsOneForNoXDistance()
        {
            Point p1 = new Point(34, -3.456);
            Point p2 = new Point(34, 78);
            var lw = new LineWidener(p1, p2);

            Point normal = lw.FindNormal();

            Assert.AreEqual(1.0, Math.Round(normal.Length(), 6));
        }

        [Test()]
        public void NormalLengthIsOneForNoYDistance()
        {
            Point p1 = new Point(1.234, -3.456);
            Point p2 = new Point(34, -3.456);
            var lw = new LineWidener(p1, p2);

            Point normal = lw.FindNormal();

            Assert.AreEqual(1.0, Math.Round(normal.Length(), 6));
        }

        [Test()]
        public void NormalScalarProductIsZero()
        {
            Point p1 = new Point(1.234, -3.456);
            Point p2 = new Point(34, 456);
            var lw = new LineWidener(p1, p2);

            Point normal = lw.FindNormal();
            double scalarProduct = normal.X * (p1.X - p2.X) + normal.Y * (p1.Y - p2.Y);
            Assert.AreEqual(0.0, Math.Round(scalarProduct, 6));
        }

        [Test()]
        public void WidenedPolygonHasFourCorners()
        {
            Point p1 = new Point(1.234, -3.456);
            Point p2 = new Point(34, 456);
            var lw = new LineWidener(p1, p2);

            Polygon p = lw.Widen(1.0);

            Assert.AreEqual(4, p.Vertices.Count);;
        }

        [Test()]
        public void WidenedPolygonHasCorrectWidth()
        {
            Point p1 = new Point(1.234, -3.456);
            Point p2 = new Point(34, 456);
            var lw = new LineWidener(p1, p2);

            Polygon p = lw.Widen(1.0);

            Assert.AreEqual(1.0, p.Vertices[0].Distance(p.Vertices[3]), 1e-6);
            Assert.AreEqual(1.0, p.Vertices[1].Distance(p.Vertices[2]), 1e-6);
        }

        [Test()]
        public void WidenedPolygonHasCorrectHeight()
        {
            Point p1 = new Point(1.234, -3.456);
            Point p2 = new Point(34, 456);
            var lw = new LineWidener(p1, p2);
            double height = p1.Distance(p2);

            Polygon p = lw.Widen(1.0);

            Assert.AreEqual(height, p.Vertices[0].Distance(p.Vertices[1]), 1e-6);
            Assert.AreEqual(height, p.Vertices[2].Distance(p.Vertices[3]), 1e-6);
        }
    }
}

