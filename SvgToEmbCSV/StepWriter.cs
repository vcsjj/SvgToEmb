using System;
using FileIO;
using ShapeLib;
using System.Collections.Generic;
using System.Linq;

namespace SvgToEmbCSV
{
    public static class StepWriter
    {
        public static IEnumerable<string> WriteStitches(List<ColorTranslation> colortranslations, List<Polygon> polygonList)
        {
            foreach (var poly in polygonList)
                foreach (string s in WriteStitchesForOnePolygonWithTranslations(colortranslations)(poly))
                    yield return s;

            yield return CsvStepWriter.WriteClosingSequence();
        }

        private static Func<Polygon, IEnumerable<string>> WriteStitchesForOnePolygonWithTranslations(List<ColorTranslation> colortranslations) 
        => poly 
        => WriteStitchesForOnePolygon(poly, colortranslations);

        private static List<string> WriteStitchesForOnePolygon(Polygon poly, List<ColorTranslation> colortranslations)
        {
            var matchingTranslations = FindMatchingTranslationsOrDefault(poly.Color, poly.Stroke, colortranslations);
            var allsteps = matchingTranslations
                .SelectMany(colortranslation => CreateStepsForOneColorTranslation(new AngleStepper(poly, colortranslation), colortranslation.IsStroke()))
                .ToList();
            
            Stepper.ChangeFirstStepToJump(allsteps);
            return allsteps
                .Select(step => new CsvStepWriter(step).Write())
                .ToList();
        }

        static List<Step> CreateStepsForOneColorTranslation(IStepper s, bool isStroke) =>
            (isStroke
                ? s.CalculateOutlineSteps() 
                : s.CalculateFillSteps())
                    .Select(p => new Step(p.Type, new Point(p.Point.X, p.Point.Y)))
                    .ToList();

        static IEnumerable<ColorTranslation> FindMatchingTranslationsOrDefault(string color, string stroke, List<ColorTranslation> colortranslations)
        {
            return colortranslations.Where(ct => ct.Color == color || ct.Color == stroke);
        }

        static IEnumerable<ColorTranslation> FindMatchingFillTranslationsOrDefault(string color, List<ColorTranslation> colortranslations)
        {
            return colortranslations.Where(ct => ct.Color == color).DefaultIfEmpty(new ColorTranslation());
        }

        static IEnumerable<ColorTranslation> FindMatchingStrokeTranslationsOrDefault(string color, List<ColorTranslation> colortranslations)
        {
            return colortranslations.Where(ct => ct.Color == color);
        }
    }
}

