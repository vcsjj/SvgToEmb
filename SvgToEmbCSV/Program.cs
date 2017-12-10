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
            var r = XElement.Load("/home/robert/penrose4.svg");
            foreach (var l1 in r.Elements())
            {
                string pointsString = l1.Attribute("points")?.Value;
                string styleString = l1.Attribute("style")?.Value;
                bool isTypeA = false;
                if (!string.IsNullOrEmpty(styleString))
                {
                    if (styleString.Contains("fill:#aaaa00"))
                    {
                        isTypeA = true;
                    }
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
                            l.Add(new MyPoint(x/2.0, y/2.0));
                        }
                    }

                    var poly = new Polygon(l);
                    IStepper s;
                    if (isTypeA)
                    {
                        s = new HorizontalStepper(poly);
                    }
                    else
                    {
                        s = new VerticalStepper(poly);
                    }

                    var stepsOrig = s.CalculateSteps(2.5);//.Select(p => new MyPoint(p.X/10.0, p.Y/10.0));
                    var avgx = 500;
                    var avgy = 1400;
                    var steps = stepsOrig.Select(p => new Step(p.Type, new MyPoint(p.Point.X - avgx, p.Point.Y-avgy)));

                    foreach (var item in steps)
                    {
                        Console.WriteLine(new CsvStepWriter(item).Write());
                    }
                }
            }

            Console.WriteLine(CsvStepWriter.WriteClosingSequence());
        }
    }
}
