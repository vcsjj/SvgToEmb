using NUnit.Framework;
using System;
using ShapeLib;
using System.Collections.Generic;

namespace SvgToEmbCSVTests
{
    [TestFixture()]
    public class StepperTests
    {
        [Test()]
        public void StepsCountainNoDoubles()
        {
            Polygon p = this.createDefaultPolygon();
            HorizontalStepper hs = new HorizontalStepper(p, 1.0);

            List<Step> steps = hs.CalculateSteps();

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
            Polygon p = this.createDefaultPolygon();
            HorizontalStepper hs = new HorizontalStepper(p, 1.0);
            List<Step> steps = hs.CalculateSteps();

            Assert.AreEqual(p.GetTopLeft(), steps[0].Point);
        }

        [Test()]
        public void FindsThreeIntersectionsForLargeHeight()
        {
            Polygon p = this.createTriangle();
            HorizontalStepper hs = new HorizontalStepper(p, 0.9);
            List<Step> steps = hs.CalculateSteps();

            Assert.AreEqual(3, steps.Count);
        }

        [Test()]
        public void FindsFiveIntersectionsForMediumHeight()
        {
            Polygon p = this.createTriangle();
            HorizontalStepper hs = new HorizontalStepper(p, 0.45);
            List<Step> steps = hs.CalculateSteps();

            Assert.AreEqual(5, steps.Count);
        }

        [Test()]
        public void FindsThreeIntersectionsForLargeHeightInverse()
        {
            Polygon p = this.createInverseTriangle();
            HorizontalStepper hs = new HorizontalStepper(p, 0.9);
            List<Step> steps = hs.CalculateSteps();

            Assert.AreEqual(3, steps.Count);
        }

        [Test()]
        public void FindsFiveIntersectionsForMediumHeightInverse()
        {
            Polygon p = this.createInverseTriangle();
            HorizontalStepper hs = new HorizontalStepper(p, 0.45);
            List<Step> steps = hs.CalculateSteps();

            Assert.AreEqual(5, steps.Count);
        }

        [Test()]
        public void FindValidIntersectionTest()
        {
            MyPoint p1, p2;
            CreateLinePoints(out p1, out p2);
            var intersection = HorizontalStepper.FindIntersection(p1, p2, 0);

            Assert.AreEqual(0, intersection.Y);
            Assert.AreEqual(2, intersection.X);
        }

        [Test()]
        public void FindLowerPointIntersectionTest()
        {
            MyPoint p1, p2;
            CreateLinePoints(out p1, out p2);
            var intersection = HorizontalStepper.FindIntersection(p1, p2, -4);

            Assert.IsNotNull(intersection);
        }

        [Test()]
        public void FindUpperPointIntersectionTest()
        {
            MyPoint p1, p2;
            CreateLinePoints(out p1, out p2);
            var intersection = HorizontalStepper.FindIntersection(p1, p2, 4);

            Assert.IsNotNull(intersection);
        }

        [Test()]
        public void FindInvalidPointIntersectionReturnsNullTest([Values(-5, 5, double.PositiveInfinity, double.NegativeInfinity, double.NaN)]double d)
        {
            MyPoint p1, p2;
            CreateLinePoints(out p1, out p2);
            var intersection = HorizontalStepper.FindIntersection(p1, p2, d);

            Assert.IsNull(intersection);
        }

        [Test()]
        public void FindIntersectionsOrderIsMaintained()
        {
            var p = this.createTriangle();
            var intersection = HorizontalStepper.FindIntersections(0.1, p);
            var first = intersection[0];
            var second = intersection[1];
            Assert.IsTrue(first.X <= second.X);
        }

        [Test()]
        public void FindIntersectionsOrderIsMaintained2()
        {
            var p = this.createInverseTriangle();
            var intersection = HorizontalStepper.FindIntersections(-0.1, p);
            var first = intersection[0];
            var second = intersection[1];
            Assert.IsTrue(first.X <= second.X);
        }

        [Test()]
        public void FindIntersectionsOrderIsMaintained3()
        {
            var p = this.createInverseTriangleOrder2();
            var intersection = HorizontalStepper.FindIntersections(-0.1, p);
            var first = intersection[0];
            var second = intersection[1];
            Assert.IsTrue(first.X <= second.X);
        }

        [Test()]
        public void FindIntersectionsOrderIsMaintained4()
        {
            var p = this.createInverseTriangleOrder3();
            var intersection = HorizontalStepper.FindIntersections(-0.1, p);
            var first = intersection[0];
            var second = intersection[1];
            Assert.IsTrue(first.X <= second.X);
        }

        [Test()]
        public void AdditionalStepsAreAddedIfDistanceIsTooBigY()
        {
            List<Step> l = new List<Step>
            {
                new Step(Step.StepType.Stitch, new MyPoint(0.0, 0.0)),
                new Step(Step.StepType.Stitch, new MyPoint(0.0, 10.0))
            };

            List<Step> filledList = HorizontalStepper.AddInbetweenStitches(l, 5.0);
            Assert.AreEqual(3, filledList.Count);
            Assert.AreEqual(0.0, filledList[0].Point.Y);
            Assert.AreEqual(5.0, filledList[1].Point.Y);
            Assert.AreEqual(10.0, filledList[2].Point.Y);

        }

        [Test()]
        public void AdditionalStepsAreAddedIfDistanceIsTooBigX()
        {
            List<Step> l = new List<Step>
                {
                    new Step(Step.StepType.Stitch, new MyPoint(0.0, 0.0)),
                    new Step(Step.StepType.Stitch, new MyPoint(1.0, 0.0))
                };

            List<Step> filledList = HorizontalStepper.AddInbetweenStitches(l, 0.5);


            Assert.AreEqual(0.0, filledList[0].Point.X);
            Assert.AreEqual(0.5, filledList[1].Point.X);
            Assert.AreEqual(1.0, filledList[2].Point.X);
        }

        [Test()]
        public void AdditionalStepsAreAddedIfDistanceIsTooBig2()
        {
            List<Step> l = new List<Step>
                {
                    new Step(Step.StepType.Stitch, new MyPoint(0.0, 0.0)),
                    new Step(Step.StepType.Stitch, new MyPoint(0.0, 9.0))
                };

            List<Step> filledList = HorizontalStepper.AddInbetweenStitches(l, 5.0);

            Assert.AreEqual(3, filledList.Count);
        }

        private static void CreateLinePoints(out MyPoint p1, out MyPoint p2)
        {
            p1 = new MyPoint(1, -4);
            p2 = new MyPoint(3, 4);
        }

        protected Polygon createDefaultPolygon()
        {
            return new Polygon(
                new List<MyPoint> {
                new MyPoint(0, 0),
                new MyPoint(0, 1),
                new MyPoint(1, 1),
                new MyPoint(1, 0),
            }
            );
        }

        private Polygon createTriangle()
        {
            return new Polygon(
                new List<MyPoint> {
                new MyPoint(0, 1),
                new MyPoint(1, 1),
                new MyPoint(1, 0),
            }
            );
        }
        private Polygon createInverseTriangle()
        {
            return new Polygon(
                new List<MyPoint> {
                new MyPoint(0, 0),
                new MyPoint(1, 0),
                new MyPoint(0.5, -1),
            }
            );
        }
        private Polygon createInverseTriangleOrder2()
        {
            return new Polygon(
                new List<MyPoint> {
                new MyPoint(1, 0),
                new MyPoint(0.5, -1),
                new MyPoint(0, 0),
            }
            );
        }
        private Polygon createInverseTriangleOrder3()
        {
            return new Polygon(
                new List<MyPoint> {
                new MyPoint(0.5, -1),
                new MyPoint(0, 0),
                new MyPoint(1, 0),
            }
            );
        }
    }
}

