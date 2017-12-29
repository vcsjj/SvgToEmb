using System;
using System.Collections.Generic;

namespace ShapeLib
{
    public class AngleStepper : Stepper
    {
        private readonly ColorTranslation ct;
        private readonly Polygon p;


        public AngleStepper(Polygon p, ColorTranslation ct)
        {
            this.p = p;
            this.ct = ct;
        }

        public override List<Step> CalculateFillSteps()
        {
            var p2 = RotateForward();

            HorizontalStepper h = new HorizontalStepper(p2, this.ct);
            var rotatedResult = h.CalculateFillSteps();

            return RotateBackward(rotatedResult);
        }

        public override List<Step> CalculateOutlineSteps()
        {
            var p2 = RotateForward();

            HorizontalStepper h = new HorizontalStepper(p2, this.ct);
            var rotatedResult = h.CalculateOutlineSteps();

            return RotateBackward(rotatedResult);
        }

        Polygon RotateForward()
        {
            var rList = new List<Point>();
            foreach (var p in this.p.Vertices)
            {
                var rotated = p.Rotate(this.ct.StepAngle);
                rList.Add(rotated);
            }

            Polygon p2 = new Polygon(rList, this.p.Color, this.p.Stroke, false);
            return p2;
        }

        List<Step> RotateBackward(List<Step> rotatedResult)
        {
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

