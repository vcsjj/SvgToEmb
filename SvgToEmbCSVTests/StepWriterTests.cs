﻿using NUnit.Framework;
using System;
using ShapeLib;
using System.Collections.Generic;
using SvgToEmbCSV;
using System.Linq;

namespace SvgToEmbCSVTests
{
    [TestFixture()]
    public class StepWriterTests
    {
        string color = "#aabbcc";
        string stroke = "stroke:#aabbcc";
        string otherColor = "#aabbdd";

        [Test()]
        public void WritesPolygonTwiceForTwoColorspecs()
        {
            var polygons = new List<Polygon> { this.createDefaultPolygon() };
            List<ColorTranslation> ctSingle = this.createSingleColorTranslation();
            List<ColorTranslation> ctDouble = this.createDoubleColorTranslation();

            var stitchesSingle = StepWriter.WriteStitches(ctSingle, polygons);
            var stitchesDouble = StepWriter.WriteStitches(ctDouble, polygons);

            Assert.AreNotEqual(0, stitchesDouble.Count());

            // substract one for final command
            Assert.AreEqual(stitchesSingle.Count() * 2 - 1, stitchesDouble.Count() );
        }

        [Test()]
        public void WriteOnePolygonTwiceForThreeColorspecs()
        {
            var polygons = new List<Polygon> { this.createDefaultPolygon(), this.createOtherPolygon() };
            List<ColorTranslation> ctSingle = this.createSingleColorTranslation();
            List<ColorTranslation> ctDouble = this.createDoubleColorTranslation();

            var stitchesSingle = StepWriter.WriteStitches(ctSingle, polygons);
            var stitchesDouble = StepWriter.WriteStitches(ctDouble, polygons);

            Assert.IsTrue(stitchesSingle.Count() * 2 - 1 > stitchesDouble.Count() );
        }

        [Test()]
        public void FillBorderOrderIsMaintained()
        {
            var polygons = new List<Polygon> { this.createDefaultPolygon()};
            List<ColorTranslation> ctForward = this.createColorAndStrokeTranslation();
            List<ColorTranslation> ctBackward = this.createColorAndStrokeTranslation();
            ctBackward.Reverse();

            var stitchesForward = StepWriter.WriteStitches(ctForward, polygons);
            var stitchesBackward = StepWriter.WriteStitches(ctBackward, polygons);

            Assert.AreNotEqual(stitchesForward.Skip(1).First(), stitchesBackward.Skip(1).First());
        }

        List<ColorTranslation> createSingleColorTranslation()
        {
            return new List<ColorTranslation> 
            {
                new ColorTranslation { Color = this.color},
                new ColorTranslation { Color = this.otherColor},
            };
        }

        List<ColorTranslation> createColorAndStrokeTranslation()
        {
            return new List<ColorTranslation> 
            {
                new ColorTranslation { Color = this.color},
                new ColorTranslation { Color = this.stroke}
            };
        }

        List<ColorTranslation> createDoubleColorTranslation()
        {
            return new List<ColorTranslation> 
            {
                new ColorTranslation { Color = this.color},
                new ColorTranslation { Color = this.color},
                new ColorTranslation { Color = this.otherColor},
            };
        }

        Polygon createOtherPolygon()
        {
            return new Polygon(
                new List<Point> {
                new Point(0, 0),
                new Point(0, 1),
                new Point(1, 1),
                new Point(1, 0),
                },
                this.otherColor
            );
        }

        private Polygon createDefaultPolygon()
        {
            return new Polygon(
                new List<Point> {
                new Point(0, 0),
                new Point(0, 1),
                new Point(1, 1),
                new Point(1, 0),
            },
                this.color,
                this.stroke
            );
        }
    }
}

