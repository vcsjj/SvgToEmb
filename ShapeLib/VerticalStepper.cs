using System;
using System.Collections.Generic;

namespace ShapeLib
{
    public class VerticalStepper : IStepper
    {
        private readonly Polygon p;

        public VerticalStepper(Polygon p)
        {
            this.p = p;
        }

        public List<Step> CalculateSteps(double lineHeight)
        {
            var rList = new List<MyPoint>();
            foreach (var p in this.p.Vertices)
            {
                var rotated = p.Rotate(90);
                rList.Add(rotated);
            }

            Polygon p2 = new Polygon(rList);
            HorizontalStepper h = new HorizontalStepper(p2);
            var rotatedResult = h.CalculateSteps(lineHeight);

            var result = new List<Step>();
            foreach (var p in rotatedResult)
            {
                var rotated = p.Point.Rotate(-90);
                result.Add(new Step(p.Type, rotated));
            }

            return result;
        }
    }
}

