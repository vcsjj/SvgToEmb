using System;
using System.Collections.Generic;
using System.Linq;

namespace ShapeLib
{
    public class OutlineCalculator
    {
        Polygon polygon;
        ColorTranslation colorTranslation;

        public OutlineCalculator(Polygon polygon, ColorTranslation ct)
        {
            this.polygon = polygon;
            this.colorTranslation = ct;
        }

        public List<Step> Calculate()
        {
            var reducedPolygon = this.polygon.MoveInside(this.colorTranslation.MoveInside);
            if (this.colorTranslation.StepAngle == 0.0)
            {
                List<Step> steps = reducedPolygon
                .Vertices
                .Select(vertex => new Step(Step.StepType.Stitch, vertex))
                .ToList();

                steps.Add(new Step(Step.StepType.Jump, reducedPolygon.Vertices[0]));
                return new StepLengthTransformer(steps, this.colorTranslation.MaxStepLength).AddInbetweenStitches();
            }
            else
            {
                List<Step> result = new List<Step>();
                for (int i = 1; i<reducedPolygon.Vertices.Count;i++)
                {
                    var previous = reducedPolygon.Vertices[i - 1];
                    var current = reducedPolygon.Vertices[i];

                    result.AddRange(AddStepsForOneLine(reducedPolygon, previous, current));
                }

                result.AddRange(AddStepsForOneLine(reducedPolygon, reducedPolygon.Vertices.Last(), reducedPolygon.Vertices.First()));
                return result;
            }
        }

        List<Step> AddStepsForOneLine(Polygon reducedPolygon, Point previous, Point current)
        {
            // use current stroke specification for fill of widened polygon
            Polygon widened = new LineWidener(previous, current, reducedPolygon.Stroke, string.Empty).Widen(this.colorTranslation.MaxStepLength);
            double lineAngle = previous.Angle(current);
            var ctRotated = new ColorTranslation {
                Color = widened.Color,
                StepAngle = this.colorTranslation.StepAngle + lineAngle,
                MaxStepLength = this.colorTranslation.MaxStepLength,
                LineHeight = this.colorTranslation.LineHeight,
                MoveInside = 0
            };
            var angleStepper = new AngleStepper(widened, ctRotated);
            return angleStepper.CalculateFillSteps();
        }
    }
}

