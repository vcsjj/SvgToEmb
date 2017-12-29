using NUnit.Framework;
using System;
using ShapeLib;
using System.Collections.Generic;
using System.Linq;

namespace SvgToEmbCSVTests
{
    [TestFixture()]
    public class StepperTests : StepperTestsBase
    {
        Polygon polygon;
        ColorTranslation colorTranslation;
        Polygon triangle;
        Polygon inverseTriangle;

        [TestFixtureSetUp()]
        public void Setup() 
        {
            this.polygon = this.createDefaultPolygon();
            this.triangle = this.createTriangle();
            this.inverseTriangle = this.createInverseTriangle();
            this.colorTranslation = this.createDefaultColorTranslation(0.1);
        }

        [Test()]
        public void StepsCountainNoDoubles()
        {

            HorizontalStepper hs = new HorizontalStepper(this.polygon, this.colorTranslation);

            List<Step> steps = hs.CalculateFillSteps();

            bool isRepeated = false;
            var set = new HashSet<Tuple<double,double>>();
            foreach(var x in steps)
            {
                if(!set.Add(new Tuple<double,double>(x.Point.X, x.Point.Y))) 
                {
                    isRepeated = true; 
                    break;
                }
            }

            Assert.IsFalse(isRepeated);
        }

        [Test()]
        public void FirstPointIsIdenticalWithUpperLeftCorner()
        {
            HorizontalStepper hs = new HorizontalStepper(this.polygon, this.colorTranslation);
            List<Step> steps = hs.CalculateFillSteps();

            Assert.AreEqual(this.polygon.Vertices[0], steps[0].Point);
        }

        [Test()]
        public void FindsThreeIntersectionsForLargeHeight()
        {
            this.colorTranslation.LineHeight = 0.9;
            HorizontalStepper hs = new HorizontalStepper(this.triangle, this.colorTranslation);
            List<Step> steps = hs.CalculateFillSteps();

            Assert.AreEqual(3, steps.Count);
        }

        [Test()]
        public void FindsFiveIntersectionsForMediumHeight()
        {
            this.colorTranslation.LineHeight = 0.45;
            HorizontalStepper hs = new HorizontalStepper(this.triangle, this.colorTranslation);
            List<Step> steps = hs.CalculateFillSteps();

            Assert.AreEqual(5, steps.Count);
        }

        [Test()]
        public void FindsThreeIntersectionsForLargeHeightInverse()
        {
            this.colorTranslation.LineHeight = 0.9;
            HorizontalStepper hs = new HorizontalStepper(this.inverseTriangle, this.colorTranslation);
            List<Step> steps = hs.CalculateFillSteps();

            Assert.AreEqual(3, steps.Count);
        }

        [Test()]
        public void FindsFiveIntersectionsForMediumHeightInverse()
        {
            this.colorTranslation.LineHeight = 0.45;
            HorizontalStepper hs = new HorizontalStepper(this.inverseTriangle, this.colorTranslation);
            List<Step> steps = hs.CalculateFillSteps();

            Assert.AreEqual(5, steps.Count);
        }

      


        [Test()]
        public void FillStartsWithOutline()
        {
            HorizontalStepper h = new HorizontalStepper(this.polygon, this.colorTranslation);
            var fillSteps = h.CalculateFillSteps();
            var outlineSteps = h.CalculateOutlineSteps();
            Assert.AreEqual(fillSteps.First().Point, outlineSteps.Last().Point);
        }

        [Test()]
        public void OutlineHasAtLeastAsManyStepsAsPolygonHasVertices()
        {
            HorizontalStepper h = new HorizontalStepper(this.polygon, this.colorTranslation);
            var outlineSteps = h.CalculateOutlineSteps();
            Assert.IsTrue(outlineSteps.Count >= this.polygon.Vertices.Count + 1);
        }

        [Test()]
        public void OutlineHasMoreStepsThanPolygonHasVerticesForSmallSteplength()
        {
            this.colorTranslation.MaxStepLength = this.polygon.Vertices[0].Distance(this.polygon.Vertices[1])/2;
            HorizontalStepper h = new HorizontalStepper(this.polygon, this.colorTranslation);
            var outlineSteps = h.CalculateOutlineSteps();
            Assert.IsTrue(outlineSteps.Count > this.polygon.Vertices.Count + 1);
        }





        private Polygon createTriangle()
        {
            return new Polygon(
                new List<Point> {
                new Point(0, 1),
                new Point(1, 1),
                new Point(1, 0),
            }
            );
        }
        private Polygon createInverseTriangle()
        {
            return new Polygon(
                new List<Point> {
                new Point(0, 0),
                new Point(1, 0),
                new Point(0.5, -1),
            }
            );
        }
        private Polygon createInverseTriangleOrder2()
        {
            return new Polygon(
                new List<Point> {
                new Point(1, 0),
                new Point(0.5, -1),
                new Point(0, 0),
            }
            );
        }
        private Polygon createInverseTriangleOrder3()
        {
            return new Polygon(
                new List<Point> {
                new Point(0.5, -1),
                new Point(0, 0),
                new Point(1, 0),
            }
            );
        }

        ColorTranslation createDefaultColorTranslation(double lineHeight)
        {
            var ct = new ColorTranslation();
            ct.LineHeight = lineHeight;
            return ct;
        }
    }
}

