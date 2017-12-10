using System;
using System.Collections.Generic;
using System.Linq;

namespace ShapeLib
{
    public class HorizontalStepper : IStepper
    {
        private readonly Polygon p;

        public HorizontalStepper(Polygon p)
        {
            this.p = p;
        }


        public List<Step> CalculateSteps(double lineHeight)
        {
            var l = new List<Step>();
            var firstVertex = this.p.GetTopLeft();
            var bb = this.p.GetBoundingBox();
            l.Add(new Step(Step.StepType.Jump, firstVertex));

            for (double y = bb.Top-lineHeight; y > bb.Bottom; y -= lineHeight)
            {
                List<Step> t = FindIntersections(y, this.p);
                l.AddRange(t);
            }

            return l;
        }

        public static MyPoint FindIntersection(MyPoint fromPoint, MyPoint toPoint, double y)
        {
            if (double.IsNaN(y))
                return null;
            
            double minY = Math.Min(fromPoint.Y, toPoint.Y);
            if (y < minY)
                return null;

            double maxY = Math.Max(fromPoint.Y, toPoint.Y);
            if (y > maxY)
                return null;

            if (maxY == minY)
                return fromPoint;

            if (fromPoint.X == toPoint.X)
                return new MyPoint(fromPoint.X, y);

            double slope = (toPoint.Y - fromPoint.Y) / (toPoint.X - fromPoint.X);
            double offset = fromPoint.Y - fromPoint.X * slope;
            return new MyPoint((y - offset) / slope, y);
        }

        public static List<Step> FindIntersections(double y, Polygon poly)
        {
            var result = new List<Step>();
            for (int lineFromIndex = 0; lineFromIndex < poly.Vertices.Count; lineFromIndex++)
            {
                int lineToIndex = lineFromIndex + 1;
                if (lineFromIndex == poly.Vertices.Count - 1)
                {
                    lineToIndex = 0;
                }

                var fromPoint = poly.Vertices[lineFromIndex];
                var toPoint = poly.Vertices[lineToIndex];
                var intersectionWithThisLine = HorizontalStepper.FindIntersection(fromPoint, toPoint, y);
                if (intersectionWithThisLine != null)
                {
                    var step = new Step(Step.StepType.Stitch, intersectionWithThisLine);
                    result.Add(step);
                }
            }

            List<Step> sortedByX = result
                .Select(step => new KeyValuePair<Step, double>(step, step.Point.X))
                .OrderBy(x => x.Value)
                .Select<KeyValuePair<Step, double>, Step>(kvp => kvp.Key)
                .ToList();

            return sortedByX;
        }
    }
}

