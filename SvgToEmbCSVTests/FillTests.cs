using NUnit.Framework;
using System;
using ShapeLib;

namespace SvgToEmbCSVTests
{
    [TestFixture()]
    public class FillTests
    {
        [Test()]
        public void TestCase()
        {
            var f = new Fill(FillTypes.Vertical, 0.4);
            Assert.AreEqual(FillTypes.Vertical, f.FillType);
            Assert.AreEqual(0.4, f.StitchWidth);
        }
    }
}

