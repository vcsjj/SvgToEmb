using System;

namespace ShapeLib
{
	public class Point
	{
        public double XOriginal
        {
            get;
        }

        public double YOriginal
        {
            get;
        }

		public double Y {
            get { return this.TransformY();}
		}

		public double X {
            get { return this.TransformX(); }
		}

        private readonly double[] transformation;

        public Point (double x, double y, double[] transformation = null)
		{
			this.XOriginal = x;
			this.YOriginal = y;

            if (transformation == null || transformation.Length != 6)
            {
                this.transformation = new double[] { 1, 0, 0, 1, 0, 0 };
            }
            else
            {
                this.transformation = transformation;
            }
		}

		public double Distance (Point p2)
		{
			return Math.Sqrt(Math.Pow(this.X-p2.X, 2) + Math.Pow(this.Y-p2.Y, 2));
		}

        public double Angle(Point p2)
        {
            return Math.Atan2(p2.Y - this.Y, p2.X - this.X) / Math.PI * 180;
        }

        public double Length()
        {
            return this.Distance(new Point(0, 0));
        }

        public Point Rotate(double degrees) 
        {
            var rad = degrees * Math.PI / 180;
            var newx = this.X * Math.Cos(rad) + this.Y * Math.Sin(rad);
            var newY = -this.X * Math.Sin(rad) + this.Y * Math.Cos(rad);

            return new Point(newx, newY);
        }

        private double TransformX()
        {
            double xo, yo;
            Transform(this.XOriginal, this.YOriginal, 
                this.transformation[0], 
                this.transformation[1],
                this.transformation[2],
                this.transformation[3],
                this.transformation[4],
                this.transformation[5], 
                out xo, out yo);
            return xo;
        }

        private double TransformY()
        {
            double xo, yo;
            Transform(this.XOriginal, this.YOriginal, 
                this.transformation[0], 
                this.transformation[1],
                this.transformation[2],
                this.transformation[3],
                this.transformation[4],
                this.transformation[5], 
                out xo, out yo);
            return yo;
        }

        private static void Transform(double x, double y, double a, double b, double c, double d, double e, double f, out double xo, out double yo) 
        {
            xo = x*a + y*c + e;
            yo = x*b + y*d + f;
        }

        public override bool Equals(object obj)
        {
            if (obj is Point)
            {
                var p2 = obj as Point;
                return p2.X == this.X && p2.Y == this.Y;
            }
            else
            {
                return false;
            }
        }
	}
}

