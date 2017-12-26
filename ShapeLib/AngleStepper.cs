using System;
using System.Collections.Generic;

namespace ShapeLib
{
    public class AngleStepper : IStepper
    {
        private readonly double angle;
        private readonly double lineHeight;
        private readonly double maxStepLength;
        private readonly double moveInside;
        private readonly Polygon p;

        public AngleStepper(Polygon p, double angle, double lineHeight, double maxStepLength = double.PositiveInfinity, double moveInside = 0.0 )
        {
            this.p = p;
            this.angle = angle;
            this.lineHeight = lineHeight;
            this.maxStepLength = maxStepLength;
            this.moveInside = moveInside;
        }

        public System.Collections.Generic.List<Step> CalculateSteps()
        {
            var rList = new List<MyPoint>();
            foreach (var p in this.p.Vertices)
            {
                var rotated = p.Rotate(this.angle);
                rList.Add(rotated);
            }

            Polygon p2 = new Polygon(rList);
            HorizontalStepper h = new HorizontalStepper(p2, this.lineHeight, this.maxStepLength, this.moveInside);
            var rotatedResult = h.CalculateSteps();

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

