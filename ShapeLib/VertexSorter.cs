using System;
using System.Collections.Generic;
using System.Linq;

namespace ShapeLib
{
    public static class VertexSorter
    {
        public static Point GetTopLeft(List<Point> points)
        {
            var v = new List<Point>(points);
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

        public static int GetTopLeftIndex(List<Point> points)
        {
            Point tl = GetTopLeft(points);
            return points.FindIndex(p => p == tl);
        }
    }
}

