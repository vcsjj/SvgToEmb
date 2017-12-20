using System;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Collections.Generic;
using ShapeLib;

namespace SvgToEmbCSV
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            OptionParser op = new OptionParser(args);
            var t = op.ParseForAction();
            var f = op.ParseForInputfile();
            switch (t)
            {
                case ActionType.WriteColormap:
                    ShowColormap(f);
                    break;

                case ActionType.CreateStitches:
                    CreateStitches(f);
                    break;
                default:
                    break;
            }

        }

        static void ShowColormap(string filename)
        {
            var r = XElement.Load(filename); 
            new SvgReader(r, null).Colors().ForEach(color => Console.WriteLine(color));
        }

        static void CreateStitches(string filename) 
        {
            var culture = System.Globalization.CultureInfo.InvariantCulture;
            var r = XElement.Load(filename);
            var fillMap = new Dictionary<string, Fill>
            {
                { "#aaaa00", new Fill(FillTypes.Vertical, 0.2) },
                { "#aa0000", new Fill(FillTypes.Horizontal, 0.01) }
            };

            new SvgReader(r, fillMap)
                .Read()
                .ForEach(WritePolygonToCsV);

            Console.WriteLine(CsvStepWriter.WriteClosingSequence());
        }

        static void WritePolygonToCsV(Polygon poly)
        {
            IStepper s;
            switch(poly.Fill.FillType) {
                case FillTypes.Vertical:
                    s = new VerticalStepper(poly);
                    break;
                case FillTypes.Horizontal:
                default:
                    s = new HorizontalStepper(poly);
                    break;
            }

            var stepsOrig = s.CalculateSteps(poly.Fill.StitchWidth);
            var steps = stepsOrig.Select(p => new Step(p.Type, new MyPoint(p.Point.X, p.Point.Y)));
            foreach (var item in steps)
            {
                Console.WriteLine(new CsvStepWriter(item).Write());
            }
        }
    }
}
