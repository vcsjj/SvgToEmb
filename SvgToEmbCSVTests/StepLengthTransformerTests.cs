using NUnit.Framework;
using System;
using System.Collections.Generic;
using ShapeLib;

namespace SvgToEmbCSVTests
{
    [TestFixture()]
    public class StepLengthTransformerTests
    {
        [Test()]
        public void AdditionalStepsAreAddedIfDistanceIsTooBigY()
        {
            List<Step> l = new List<Step>
                {
                    new Step(Step.StepType.Stitch, new Point(0.0, 0.0)),
                    new Step(Step.StepType.Stitch, new Point(0.0, 10.0))
                };

            List<Step> filledList = new StepLengthTransformer(l, 5).AddInbetweenStitches();
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
                    new Step(Step.StepType.Stitch, new Point(0.0, 0.0)),
                    new Step(Step.StepType.Stitch, new Point(1.0, 0.0))
                };

            List<Step> filledList = new StepLengthTransformer(l, 0.5).AddInbetweenStitches();


            Assert.AreEqual(0.0, filledList[0].Point.X);
            Assert.AreEqual(0.5, filledList[1].Point.X);
            Assert.AreEqual(1.0, filledList[2].Point.X);
        }

        [Test()]
        public void AdditionalStepsAreAddedIfDistanceIsTooBig2()
        {
            List<Step> l = new List<Step>
                {
                    new Step(Step.StepType.Stitch, new Point(0.0, 0.0)),
                    new Step(Step.StepType.Stitch, new Point(0.0, 9.0))
                };

            List<Step> filledList = new StepLengthTransformer(l, 5).AddInbetweenStitches();

            Assert.AreEqual(3, filledList.Count);
        }
    }
}

