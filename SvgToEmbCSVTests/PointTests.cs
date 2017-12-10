using NUnit.Framework;
using System;
using ShapeLib;

namespace SvgToEmbCSVTests
{
	[TestFixture ()]
	public class PointTests
	{
		[Test ()]
		public void DistanceIsPositive ([Values(1,2,3,-33.5,0,double.PositiveInfinity)] double x, [Values(3.4,-3,22,0,double.PositiveInfinity)]double y)
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
	}
}

