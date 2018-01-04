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

        private static System.Globalization.CultureInfo culture = System.Globalization.CultureInfo.InvariantCulture;
        private static System.Globalization.NumberStyles numberStyle = System.Globalization.NumberStyles.Any;
      
        public SvgReader(System.Xml.Linq.XElement xElement)
        {
            this.source = xElement;
        }

        public IEnumerable<Polygon> ExtractPolygons()
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
            foreach (XElement xelement in elements)
            {
                if (IsPolygon(xelement))
                {
                    lc.AddRange(ExtractDescription(xelement, SvgPropertyType.Fill, FillIdentifier).Where(x => !lc.Contains(x)));
                    lc.AddRange(ExtractDescription(xelement, SvgPropertyType.Stroke, StrokeIdentifier).Where(x => !lc.Contains(x)));
                }
                else
                {
                    lc.AddRange(this.ReadObjectProperties(xelement.Elements()));
                }
            }

            return lc;
        }

        private static bool IsPolygon(XElement p) => p.Name.LocalName == "polygon";

        private IEnumerable<Polygon> ExtractPolygons(IEnumerable<XElement> elements)
        {
            return elements.SelectMany(xelement => IsPolygon(xelement)
                ? ExtractPolygon(xelement)
                : this.ExtractPolygons(xelement.Elements()));
        }

        IEnumerable<SvgObjectProperty> ExtractDescription(XElement xelement, SvgPropertyType type, string identifier)
        {
            List<SvgObjectProperty> description = new List<SvgObjectProperty>();
            string styleString = xelement.Attribute("style")?.Value;
            if (styleString != null)
            {
                description.Add(new SvgObjectProperty
                { 
                    Color = styleString.Split(';').Where(s => s.StartsWith(identifier)).DefaultIfEmpty(string.Empty).First(),
                    Type = type
                });
            }

            return description;
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

        IEnumerable<Polygon> ExtractPolygon(XElement xelement)
        {
            List<Polygon> ret = new List<Polygon>();
            string pointsString = xelement.Attribute("points")?.Value;
            string styleString = xelement.Attribute("style")?.Value;
            string transformationString = xelement.Attribute("transform")?.Value;

            string color = ColorFromStyle(styleString);
            string stroke = StrokeFromStyle(styleString);

            var transformation = new TransformationParser(transformationString).GetTransformation();

            if (!string.IsNullOrEmpty(pointsString))
            {
                var points = pointsString
                    .Split(' ')
                    .Select(position => position.Split(','))
                    .SelectMany(positionParts => AddPoint(positionParts, transformation))
                    .ToList();
                
                ret.Add(new Polygon(points, color, stroke));
            }

            return ret;
        }

        static List<Point> AddPoint(string [] xy, double[] transformation)
        {
            List<Point> l = new List<Point>();

            double x = 0.0;
            double y = 0.0;

            if (double.TryParse(xy[0], numberStyle, culture, out x) && double.TryParse(xy[1], numberStyle, culture, out y))
            {
                l.Add(new Point(x, y, transformation));
            }

            return l;
        }
    }
}

