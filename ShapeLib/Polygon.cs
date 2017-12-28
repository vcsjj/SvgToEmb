using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace ShapeLib
{
	public class Polygon
	{
        private readonly List<Point> vertices;

        public string Color
        {
            get;
        }

        public string Stroke
        {
            get;
        }

		public List<Point> Vertices {
			get {
				return vertices;
			}
		}

        public Polygon(IEnumerable<Point> points, string color = "", string stroke = "")
		{
            this.Color = color;
            this.Stroke = stroke;
            this.vertices = this.moveTopLeftToFirstIndex(points.ToList());
		}

        public Point CenterOfMass()
        {
            var cX = this.vertices.Select(v => v.X).Sum() / this.vertices.Count;
            var cY = this.vertices.Select(v => v.Y).Sum() / this.vertices.Count;

            return new Point(cX, cY);
        }

        public Polygon MoveInside(double d)
        {
            var bb = this.GetBoundingBox();
            double width = bb.Right - bb.Left;
            double height = bb.Top - bb.Bottom;

            double theLarger = width > height ? width : height;

            double scalingFactor = (theLarger - 2*d) / theLarger;

            return this.Scale(scalingFactor);
        }

        public Polygon Scale(double d)
        {
            var scaledVertices = new List<Point>();
            var com = this.CenterOfMass();
            foreach (var p in this.vertices)
            {

                double newX = (p.X - com.X) * d + com.X;
                double newY = (p.Y - com.Y) * d + com.Y;
                scaledVertices.Add(new Point(newX, newY));
            }

            return new Polygon(scaledVertices);
        }

        public BoundingBox GetBoundingBox()
        {
            double minX = this.Vertices.OrderBy(p => p.X).First().X;
            double maxX = this.Vertices.OrderBy(p => -p.X).First().X;
            double minY = this.Vertices.OrderBy(p => p.Y).First().Y;
            double maxY = this.Vertices.OrderBy(p => -p.Y).First().Y;

            return new BoundingBox(minX, minY, maxX - minX, maxY - minY);
        }

        private List<Point> moveTopLeftToFirstIndex(List<Point> points)
        {
            int tli = VertexSorter.GetTopLeftIndex(points);
            if (tli == 0)
                return points;

            var shifted = new List<Point>();
            for (int i = 0; i < points.Count; i++)
            {
                shifted.Add(points[(i + tli) % points.Count]); 
            }

            return shifted;
        }
	}
}

