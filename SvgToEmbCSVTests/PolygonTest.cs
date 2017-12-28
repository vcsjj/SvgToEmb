using NUnit.Framework;
using System;
using ShapeLib;
using System.Collections.Generic;

namespace SvgToEmbCSVTests
{
    [TestFixture()]
    public class PolygonTest
    {
        [Test()]
        public void CanInitFromPoints()
        {
            Polygon p = new Polygon(
                   new List<Point>
                {
                    new Point(1, 2),
                    new Point(2, 2),
                    new Point(3, 4)
                }
               );

            Assert.AreEqual(3, p.Vertices.Count);
        }



        [Test()]
        public void BoundingBoxIdenticalWithPolygonIfIsRectangle()
        {
            Polygon p = new Polygon(
                            new List<Point>
                {
                    new Point(0, 0),
                    new Point(0, 1),
                    new Point(1, 1),
                    new Point(1, 0),
                }
                        );

            BoundingBox r = p.GetBoundingBox();

            Assert.AreEqual(0.0, r.Bottom);
            Assert.AreEqual(1.0, r.Top);
            Assert.AreEqual(0.0, r.Left);
            Assert.AreEqual(1.0, r.Right);

        }

        [Test()]
        public void BoundingBoxCorrectForTriangle()
        {
            Polygon p = new Polygon(
                            new List<Point>
                {
                    new Point(0, 0),
                    new Point(0.8, 1),
                    new Point(1, 1),
                }
                        );

            BoundingBox r = p.GetBoundingBox();

            Assert.AreEqual(0.0, r.Bottom);
            Assert.AreEqual(1.0, r.Top);
            Assert.AreEqual(0.0, r.Left);
            Assert.AreEqual(1.0, r.Right);
        }

        [Test()]
        public void CenterOfMassIsZeroForSimplePolygon()
        {
            Polygon p = new Polygon(
                            new List<Point>
                {
                    new Point(-1, 1),
                    new Point(-1, -1),
                    new Point(1, -1),
                    new Point(1, 1),
                }
                        );

            Point com = p.CenterOfMass();
            Assert.AreEqual(0, com.X);
            Assert.AreEqual(0, com.Y);
        }

        [Test()]
        public void CenterOfMassIsZeroForSimplePolygon2()
        {
            Polygon p = new Polygon(
                            new List<Point>
                {
                    new Point(0, 1),
                    new Point(0, 0),
                    new Point(2, 0),
                    new Point(2, 1),
                }
                        );

            Point com = p.CenterOfMass();
            Assert.AreEqual(1, com.X);
            Assert.AreEqual(0.5, com.Y);
        }

        [Test()]
        public void ShrinkKeepsCenterOfMassConstant()
        {
            Polygon p = new Polygon(
                            new List<Point>
                {
                    new Point(0, 0),
                    new Point(0.8, 1),
                    new Point(1, 1),
                }
                        );
            var oldCenter = p.CenterOfMass();

            Polygon q = p.Scale(0.4);
            var newCenter = q.CenterOfMass();

            Assert.AreEqual(oldCenter.X, newCenter.X);
            Assert.AreEqual(oldCenter.Y, newCenter.Y);
        }

        [Test()]
        public void ShrinkKeepsCenterOfMassConstant2()
        {
            Polygon p = new Polygon(
                            new List<Point>
                {
                    new Point(-2, -2),
                    new Point(-2, 2),
                    new Point(2, 2),
                    new Point(2, -2),
                }
                        );
            var oldCenter = p.CenterOfMass();

            Polygon q = p.Scale(1);
            var newCenter = q.CenterOfMass();

            Assert.AreEqual(oldCenter.X, newCenter.X);
            Assert.AreEqual(oldCenter.Y, newCenter.Y);
        }

        [Test()]
        public void ShrinkReducesDistanceOfVertices()
        {
            Polygon p = new Polygon(
                            new List<Point>
                {
                    new Point(0, 0),
                    new Point(0.8, 1),
                    new Point(1, 1),
                }
                        );
            double oldDistance = p.Vertices[0].Distance(p.Vertices[1]);

            Polygon q = p.Scale(0.4);
            double newDistance = q.Vertices[0].Distance(q.Vertices[1]);

            Assert.IsTrue(newDistance < oldDistance);
        }
    }
}

