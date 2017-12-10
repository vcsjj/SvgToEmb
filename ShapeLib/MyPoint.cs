using System;

namespace ShapeLib
{
	public class MyPoint
	{
		public double Y {
			get;
			set;
		}

		public double X {
			get;
			set;
		}

		public MyPoint (double x, double y)
		{
			this.X = x;
			this.Y = y;
		}

		public double Distance (MyPoint p2)
		{
			return Math.Sqrt(Math.Pow(this.X-p2.X, 2) + Math.Pow(this.Y-p2.Y, 2));
		}

        public MyPoint Rotate(double degrees) 
        {
            var rad = degrees * Math.PI / 180;
            var newx = this.X * Math.Cos(rad) + this.Y * Math.Sin(rad);
            var newY = -this.X * Math.Sin(rad) + this.Y * Math.Cos(rad);

            return new MyPoint(newx, newY);
        }
	}
}

