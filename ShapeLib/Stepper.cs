using System;
using System.Collections.Generic;
using System.Linq;

namespace ShapeLib
{
    public abstract class Stepper : IStepper
    {
        public List<Step> CalculateAllSteps()
        {
            var outline = this.CalculateOutlineSteps();
            outline.RemoveAt(outline.Count - 1);
            var fill = this.CalculateFillSteps();
            outline.AddRange(fill);
            return outline;
        }

        public virtual List<Step> CalculateFillSteps()
        {
            throw new NotImplementedException();
        }
        public virtual List<Step> CalculateOutlineSteps()
        {
            throw new NotImplementedException();
        }
    }
}

