using NUnit.Framework;
using System;
using ShapeLib;
using System.Collections.Generic;

namespace SvgToEmbCSVTests
{
    [TestFixture()]
    public class AngleStepperTests : StepperTests
    {
        [Test()]
        public void Angle0IsIdenticalWithHorizontal()
        {
            Polygon p = this.createDefaultPolygon();
            HorizontalStepper hs = new HorizontalStepper(p);
            AngleStepper a = new AngleStepper(p, 0);

            List<Step> stepsHorizontal = hs.CalculateSteps(0.234);
            List<Step> stepsAngle = a.CalculateSteps(0.234);

            CollectionAssert.AreEqual(stepsHorizontal, stepsAngle);
        }

        [Test()]
        public void Angle1IsNotIdenticalWithHorizontal()
        {
            Polygon p = this.createDefaultPolygon();
            HorizontalStepper hs = new HorizontalStepper(p);
            AngleStepper a = new AngleStepper(p, 1.0);

            List<Step> stepsHorizontal = hs.CalculateSteps(0.234);
            List<Step> stepsAngle = a.CalculateSteps(0.234);

            CollectionAssert.AreNotEqual(stepsHorizontal, stepsAngle);
        }

        [Test()]
        public void Angle90IsIdenticalWithVertical()
        {
            Polygon p = this.createDefaultPolygon();
            VerticalStepper hs = new VerticalStepper(p);
            AngleStepper a = new AngleStepper(p, 90);

            List<Step> stepsVertical = hs.CalculateSteps(0.234);
            List<Step> stepsAngle = a.CalculateSteps(0.234);

            CollectionAssert.AreEqual(stepsVertical, stepsAngle);
        }
    }
}

