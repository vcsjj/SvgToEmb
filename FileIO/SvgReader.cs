﻿using System;
using ShapeLib;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace FileIO
{
    public class SvgReader
    {
        private const string defaultColor = "#000000";
        private readonly System.Xml.Linq.XElement source;
      
        public SvgReader(System.Xml.Linq.XElement xElement)
        {
            this.source = xElement;
        }

        public List<Polygon> Read()
        {
            var lp = this.ReadElements(this.source.Elements());

            return lp;
        }

        public List<string> Colors()
        {
            return this.ReadFillColors(this.source.Elements());
        }

        private List<string> ReadFillColors(IEnumerable<XElement> elements)
        {
            var lc = new List<string>();
            foreach (XElement l1 in elements)
            {
                if (l1.Name.LocalName == "polygon")
                {
                    var p = ExtractFill(l1);
                    if (p != null)
                    {
                        if (!lc.Contains(p))
                        {
                            lc.Add(p);
                        }
                    }
                }
                else
                {
                    lc.AddRange(this.ReadFillColors(l1.Elements()));
                }
            }

            return lc;
        }

        List<Polygon> ReadElements(IEnumerable<XElement> elements)
        {

            var lp = new List<Polygon>();
            foreach (var l1 in elements)
            {
                if (l1.Name.LocalName == "polygon")
                {
                    var p = ExtractPolygon(l1);
                    if (p != null)
                    {
                        lp.Add(p);
                    }
                }
                else
                {
                    lp.AddRange(this.ReadElements(l1.Elements()));
                }
            }

            return lp;
        }

        string ExtractFill(XElement l1)
        {
            string styleString = l1.Attribute("style")?.Value;
            if (styleString != null)
            {
                return styleString.Split(';').Where(s => s.StartsWith("fill")).Select(s => s.Substring(5)).First();
            }

            return string.Empty;
        }

        static string ColorFromStyle(string styleString) => styleString?.Split(';')
            .DefaultIfEmpty("fill:" + defaultColor)
            .First(s => s.ToLower()
                .StartsWith("fill:"))
            ?.Substring(5) ?? defaultColor;

        Polygon ExtractPolygon(XElement l1)
        {
            var culture = System.Globalization.CultureInfo.InvariantCulture;
            var numberStyle = System.Globalization.NumberStyles.Any;
            Polygon ret = null;
            string pointsString = l1.Attribute("points")?.Value;
            string styleString = l1.Attribute("style")?.Value;
            string transformationString = l1.Attribute("transform")?.Value;

            string color = ColorFromStyle(styleString);
            
            double[] transformation = new double[] {
                1,
                0,
                0,
                1,
                0,
                0
            };
            if (!string.IsNullOrEmpty(transformationString))
            {
                transformation = new TransformationParser(transformationString).GetTransformation();
            }
            if (!string.IsNullOrEmpty(pointsString))
            {
                string[] positionList = pointsString.Split(' ');
                List<Point> l = new List<Point>();
                foreach (string position in positionList)
                {
                    string[] xy = position.Split(',');
                    double x = 0.0;
                    double y = 0.0;
                    if (double.TryParse(xy[0], numberStyle, culture, out x) && double.TryParse(xy[1], numberStyle, culture, out y))
                    {
                        l.Add(new Point(x, y, transformation));
                    }
                }
                ret = new Polygon(l, color);
            }

            return ret;
        }
    }
}

