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
            var culture = System.Globalization.CultureInfo.InvariantCulture;
            var r = XElement.Load("/home/robert/penrose3.svg");
            var fillMap = new Dictionary<string, Fill>
            {
                { "#aaaa00", new Fill(FillTypes.Vertical, 0.2) },
                { "#aa0000", new Fill(FillTypes.Horizontal, 0.01) }
            };

            new CsvReader(r, fillMap)
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
