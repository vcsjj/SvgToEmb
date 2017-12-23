using System;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Collections.Generic;
using ShapeLib;
using FileIO;

namespace SvgToEmbCSV
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            OptionParser op = new OptionParser(args);
            var t = op.ParseForAction();
            var f = op.ParseForInputfile();
            var cm = op.ParseForColormap();
            switch (t)
            {
                case ActionType.WriteColormap:
                    ShowColormap(f);
                    break;

                case ActionType.CreateStitches:
                    CreateStitches(f, cm);
                    break;
                default:
                    break;
            }

        }

        static void ShowColormap(string filename)
        {
            Console.WriteLine(ColorTranslation.Header);

            var r = XElement.Load(filename); 
            new SvgReader(r).Colors().ForEach(color => Console.WriteLine(new ColorTranslation {Color = color, StepAngle = 0.0, StepWidth = 0.5, MoveInside = 0.0}));
        }

        static void CreateStitches(string filename, string colormap) 
        {
            var cmr = new ColorMapReader();

            var colortranslations = cmr.Read(TryReadLines(colormap));

            var culture = System.Globalization.CultureInfo.InvariantCulture;
            var r = XElement.Load(filename);
            var defaultMap = new List<ColorTranslation>
            {
                    new ColorTranslation{ Color = "#aaaa00", StepAngle = 90, StepWidth = 0.2 },
                    new ColorTranslation{ Color = "#aa0000", StepAngle =  0, StepWidth = 0.1 },
            };

            colortranslations = colortranslations.Count == 0 ? defaultMap : colortranslations;

            foreach (var poly in new SvgReader(r).Read())
            {
                foreach (string s in WritePolygonToCsVWithTranslations(colortranslations)(poly))
                {
                    Console.WriteLine(s);
                }
            }

            Console.WriteLine(CsvStepWriter.WriteClosingSequence());
        }

        static IEnumerable<string> TryReadLines(string filename)
        {
            IEnumerable<string> enumerable;
            try
            {
                enumerable = System.IO.File.ReadLines(filename);
            }
            catch (Exception)
            {
                enumerable = new List<string>();
            }
            return enumerable;
        }

        static Func<Polygon, IEnumerable<string>> WritePolygonToCsVWithTranslations(List<ColorTranslation> colortranslations) 
            => poly 
            => WritePolygon(poly, colortranslations);

        static IEnumerable<string> WritePolygon(Polygon poly, List<ColorTranslation> colortranslations)
        {
            var colortranslation = colortranslations
                .Where(ct => ct.Color == poly.Color)
                .DefaultIfEmpty(ColorTranslation.Default)
                .First();
            
            IStepper s = new AngleStepper(poly, colortranslation.StepAngle);

            var stepsOrig = s.CalculateSteps(colortranslation.StepWidth);
            var steps = stepsOrig.Select(p => new Step(p.Type, new MyPoint(p.Point.X, p.Point.Y)));
            foreach (var item in steps)
            {
                yield return new CsvStepWriter(item).Write();
            }
        }
    }
}
