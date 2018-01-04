using System;
using System.Collections.Generic;
using System.Linq;

namespace ShapeLib
{
	public class FillCalculator
    {        
        private readonly Polygon polygon;
        private readonly ColorTranslation colorTranslation;

        public FillCalculator(Polygon polygon, ColorTranslation ct)
        {
            this.polygon = polygon;
            this.colorTranslation = ct;
        }

        public List<Step> Calculate()
        {
            var polygon = this.polygon.MoveInside(this.colorTranslation.MoveInside);
            var l = new List<Step>();
            var firstVertex = polygon.Vertices[0];
            var bb = this.polygon.GetBoundingBox();
            l.Add(new Step(Step.StepType.Stitch, firstVertex));

            for (double y = bb.Top - this.colorTranslation.LineHeight; y > bb.Bottom; y -= this.colorTranslation.LineHeight)
            {
                l.AddRange(FindIntersections(y, polygon));
            }

            return new StepLengthTransformer(l, this.colorTranslation.MaxStepLength).AddInbetweenStitches();
        }

        public static Point FindIntersection(Point fromPoint, Point toPoint, double y)
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
                return new Point(fromPoint.X, y);

            double slope = (toPoint.Y - fromPoint.Y) / (toPoint.X - fromPoint.X);
            double offset = fromPoint.Y - fromPoint.X * slope;
            return new Point((y - offset) / slope, y);
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
                var intersectionWithThisLine = FillCalculator.FindIntersection(fromPoint, toPoint, y);
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

