using NUnit.Framework;
using System;
using ShapeLib;

namespace SvgToEmbCSVTests
{
	[TestFixture ()]
	public class PointTests
	{
		[Test ()]
		public void DistanceIsPositive ([Values(1,2,3,-33.5,0)] double x, [Values(3.4,-3,22,0)]double y)
		{
			Point p1 = new Point (x, y);
			Point p2 = new Point (-5, 3);

			double d = p1.Distance (p2);
			Assert.IsTrue (d > 0);
		}

		[Test ()]
		public void DistanceIsMeaningful ()
		{
			Point p1 = new Point (1, -1);
			Point p2 = new Point (-1, 1);

			double d = p1.Distance (p2);
			Assert.AreEqual (d, 2.0*Math.Sqrt(2));
		}

        [Test ()]
        public void AbsolutePositionAndRelativePositionAreEqual()
        {
            Point p1 = new Point (1, -1);

            Assert.AreEqual(p1.X, p1.XOriginal);
            Assert.AreEqual(p1.Y, p1.YOriginal);
        }

        [Test ()]
        public void AbsolutePositionAndRelativePositionAreEqualWithIdentityTransformation()
        {
            Point p1 = new Point(1, -1, new double[] {1, 0, 0, 1, 0, 0});

            Assert.AreEqual(p1.X, p1.XOriginal);
            Assert.AreEqual(p1.Y, p1.YOriginal);
        }

        [Test ()]
        public void YAxisFlip()
        {
            Point p1 = new Point(3, -14, new double[] {1, 0, 0, -1, 0, 0});

            Assert.AreEqual(p1.X, p1.XOriginal);
            Assert.AreEqual(p1.Y, -p1.YOriginal);
        }

        [Test ()]
        public void XAxisFlip()
        {
            Point p1 = new Point(3, -14, new double[] {-1, 0, 0, 1, 0, 0});

            Assert.AreEqual(p1.X, -p1.XOriginal);
            Assert.AreEqual(p1.Y, p1.YOriginal);
        }

        [Test ()]
        public void XYScaling()
        {
            Point p1 = new Point(3, -14, new double[] {3, 0, 0, 4, 0, 0});

            Assert.AreEqual(p1.X, 3*p1.XOriginal);
            Assert.AreEqual(p1.Y, 4*p1.YOriginal);
        }

        [Test ()]
        public void XYExchange()
        {
            Point p1 = new Point(3, -14, new double[] {0, 1, 1, 0, 0, 0});

            Assert.AreEqual(p1.X, p1.YOriginal);
            Assert.AreEqual(p1.Y, p1.XOriginal);
        }

        [Test ()]
        public void XYOffset()
        {
            Point p1 = new Point(3, -14, new double[] {1, 0, 0, 1, 2, -3});

            Assert.AreEqual(p1.X, p1.XOriginal + 2);
            Assert.AreEqual(p1.Y, p1.YOriginal - 3);
        }
	}
}

