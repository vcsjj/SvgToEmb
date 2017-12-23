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
            List<Polygon> lp = reader.Read();

            Assert.AreEqual(0, lp.Count);
        }

        [Test()]
        public void XmlWithOneElementHasOnePolygon()
        {
            var reader = new SvgReader(System.Xml.Linq.XElement.Parse(
                                 "  <svg><polygon\n     points=\"1088.95,2511.82 1169.85,2570.6 1138.95,2475.49 1058.05,2416.71 \"  transform=\"matrix(0.05880683,0,0,0.0624649,-51.602926,-159.78388)\" /></svg>"
                             ));

            List<Polygon> lp = reader.Read();

            Assert.AreEqual(1, lp.Count);
        }

        [Test()]
        public void XmlWithOneElementHasCorrectPolygon()
        {
            var reader = new SvgReader(System.Xml.Linq.XElement.Parse(
                "  <svg><polygon\n     points=\"1088.95,2511.82 1169.85,2570.6 1138.95,2475.49 1058.05,2416.71 \" /></svg>"
            ));

            List<Polygon> lp = reader.Read();

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

            List<Polygon> lp = reader.Read();

            Assert.AreEqual(2, lp.Count);
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

            List<Polygon> lp = reader.Read();

            Assert.AreEqual(2, lp.Count);
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

            List<Polygon> lp = reader.Read();

            Assert.AreEqual("#aaaa00", lp.First().Color);
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

            List<Polygon> lp = reader.Read();

            Assert.AreEqual("#bcdaa0", lp.First().Color);
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
            List<string> colors = reader.Colors();
            Assert.AreEqual(1, colors.Count);
            Assert.AreEqual("#bbbbcc", colors[0]);
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
            List<string> colors = reader.Colors();
            Assert.AreEqual(1, colors.Count);
            Assert.AreEqual("#bbbbcc", colors[0]);
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
            List<string> colors = reader.Colors();
            Assert.AreEqual(2, colors.Count);
            Assert.AreEqual("#bbbbcc", colors[0]);
            Assert.AreEqual("#aabbcc", colors[1]);
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
            List<string> colors = reader.Colors();
            Assert.AreEqual(3, colors.Count);
            Assert.AreEqual("#aabbdd", colors[0]);
            Assert.AreEqual("#bbbbcc", colors[1]);
            Assert.AreEqual("#aabbcc", colors[2]);
        }
    }
}

