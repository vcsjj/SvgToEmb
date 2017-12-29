using System;
using System.Collections.Generic;

namespace ShapeLib
{
    public class LineWidener
    {
        private readonly Point second;
        private readonly Point first;
        private readonly string color;
        private readonly string stroke;

        public LineWidener(Point first, Point second, string color = "", string stroke = "")
        {
            this.first = first;
            this.second = second;
            this.color = color;
            this.stroke = stroke;
        }

        public Polygon Widen(double d)
        {
            var halfd = d / 2.0;
            var normal = this.FindNormal();

            var list = new List<Point> {
                this.MoveAlongNormal(this.first, normal, halfd),
                this.MoveAlongNormal(this.second, normal, halfd),
                this.MoveAlongNormal(this.second, normal, -halfd),
                this.MoveAlongNormal(this.first, normal, -halfd),
            };

            return new Polygon(list, color, stroke, false);
        }

        public Point FindNormal() 
        {
            if (this.first.Y - this.second.Y != 0)
            {
                double x = 1.0;
                double y = -(this.first.X - this.second.X) / (this.first.Y - this.second.Y);

                double length = Math.Sqrt(x * x + y * y);

                return new Point(x / length, y / length);
            }
            else if (this.first.X - this.second.X != 0)
            {
                double y = 1.0;
                double x = -(this.first.Y - this.second.Y) / (this.first.X - this.second.X);

                double length = Math.Sqrt(x * x + y * y);

                return new Point(x / length, y / length);
            }
            else
            {
                return new Point(0, 0);
            }
        }

        private Point MoveAlongNormal(Point p, Point normal, double d)
        {
            return new Point(p.X + d * normal.X, p.Y + d * normal.Y);
        }
    }
}

