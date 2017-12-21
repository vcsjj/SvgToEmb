using System;
using System.Collections.Generic;

namespace ShapeLib
{
    public class AngleStepper : IStepper
    {
        private readonly double angle;

        private readonly Polygon p;

        public AngleStepper(Polygon p, double angle)
        {
            this.p = p;
            this.angle = angle;
        }

        public System.Collections.Generic.List<Step> CalculateSteps(double lineHeight)
        {
            var rList = new List<MyPoint>();
            foreach (var p in this.p.Vertices)
            {
                var rotated = p.Rotate(this.angle);
                rList.Add(rotated);
            }

            Polygon p2 = new Polygon(rList);
            HorizontalStepper h = new HorizontalStepper(p2);
            var rotatedResult = h.CalculateSteps(lineHeight);

            var result = new List<Step>();
            foreach (var p in rotatedResult)
            {
                var rotated = p.Point.Rotate(-this.angle);
                result.Add(new Step(p.Type, rotated));
            }

            return result;
        }

    }
}

