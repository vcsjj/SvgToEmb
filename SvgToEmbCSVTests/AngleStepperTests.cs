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
            var d = 0.234;
            Polygon p = this.createDefaultPolygon();
            HorizontalStepper hs = new HorizontalStepper(p, d);
            AngleStepper a = new AngleStepper(p, 0, d);


            List<Step> stepsHorizontal = hs.CalculateSteps();
            List<Step> stepsAngle = a.CalculateSteps();

            CollectionAssert.AreEqual(stepsHorizontal, stepsAngle);
        }

        [Test()]
        public void Angle1IsNotIdenticalWithHorizontal()
        {
            Polygon p = this.createDefaultPolygon();
            const double d = 0.234;
            HorizontalStepper hs = new HorizontalStepper(p, d);
            AngleStepper a = new AngleStepper(p, 1.0, d);

            List<Step> stepsHorizontal = hs.CalculateSteps();
            List<Step> stepsAngle = a.CalculateSteps();

            CollectionAssert.AreNotEqual(stepsHorizontal, stepsAngle);
        }
    }
}

