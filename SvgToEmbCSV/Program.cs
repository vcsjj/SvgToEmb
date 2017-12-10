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
                            l.Add(new MyPoint(x, y));
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

                    var stepsOrig = s.CalculateSteps(5);//.Select(p => new MyPoint(p.X/10.0, p.Y/10.0));
                    var avgx = 1000;
                    var avgy = 2500;
                    var steps = stepsOrig.Select(p => new Step(p.Type, new MyPoint(p.Point.X - avgx, p.Point.Y-avgy)));

                    Console.WriteLine(("#*#,#JUMP#,#" 
                        + steps.First().Point.X.ToString("F4", culture) + "#,#"
                        + steps.First().Point.Y.ToString("F4", culture) + "#").Replace('#', '"'));
                    bool firstLoop = true;
                    foreach (var item in steps)
                    {
                        if (firstLoop)
                        {
                            firstLoop = false;
                        }
                        else
                        {
                            Console.WriteLine(("#*#,#STITCH#,#"
                                + item.Point.X.ToString("F4", culture) + "#,#"
                                + item.Point.Y.ToString("F4", culture) + "#").Replace('#', '"'));    
                        }
                    }
                }
            }

            Console.WriteLine(("#*#,#JUMP#,#0.0#,#0.0#").Replace('#', '"'));
            Console.WriteLine(("#*#,#END#,#0.0#,#0.0#").Replace('#', '"'));
        }
    }
}
