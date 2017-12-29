using System;
using ShapeLib;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace FileIO
{
    public class SvgReader
    {
        private const string defaultFill = "fill:#000000";
        private const string defaultStroke = "";
        private readonly System.Xml.Linq.XElement source;

        public const string FillIdentifier = "fill";
        public const string StrokeIdentifier = "stroke";
      
        public SvgReader(System.Xml.Linq.XElement xElement)
        {
            this.source = xElement;
        }

        public List<Polygon> ExtractPolygons()
        {
            return this.ExtractPolygons(this.source.Elements());
        }

        public List<SvgObjectProperty> ReadObjectProperties()
        {
            return this.ReadObjectProperties(this.source.Elements());
        }

        private List<SvgObjectProperty> ReadObjectProperties(IEnumerable<XElement> elements)
        {
            var lc = new List<SvgObjectProperty>();
            foreach (XElement l1 in elements)
            {
                if (l1.Name.LocalName == "polygon")
                {
                    lc.AddRange(AddDescriptionIfAny(l1, SvgPropertyType.Fill, FillIdentifier).Where(x => !lc.Contains(x)));
                    lc.AddRange(AddDescriptionIfAny(l1, SvgPropertyType.Stroke, StrokeIdentifier).Where(x => !lc.Contains(x)));
                }
                else
                {
                    lc.AddRange(this.ReadObjectProperties(l1.Elements()));
                }
            }

            return lc;
        }

        List<SvgObjectProperty> AddDescriptionIfAny(XElement l1, SvgPropertyType type, string identifier)
        {
            var list = new List<SvgObjectProperty>();
            var p = ExtractDescription(l1, type, identifier);
            if (p != null)
            {
                list.Add(p);
            }

            return list;
        }

        List<Polygon> ExtractPolygons(IEnumerable<XElement> elements)
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
                    lp.AddRange(this.ExtractPolygons(l1.Elements()));
                }
            }

            return lp;
        }

        SvgObjectProperty ExtractDescription(XElement l1, SvgPropertyType type, string identifier)
        {
            string styleString = l1.Attribute("style")?.Value;
            if (styleString != null)
            {
                return new SvgObjectProperty
                { 
                    Color = styleString.Split(';').Where(s => s.StartsWith(identifier)).DefaultIfEmpty(string.Empty).First(),
                    Type = type
                };
            }

            return null;
        }

        static string ColorFromStyle(string styleString) => styleString?.Split(';')
            .DefaultIfEmpty(FillIdentifier + ":" + defaultFill)
            .First(s => s.ToLower()
                .StartsWith(FillIdentifier))
            ?? defaultFill;

        static string StrokeFromStyle(string styleString) => styleString?.Split(';')
            .DefaultIfEmpty(StrokeIdentifier + ":" + defaultStroke)
            .First(s => s.ToLower()
                .StartsWith(StrokeIdentifier + ":"))
            ?? defaultStroke;

        Polygon ExtractPolygon(XElement l1)
        {
            var culture = System.Globalization.CultureInfo.InvariantCulture;
            var numberStyle = System.Globalization.NumberStyles.Any;
            Polygon ret = null;
            string pointsString = l1.Attribute("points")?.Value;
            string styleString = l1.Attribute("style")?.Value;
            string transformationString = l1.Attribute("transform")?.Value;

            string color = ColorFromStyle(styleString);
            string stroke = StrokeFromStyle(styleString);
            
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
                ret = new Polygon(l, color, stroke);
            }

            return ret;
        }
    }
}

