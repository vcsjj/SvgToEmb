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
            var numberStyle = System.Globalization.NumberStyles.Any;
            var r = XElement.Load("/home/robert/penrose3.svg");
            List<Polygon> lp = new List<Polygon>();

            foreach (var l1 in r.Elements())
            {
                string pointsString = l1.Attribute("points")?.Value;
                string styleString = l1.Attribute("style")?.Value;
                string transformationString = l1.Attribute("transform")?.Value;

                bool isTypeA = false;
                if (!string.IsNullOrEmpty(styleString))
                {
                    if (styleString.Contains("fill:#aaaa00"))
                    {
                        isTypeA = true;
                    }
                }

                double[] transformation = new double[] { 1, 0, 0, 1, 0, 0 };
                if (!string.IsNullOrEmpty(transformationString))
                {
                    transformation = new TransformationParser(transformationString).GetTransformation();
                }

                if (!string.IsNullOrEmpty(pointsString))
                {
                    string[] positionList = pointsString.Split(' ');
                    List<MyPoint> l = new List<MyPoint>();
                    foreach (string position in positionList)
                    {
                        string[] xy = position.Split(',');
                        double x = 0.0;
                        double y = 0.0;
                        if(double.TryParse(xy[0], numberStyle, culture, out x) && double.TryParse(xy[1],numberStyle, culture, out y) )
                        {
                            l.Add(new MyPoint(x, y, transformation));
                        }
                    }

                    lp.Add(new Polygon(l, isTypeA));

                }
            }

            foreach (var item in lp)
            {
                WritePolygonToCsV(item);
            }

            Console.WriteLine(CsvStepWriter.WriteClosingSequence());
        }

        static void WritePolygonToCsV(Polygon poly)
        {
            IStepper s;
            if (poly.IsTypeA)
            {
                s = new HorizontalStepper(poly);
            }
            else
            {
                s = new VerticalStepper(poly);
            }
            var stepsOrig = s.CalculateSteps(0.2);
            var steps = stepsOrig.Select(p => new Step(p.Type, new MyPoint(p.Point.X, p.Point.Y)));
            foreach (var item in steps)
            {
                Console.WriteLine(new CsvStepWriter(item).Write());
            }
        }
    }
}
