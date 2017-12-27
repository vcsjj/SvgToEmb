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
            {
                foreach (string s in WriteStitchesForOnePolygonWithTranslations(colortranslations)(poly))
                {
                    yield return s;
                }
            }

            yield return CsvStepWriter.WriteClosingSequence();
        }

        private static Func<Polygon, IEnumerable<string>> WriteStitchesForOnePolygonWithTranslations(List<ColorTranslation> colortranslations) 
        => poly 
        => WriteStitchesForOnePolygon(poly, colortranslations);

        private static IEnumerable<string> WriteStitchesForOnePolygon(Polygon poly, List<ColorTranslation> colortranslations)
        {
            var matchingColortranslations = FindMatchingColorTranslationsOrDefault(poly.Color, colortranslations);

            foreach (var colortranslation in matchingColortranslations) 
            {
                IStepper s = new AngleStepper(poly, colortranslation);

                var stepsOrig = s.CalculateSteps();
                var steps = stepsOrig.Select(p => new Step(p.Type, new Point(p.Point.X, p.Point.Y)));
                foreach (var item in steps)
                {
                    yield return new CsvStepWriter(item).Write();
                }
            }
        }

        static IEnumerable<ColorTranslation> FindMatchingColorTranslationsOrDefault(string color, List<ColorTranslation> colortranslations)
        {
            return colortranslations.Where(ct => ct.Color == color).DefaultIfEmpty(new ColorTranslation());
        }
    }
}

