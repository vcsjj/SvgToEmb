using System;
using System.Collections.Generic;
using System.Linq;

namespace ShapeLib
{
    public class HorizontalStepper : Stepper
    {
        private readonly Polygon p;
        private readonly ColorTranslation ct;
        private bool hasOutline = false;

        public HorizontalStepper(Polygon p, ColorTranslation ct)
        {
            this.p = p;
            this.ct = ct;
        }

        public override List<Step> CalculateFillSteps()
        {
            var polygon = this.p.MoveInside(this.ct.MoveInside);

            var l = new List<Step>();
            var firstVertex = polygon.Vertices[0];
            var bb = this.p.GetBoundingBox();
            l.Add(new Step(this.hasOutline ? Step.StepType.Stitch : Step.StepType.Jump, firstVertex));

            for (double y = bb.Top - this.ct.LineHeight; y > bb.Bottom; y -= this.ct.LineHeight)
            {
                List<Step> t = FindIntersections(y, polygon);
                l.AddRange(t);
            }

            return new StepLengthTransformer(l, this.ct.MaxStepLength).AddInbetweenStitches();
        }

        public override List<Step> CalculateOutlineSteps()
        {
            this.hasOutline = true;
            List<Step> steps = this.p.Vertices
                                   .Select(vertex => new Step(Step.StepType.Stitch, vertex))
                                   .ToList();
            
            steps.Add(new Step(Step.StepType.Jump, this.p.Vertices[0]));
            return new StepLengthTransformer(steps, this.ct.MaxStepLength).AddInbetweenStitches();
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

