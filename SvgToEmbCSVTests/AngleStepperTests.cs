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
            var ct = new ColorTranslation();
            ct.LineHeight = 0.234;

            Polygon p = this.createDefaultPolygon();
            HorizontalStepper hs = new HorizontalStepper(p, ct);
            AngleStepper a = new AngleStepper(p, ct);


            List<Step> stepsHorizontal = hs.CalculateSteps();
            List<Step> stepsAngle = a.CalculateSteps();

            CollectionAssert.AreEqual(stepsHorizontal, stepsAngle);
        }

        [Test()]
        public void Angle1IsNotIdenticalWithHorizontal()
        {
            Polygon p = this.createDefaultPolygon();
            var ct = new ColorTranslation();
            ct.LineHeight = 0.234;

            var ct2 = new ColorTranslation();
            ct2.LineHeight = 0.234;
            ct2.StepAngle = 1;

            HorizontalStepper hs = new HorizontalStepper(p, ct);
            AngleStepper a = new AngleStepper(p, ct2);

            List<Step> stepsHorizontal = hs.CalculateSteps();
            List<Step> stepsAngle = a.CalculateSteps();

            CollectionAssert.AreNotEqual(stepsHorizontal, stepsAngle);
        }
    }
}

