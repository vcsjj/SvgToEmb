using System;
using System.Collections.Generic;

namespace ShapeLib
{
    public class StepLengthTransformer
    {
        private readonly List<Step> originalSteps;
        private readonly double maxStepLength;

        public StepLengthTransformer(List<Step> l, double dMax)
        {
            this.originalSteps = l;
            this.maxStepLength = dMax;
        }


        public List<Step> AddInbetweenStitches()
        {
            var result = new List<Step>();
            if(this.originalSteps.Count == 0) 
                return result;

            result.Add(this.originalSteps[0]);
            for (int i = 1; i < this.originalSteps.Count; i++)
            {
                var pre = this.originalSteps[i - 1];
                var cur = this.originalSteps[i];

                result.AddRange(AddStitchesBetweenTwoStepsIfNeccessary(pre, cur));
            }

            return result;
        }

        List<Step> AddStitchesBetweenTwoStepsIfNeccessary(Step pre, Step cur)
        {
            var result = new List<Step>();

            var distance = cur.Point.Distance(pre.Point);
            if (distance > this.maxStepLength)
            {
                result.AddRange(AddStitchesBetweenTwoSteps(pre, cur, distance));
            }
            else
            {
                result.Add(cur);
            }

            return result;
        }

        List<Step> AddStitchesBetweenTwoSteps(Step pre, Step cur, double stepDistance)
        {
            var result = new List<Step>();
            int parts = (int)Math.Ceiling(stepDistance / this.maxStepLength);
            for (int p = 1; p <= parts; p++)
            {
                var intermediatePoint = new Point(pre.X + p * (cur.X - pre.X) / parts, pre.Y + p * (cur.Y - pre.Y) / parts);
                result.Add(new Step(Step.StepType.Stitch, intermediatePoint));
            }

            return result;
        }
    }
}

