﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace ShapeLib
{
    public class HorizontalStepper : IStepper
    {
        private readonly Polygon p;
        private readonly double lineHeight;
        private readonly double maxStepLength;
        private readonly double moveInside;

        public HorizontalStepper(Polygon p, double lineHeight, double maxStepLength = double.PositiveInfinity, double moveInside = 0.0)
        {
            this.p = p;
            this.lineHeight = lineHeight;
            this.maxStepLength = maxStepLength;
            this.moveInside = moveInside;
        }


        public List<Step> CalculateSteps()
        {
            var polygon = this.p.MoveInside(this.moveInside);

            var l = new List<Step>();
            var firstVertex = polygon.GetTopLeft();
            var bb = this.p.GetBoundingBox();
            l.Add(new Step(Step.StepType.Trim, firstVertex));

            for (double y = bb.Top-lineHeight; y > bb.Bottom; y -= lineHeight)
            {
                List<Step> t = FindIntersections(y, polygon);
                l.AddRange(t);
            }

            return AddInbetweenStitches(l, this.maxStepLength);
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

        public static List<Step> AddInbetweenStitches(List<Step> l, double dMax)
        {
            
            var result = new List<Step>();
            if(l.Count == 0) 
                return result;
            
            result.Add(l[0]);
            for (int i = 1; i < l.Count; i++)
            {
                var pre = l[i - 1];
                var cur = l[i];

                var distance = cur.Point.Distance(pre.Point);
                if (distance > dMax)
                {
                    

                    int parts = (int)Math.Ceiling(distance / dMax);
                    for (int p = 1; p <= parts; p++)
                    {
                        var intermediatePoint = new MyPoint(pre.X + p * (cur.X - pre.X) / parts, pre.Y + p * (cur.Y - pre.Y) / parts);
                        result.Add(new Step(Step.StepType.Stitch, intermediatePoint));
                    }
                }
                else
                {
                    result.Add(cur);
                }
            }

            return result;
        }
    }
}

