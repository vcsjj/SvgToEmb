using System;
using System.Collections.Generic;

namespace ShapeLib
{
    public class AngleStepper : IStepper
    {
        private readonly ColorTranslation ct;
        private readonly Polygon p;

        public AngleStepper(Polygon p, ColorTranslation ct)
        {
            this.p = p;
            this.ct = ct;
        }

        public System.Collections.Generic.List<Step> CalculateSteps()
        {
            var rList = new List<Point>();
            foreach (var p in this.p.Vertices)
            {
                var rotated = p.Rotate(this.ct.StepAngle);
                rList.Add(rotated);
            }

            Polygon p2 = new Polygon(rList);
            HorizontalStepper h = new HorizontalStepper(p2, this.ct);
            var rotatedResult = h.CalculateSteps();

            var result = new List<Step>();
            foreach (var p in rotatedResult)
            {
                var rotated = p.Point.Rotate(-this.ct.StepAngle);
                result.Add(new Step(p.Type, rotated));
            }

            return result;
        }

    }
}

