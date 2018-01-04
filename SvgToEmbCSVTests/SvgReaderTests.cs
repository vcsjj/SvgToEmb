using NUnit.Framework;
using System;
using System.Linq;
using SvgToEmbCSV;
using ShapeLib;
using System.Collections.Generic;
using FileIO;

namespace SvgToEmbCSVTests
{
    [TestFixture()]
    public class SvgReaderTests
    {
        [Test()]
        public void EmptyElementHasZeroPolygons()
        {
            var reader = new SvgReader(new System.Xml.Linq.XElement("test"));
            IEnumerable<Polygon> lp = reader.ExtractPolygons();

            Assert.AreEqual(0, lp.Count());
        }

        [Test()]
        public void XmlWithOneElementHasOnePolygon()
        {
            var reader = new SvgReader(System.Xml.Linq.XElement.Parse(
                                 "  <svg><polygon\n     points=\"1088.95,2511.82 1169.85,2570.6 1138.95,2475.49 1058.05,2416.71 \"  transform=\"matrix(0.05880683,0,0,0.0624649,-51.602926,-159.78388)\" /></svg>"
                             ));

            IEnumerable<Polygon> lp = reader.ExtractPolygons();

            Assert.AreEqual(1, lp.Count());
        }

        [Test()]
        public void XmlWithOneElementHasCorrectPolygon()
        {
            var reader = new SvgReader(System.Xml.Linq.XElement.Parse(
                "  <svg><polygon\n     points=\"1088.95,2511.82 1169.85,2570.6 1138.95,2475.49 1058.05,2416.71 \" /></svg>"
            ));

            IEnumerable<Polygon> lp = reader.ExtractPolygons();

            Assert.IsTrue(lp.Where(poly => poly.Vertices.Where(pt => Math.Abs(pt.X - 1058.05) < 1e-8 && Math.Abs(pt.Y - 2416.71) < 1e-8).Any()).Any());
        }

        [Test()]
        public void XmlWithTwoElementHasTwoPolygons()
        {
            var reader = new SvgReader(System.Xml.Linq.XElement.Parse(
                "<svg>"
                + "<polygon\n     points=\"1,2 3,4 5,6 \"  />"
                + "<polygon\n     points=\"1088.95,2511.82 1169.85,2570.6 1138.95,2475.49 1058.05,2416.71 \"/></svg>"
            ));

            IEnumerable<Polygon> lp = reader.ExtractPolygons();

            Assert.AreEqual(2, lp.Count());
        }

        [Test()]
        public void XmlWithTwoNestedElementsHasTwoPolygons()
        {
            var reader = new SvgReader(System.Xml.Linq.XElement.Parse(
                "<svg>"
                + "<polygon\n     points=\"1,2 3,4 5,6 \"/>"
                + "<group>"
                + "<polygon\n     points=\"3,4 5,6 7,8\"/>"
                + "</group>"
                + "</svg>"
            ));

            IEnumerable<Polygon> lp = reader.ExtractPolygons();

            Assert.AreEqual(2, lp.Count());
        }

        [Test()]
        public void SimpleColorToFillMap()
        {
            var source = System.Xml.Linq.XElement.Parse(
                             "<svg>"
                             + "<polygon\n     points=\"1,2 3,4 5,6 \" style=\"fill:#aaaa00;stroke:#000000;stroke-width:1\"/>"
                             + "</svg>"
                         );

            var reader = new SvgReader(source);

            IEnumerable<Polygon> lp = reader.ExtractPolygons();

            Assert.AreEqual("fill:#aaaa00", lp.First().Color);
        }

        [Test()]
        public void DefaultFillForUndefinedColor()
        {
            var source = System.Xml.Linq.XElement.Parse(
                "<svg>"
                + "<polygon\n     points=\"1,2 3,4 5,6 \" style=\"fill:#bcdaa0;stroke:#000000;stroke-width:1\"/>"
                + "</svg>"
            );

            var reader = new SvgReader(source);

            IEnumerable<Polygon> lp = reader.ExtractPolygons();

            Assert.AreEqual("fill:#bcdaa0", lp.First().Color);
        }

        [Test()]
        public void OneColorIsFound()
        {
            var source = System.Xml.Linq.XElement.Parse(
                "<svg>"
                + "<polygon\n     points=\"1,2 3,4 5,6 \" style=\"fill:#bbbbcc;stroke:#000000;stroke-width:1\"/>"
                + "</svg>"
            );

            var reader = new SvgReader(source);
            List<SvgObjectProperty> fills = reader.ReadObjectProperties().Where(s => s.Type == SvgPropertyType.Fill).ToList();
            Assert.AreEqual(1, fills.Count);
            Assert.AreEqual("fill:#bbbbcc", fills[0].Color);
        }

        [Test()]
        public void TwoSameColorsAreFoundOnlyOnce()
        {
            var source = System.Xml.Linq.XElement.Parse(
                "<svg>"
                + "<polygon\n     points=\"1,2 3,4 5,6 \" style=\"fill:#bbbbcc;stroke:#000000;stroke-width:1\"/>"
                + "<polygon\n     points=\"54,2 3,4 5,6 \" style=\"stroke:#000000;fill:#bbbbcc;stroke-width:1\"/>"
                + "</svg>"
            );

            var reader = new SvgReader(source);
            List<SvgObjectProperty> fills = reader.ReadObjectProperties().Where(s => s.Type == SvgPropertyType.Fill).ToList();
            Assert.AreEqual(1, fills.Count);
            Assert.AreEqual("fill:#bbbbcc", fills[0].Color);
        }

        [Test()]
        public void TwoColorsAreFound()
        {
            var source = System.Xml.Linq.XElement.Parse(
                "<svg>"
                + "<polygon\n     points=\"1,2 3,4 5,6 \" style=\"fill:#bbbbcc;stroke:#000000;stroke-width:1\"/>"
                + "<polygon\n     points=\"54,2 3,4 5,6 \" style=\"stroke:#000000;fill:#aabbcc;stroke-width:1\"/>"
                + "</svg>"
            );

            var reader = new SvgReader(source);
            List<SvgObjectProperty> fills = reader.ReadObjectProperties().Where(s => s.Type == SvgPropertyType.Fill).ToList();
            Assert.AreEqual(2, fills.Count);
            Assert.AreEqual("fill:#bbbbcc", fills[0].Color);
            Assert.AreEqual("fill:#aabbcc", fills[1].Color);
        }

        [Test()]
        public void NestedColorsAreFound()
        {
            var source = System.Xml.Linq.XElement.Parse(
                "<svg>"
                + "<group>"
                + "<polygon\n     points=\"54,2 3,4 5,6 \" style=\"stroke:#000000;fill:#aabbdd;stroke-width:1\"/>"
                + "<polygon\n     points=\"1,2 3,4 5,6 \" style=\"fill:#bbbbcc;stroke:#000000;stroke-width:1\"/>"
                + "</group>"
                + "<polygon\n     points=\"54,2 3,4 5,6 \" style=\"stroke:#000000;fill:#aabbcc;stroke-width:1\"/>"
                + "</svg>"
            );

            var reader = new SvgReader(source);
            List<SvgObjectProperty> fills = reader.ReadObjectProperties().Where(s => s.Type == SvgPropertyType.Fill).ToList();
            Assert.AreEqual(3, fills.Count);
            Assert.AreEqual("fill:#aabbdd", fills[0].Color);
            Assert.AreEqual("fill:#bbbbcc", fills[1].Color);
            Assert.AreEqual("fill:#aabbcc", fills[2].Color);
        }

        [Test()]
        public void OneStrokeIsFound()
        {
            var source = System.Xml.Linq.XElement.Parse(
                "<svg>"
                + "<polygon\n     points=\"1,2 3,4 5,6 \" style=\"fill:#bbbbcc;stroke:#00aaff;stroke-width:1\"/>"
                + "</svg>"
            );

            var reader = new SvgReader(source);
            List<SvgObjectProperty> strokes = reader.ReadObjectProperties().Where(s => s.Type == SvgPropertyType.Stroke).ToList();
            Assert.AreEqual(1, strokes.Count);
            Assert.AreEqual("stroke:#00aaff", strokes[0].Color);
        }
    }
}

