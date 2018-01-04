using System;
using System.Collections.Generic;
using System.Linq;

namespace ShapeLib
{
    public class HorizontalStepper : Stepper
    {
        private readonly Polygon polygon;
        private readonly ColorTranslation ct;

        public HorizontalStepper(Polygon p, ColorTranslation ct)
        {
            this.polygon = p;
            this.ct = ct;
        }

        public override List<Step> CalculateFillSteps()
        {
            FillCalculator fc = new FillCalculator(this.polygon, this.ct);
            return fc.Calculate();

        }

        public override List<Step> CalculateOutlineSteps()
        {
            OutlineCalculator oc = new OutlineCalculator(this.polygon, this.ct);
            return oc.Calculate();
        }
    }
}

