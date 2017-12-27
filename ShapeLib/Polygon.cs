﻿using System;
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

		private readonly List<Point> vertices = new List<Point>();

		public List<Point> Vertices {
			get {
				return vertices;
			}
		}

		public Polygon(int vertices)
		{
			for(int i = 0; i < vertices; i++) {
				this.Vertices.Add(new Point (0, 0));
			}
		} 

        public Polygon(IEnumerable<Point> points, string color = "")
		{
            this.Color = color;
			foreach (var item in points) 
			{
				this.Vertices.Add (item);
			}
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

        public Point GetTopLeft()
        {
            var v = new List<Point>(this.Vertices);
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

