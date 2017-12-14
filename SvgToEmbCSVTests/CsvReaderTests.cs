﻿using NUnit.Framework;
using System;
using System.Linq;
using SvgToEmbCSV;
using ShapeLib;
using System.Collections.Generic;

namespace SvgToEmbCSVTests
{
    [TestFixture()]
    public class CsvReaderTests
    {
        [Test()]
        public void EmptyElementHasZeroPolygons()
        {
            var reader = new CsvReader(new System.Xml.Linq.XElement("test"));
            List<Polygon> lp = reader.Read();

            Assert.AreEqual(0, lp.Count);
        }

        [Test()]
        public void XmlWithOneElementHasOnePolygon()
        {
            var reader = new CsvReader(System.Xml.Linq.XElement.Parse(
                                 "  <svg><polygon\n     points=\"1088.95,2511.82 1169.85,2570.6 1138.95,2475.49 1058.05,2416.71 \"  transform=\"matrix(0.05880683,0,0,0.0624649,-51.602926,-159.78388)\" /></svg>"
                             ));

            List<Polygon> lp = reader.Read();

            Assert.AreEqual(1, lp.Count);
        }

        [Test()]
        public void XmlWithOneElementHasCorrectPolygon()
        {
            var reader = new CsvReader(System.Xml.Linq.XElement.Parse(
                "  <svg><polygon\n     points=\"1088.95,2511.82 1169.85,2570.6 1138.95,2475.49 1058.05,2416.71 \" /></svg>"
            ));

            List<Polygon> lp = reader.Read();

            Assert.IsTrue(lp.Where(poly => poly.Vertices.Where(pt => Math.Abs(pt.X - 1058.05) < 1e-8 && Math.Abs(pt.Y - 2416.71) < 1e-8).Any()).Any());
        }

        [Test()]
        public void XmlWithTwoElementHasTwoPolygons()
        {
            var reader = new CsvReader(System.Xml.Linq.XElement.Parse(
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
            var reader = new CsvReader(System.Xml.Linq.XElement.Parse(
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
            var fillMap = new Dictionary<string, Fill>
            {
                { "#aaaa00", new Fill(FillTypes.Horizontal, 0.2) }
            };

            var source = System.Xml.Linq.XElement.Parse(
                             "<svg>"
                             + "<polygon\n     points=\"1,2 3,4 5,6 \" style=\"fill:#aaaa00;stroke:#000000;stroke-width:1\"/>"
                             + "</svg>"
                         );

            var reader = new CsvReader(source, fillMap);

            List<Polygon> lp = reader.Read();

            Assert.AreEqual(FillTypes.Horizontal, lp.First().Fill.FillType);
        }

        [Test()]
        public void DefaultFillForUndefinedColor()
        {
            var fillMap = new Dictionary<string, Fill>
            {
                { "#aaaa00", new Fill(FillTypes.Vertical, 0.7) }
            };

            var source = System.Xml.Linq.XElement.Parse(
                "<svg>"
                + "<polygon\n     points=\"1,2 3,4 5,6 \" style=\"fill:#bcdaa0;stroke:#000000;stroke-width:1\"/>"
                + "</svg>"
            );

            var reader = new CsvReader(source, fillMap);

            List<Polygon> lp = reader.Read();

            Assert.AreEqual(FillTypes.Horizontal, lp.First().Fill.FillType);
            Assert.AreEqual(0.2, lp.First().Fill.StitchWidth);
        }

        [Test()]
        public void AdvancedColorToFillMap()
        {
            var fillMap = new Dictionary<string, Fill>
            {
                { "#aaaa00", new Fill(FillTypes.Horizontal, 0.2) },
                { "#bbbbcc", new Fill(FillTypes.Vertical, 0.3) }
            };

            var source = System.Xml.Linq.XElement.Parse(
                "<svg>"
                + "<polygon\n     points=\"1,2 3,4 5,6 \" style=\"fill:#bbbbcc;stroke:#000000;stroke-width:1\"/>"
                + "<polygon\n     points=\"1,2 3,4 5,6 \" style=\"fill:#aaaa00;stroke:#000000;stroke-width:1\"/>"
                + "</svg>"
            );

            var reader = new CsvReader(source, fillMap);

            List<Polygon> lp = reader.Read();

            Assert.AreEqual(FillTypes.Horizontal, lp[1].Fill.FillType);
            Assert.AreEqual(FillTypes.Vertical, lp[0].Fill.FillType);
            Assert.AreEqual(0.3, lp[0].Fill.StitchWidth);

        }
    }
}

