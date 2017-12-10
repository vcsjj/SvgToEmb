using NUnit.Framework;
using System;
using ShapeLib;

namespace SvgToEmbCSVTests
{
    [TestFixture()]
    public class StepTests
    {
        [Test()]
        public void HasType([Values(Step.StepType.Jump, Step.StepType.Stitch)] Step.StepType t)
        {
            var st = new Step(t, new MyPoint(1,2));
            Assert.AreEqual(t, st.Type);
        }

        [Test()]
        public void HasPoint([Values(1.0, 2.0)] double px, [Values(-3.0, 4.9)] double py)
        {
            var p = new MyPoint(px, py);
            var st = new Step(Step.StepType.Jump, p);
            Assert.AreEqual(p, st.Point);
        }
    }
}

