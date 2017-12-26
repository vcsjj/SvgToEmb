using NUnit.Framework;
using System;
using ShapeLib;
using System.Collections.Generic;
using System.Windows;

namespace SvgToEmbCSVTests
{
	[TestFixture ()]
	public class PolygonTest
	{
		[Test ()]
		public void HasAtCorrectNumberOfVertices ()
		{
			Polygon p = new Polygon(4);
			Assert.AreEqual(4, p.Vertices.Count);
		}

		[Test ()]
		public void CanInitFromPoints()
		{
			Polygon p = new Polygon(
				new List<MyPoint> {
					new MyPoint(1,2),
					new MyPoint(2,2),
					new MyPoint(3,4)
				}
			);

			Assert.AreEqual(3, p.Vertices.Count);
		}

        [Test ()]
        public void IdentifiesTopLeftPoint1()
        {
            Polygon p = new Polygon(
                new List<MyPoint> {
                new MyPoint(0, 1),
                new MyPoint(1, 1),
                new MyPoint(1, 0),
                new MyPoint(0, 0),
            }
            );

            MyPoint tl = p.GetTopLeft();

            Assert.AreEqual(tl.X, 0);
            Assert.AreEqual(tl.Y, 1);
        }

        [Test ()]
        public void IdentifiesTopLeftPoint2()
        {
            Polygon p = new Polygon(
                new List<MyPoint> {
                new MyPoint(1, 0),
                new MyPoint(1, 1),
                new MyPoint(0, 0),
                new MyPoint(0, 1),
            }
            );

            MyPoint tl = p.GetTopLeft();

            Assert.AreEqual(tl.X, 0);
            Assert.AreEqual(tl.Y, 1);
        }

        [Test ()]
        public void IdentifiesTopLeftPoint3()
        {
            Polygon p = new Polygon(
                new List<MyPoint> {
                new MyPoint(0, 1),
                new MyPoint(-1, 0),
                new MyPoint(1, 0),
            }
            );

            MyPoint tl = p.GetTopLeft();

            Assert.AreEqual(tl.X, 0);
            Assert.AreEqual(tl.Y, 1);
        }

        [Test ()]
        public void IdentifiesTopLeftPoint4()
        {
            Polygon p = new Polygon(
                new List<MyPoint> {
                new MyPoint(1, 1),
                new MyPoint(-1, 0),
                new MyPoint(1, 0),
            }
            );

            MyPoint tl = p.GetTopLeft();

            Assert.AreEqual(tl.X, 1);
            Assert.AreEqual(tl.Y, 1);
        }

        [Test ()]
        public void BoundingBoxIdenticalWithPolygonIfIsRectangle()
        {
            Polygon p = new Polygon(
                new List<MyPoint> {
                new MyPoint(0, 0),
                new MyPoint(0, 1),
                new MyPoint(1, 1),
                new MyPoint(1, 0),
            }
            );

            BoundingBox r = p.GetBoundingBox();

            Assert.AreEqual(0.0, r.Bottom);
            Assert.AreEqual(1.0, r.Top);
            Assert.AreEqual(0.0, r.Left);
            Assert.AreEqual(1.0, r.Right);

        }

        [Test ()]
        public void BoundingBoxCorrectForTriangle()
        {
            Polygon p = new Polygon(
                new List<MyPoint> {
                new MyPoint(0, 0),
                new MyPoint(0.8, 1),
                new MyPoint(1, 1),
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
                new List<MyPoint> {
                new MyPoint(-1, 1),
                new MyPoint(-1, -1),
                new MyPoint(1, -1),
                new MyPoint(1, 1),
            }
            );

            MyPoint com = p.CenterOfMass();
            Assert.AreEqual(0, com.X);
            Assert.AreEqual(0, com.Y);
        }

        [Test()]
        public void CenterOfMassIsZeroForSimplePolygon2()
        {
            Polygon p = new Polygon(
                new List<MyPoint> {
                new MyPoint(0, 1),
                new MyPoint(0, 0),
                new MyPoint(2, 0),
                new MyPoint(2, 1),
            }
            );

            MyPoint com = p.CenterOfMass();
            Assert.AreEqual(1, com.X);
            Assert.AreEqual(0.5, com.Y);
        }

        [Test()]
        public void ShrinkKeepsCenterOfMassConstant()
        {
            Polygon p = new Polygon(
                new List<MyPoint> {
                new MyPoint(0, 0),
                new MyPoint(0.8, 1),
                new MyPoint(1, 1),
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
                new List<MyPoint> {
                new MyPoint(-2, -2),
                new MyPoint(-2, 2),
                new MyPoint(2, 2),
                new MyPoint(2, -2),
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
                new List<MyPoint> {
                new MyPoint(0, 0),
                new MyPoint(0.8, 1),
                new MyPoint(1, 1),
            }
            );
            double oldDistance = p.Vertices[0].Distance(p.Vertices[1]);

            Polygon q = p.Scale(0.4);
            double newDistance = q.Vertices[0].Distance(q.Vertices[1]);

            Assert.IsTrue(newDistance < oldDistance);
        }
	}
}

