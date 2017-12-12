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
			MyPoint p1 = new MyPoint (x, y);
			MyPoint p2 = new MyPoint (-5, 3);

			double d = p1.Distance (p2);
			Assert.IsTrue (d > 0);
		}

		[Test ()]
		public void DistanceIsMeaningful ()
		{
			MyPoint p1 = new MyPoint (1, -1);
			MyPoint p2 = new MyPoint (-1, 1);

			double d = p1.Distance (p2);
			Assert.AreEqual (d, 2.0*Math.Sqrt(2));
		}

        [Test ()]
        public void AbsolutePositionAndRelativePositionAreEqual()
        {
            MyPoint p1 = new MyPoint (1, -1);

            Assert.AreEqual(p1.X, p1.XOriginal);
            Assert.AreEqual(p1.Y, p1.YOriginal);
        }

        [Test ()]
        public void AbsolutePositionAndRelativePositionAreEqualWithIdentityTransformation()
        {
            MyPoint p1 = new MyPoint(1, -1, new double[] {1, 0, 0, 1, 0, 0});

            Assert.AreEqual(p1.X, p1.XOriginal);
            Assert.AreEqual(p1.Y, p1.YOriginal);
        }

        [Test ()]
        public void YAxisFlip()
        {
            MyPoint p1 = new MyPoint(3, -14, new double[] {1, 0, 0, -1, 0, 0});

            Assert.AreEqual(p1.X, p1.XOriginal);
            Assert.AreEqual(p1.Y, -p1.YOriginal);
        }

        [Test ()]
        public void XAxisFlip()
        {
            MyPoint p1 = new MyPoint(3, -14, new double[] {-1, 0, 0, 1, 0, 0});

            Assert.AreEqual(p1.X, -p1.XOriginal);
            Assert.AreEqual(p1.Y, p1.YOriginal);
        }

        [Test ()]
        public void XYScaling()
        {
            MyPoint p1 = new MyPoint(3, -14, new double[] {3, 0, 0, 4, 0, 0});

            Assert.AreEqual(p1.X, 3*p1.XOriginal);
            Assert.AreEqual(p1.Y, 4*p1.YOriginal);
        }

        [Test ()]
        public void XYExchange()
        {
            MyPoint p1 = new MyPoint(3, -14, new double[] {0, 1, 1, 0, 0, 0});

            Assert.AreEqual(p1.X, p1.YOriginal);
            Assert.AreEqual(p1.Y, p1.XOriginal);
        }

        [Test ()]
        public void XYOffset()
        {
            MyPoint p1 = new MyPoint(3, -14, new double[] {1, 0, 0, 1, 2, -3});

            Assert.AreEqual(p1.X, p1.XOriginal + 2);
            Assert.AreEqual(p1.Y, p1.YOriginal - 3);
        }
	}
}

