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

        public abstract List<Step> CalculateFillSteps();
        public abstract List<Step> CalculateOutlineSteps();

        public static void ChangeFirstStepToJump(List<Step> stepsForOneLine)
        {
            stepsForOneLine[0] = new Step(Step.StepType.Jump, stepsForOneLine[0].Point);
        }
    }
}

