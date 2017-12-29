using NUnit.Framework;
using System;
using ShapeLib;
using System.Collections.Generic;

namespace SvgToEmbCSVTests
{
    [TestFixture()]
    public class FillCalculatorTests
    {
        [Test()]
        public void FindValidIntersectionTest()
        {
            Point p1, p2;
            CreateLinePoints(out p1, out p2);
            var intersection = FillCalculator.FindIntersection(p1, p2, 0);

            Assert.AreEqual(0, intersection.Y);
            Assert.AreEqual(2, intersection.X);
        }

        [Test()]
        public void FindLowerPointIntersectionTest()
        {
            Point p1, p2;
            CreateLinePoints(out p1, out p2);
            var intersection = FillCalculator.FindIntersection(p1, p2, -4);

            Assert.IsNotNull(intersection);
        }

        [Test()]
        public void FindUpperPointIntersectionTest()
        {
            Point p1, p2;
            CreateLinePoints(out p1, out p2);
            var intersection = FillCalculator.FindIntersection(p1, p2, 4);

            Assert.IsNotNull(intersection);
        }

        [Test()]
        public void FindInvalidPointIntersectionReturnsNullTest([Values(-5, 5, double.PositiveInfinity, double.NegativeInfinity, double.NaN)]double d)
        {
            Point p1, p2;
            CreateLinePoints(out p1, out p2);
            var intersection = FillCalculator.FindIntersection(p1, p2, d);

            Assert.IsNull(intersection);
        }

        [Test()]
        public void FindIntersectionsOrderIsMaintained()
        {
            var p = this.createTriangle();
            var intersection = FillCalculator.FindIntersections(0.1, p);
            var first = intersection[0];
            var second = intersection[1];
            Assert.IsTrue(first.X <= second.X);
        }

        [Test()]
        public void FindIntersectionsOrderIsMaintained2()
        {
            var p = this.createInverseTriangle();
            var intersection = FillCalculator.FindIntersections(-0.1, p);
            var first = intersection[0];
            var second = intersection[1];
            Assert.IsTrue(first.X <= second.X);
        }

        [Test()]
        public void FindIntersectionsOrderIsMaintained3()
        {
            var p = this.createInverseTriangleOrder2();
            var intersection = FillCalculator.FindIntersections(-0.1, p);
            var first = intersection[0];
            var second = intersection[1];
            Assert.IsTrue(first.X <= second.X);
        }

        [Test()]
        public void FindIntersectionsOrderIsMaintained4()
        {
            var p = this.createInverseTriangleOrder3();
            var intersection = FillCalculator.FindIntersections(-0.1, p);
            var first = intersection[0];
            var second = intersection[1];
            Assert.IsTrue(first.X <= second.X);
        }

        private static void CreateLinePoints(out Point p1, out Point p2)
        {
            p1 = new Point(1, -4);
            p2 = new Point(3, 4);
        }

        private Polygon createTriangle()
        {
            return new Polygon(
                new List<Point> {
                new Point(0, 1),
                new Point(1, 1),
                new Point(1, 0),
            }
            );
        }
        private Polygon createInverseTriangle()
        {
            return new Polygon(
                new List<Point> {
                new Point(0, 0),
                new Point(1, 0),
                new Point(0.5, -1),
            }
            );
        }
        private Polygon createInverseTriangleOrder2()
        {
            return new Polygon(
                new List<Point> {
                new Point(1, 0),
                new Point(0.5, -1),
                new Point(0, 0),
            }
            );
        }
        private Polygon createInverseTriangleOrder3()
        {
            return new Polygon(
                new List<Point> {
                new Point(0.5, -1),
                new Point(0, 0),
                new Point(1, 0),
            }
            );
        }
    }
}

