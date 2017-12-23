using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace ShapeLib
{
	public class Polygon
	{
        public string Color
        {
            get;
        }

		private readonly List<MyPoint> vertices = new List<MyPoint>();

		public List<MyPoint> Vertices {
			get {
				return vertices;
			}
		}

		public Polygon(int vertices)
		{
			for(int i = 0; i < vertices; i++) {
				this.Vertices.Add(new MyPoint (0, 0));
			}
		} 

        public Polygon(IEnumerable<MyPoint> points, string color = "")
		{
            this.Color = color;
			foreach (var item in points) 
			{
				this.Vertices.Add (item);
			}
		}

        public MyPoint GetTopLeft()
        {
            var v = new List<MyPoint>(this.Vertices);
            v.Sort((p1, p2) =>
                {
                    if(p1.Y > p2.Y) return 1;
                    else if (p1.Y < p2.Y) return -1;
                    else 
                    {
                        if(p1.X < p2.X) return 1;
                        else if (p1.X > p2.X) return -1;
                        else return 0;
                    }
                });
            return v.Last();
        }

        public BoundingBox GetBoundingBox()
        {
            double minX = this.Vertices.OrderBy(p => p.X).First().X;
            double maxX = this.Vertices.OrderBy(p => -p.X).First().X;
            double minY = this.Vertices.OrderBy(p => p.Y).First().Y;
            double maxY = this.Vertices.OrderBy(p => -p.Y).First().Y;

            return new BoundingBox(minX, minY, maxX - minX, maxY - minY);
        }
	}
}

