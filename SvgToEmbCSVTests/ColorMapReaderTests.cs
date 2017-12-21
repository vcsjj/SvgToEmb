using NUnit.Framework;
using System;
using SvgToEmbCSV;
using ShapeLib;
using System.Collections.Generic;
using FileIO;

namespace SvgToEmbCSVTests
{
    [TestFixture()]
    public class ColorMapReaderTests
    {
        [Test()]
        public void ReadsEmptyFile()
        {
            var c = new ColorMapReader();
            List<ColorTranslation>  d = c.Read(new string[]{});

            Assert.AreEqual(0, d.Count);
        }

        [Test()]
        public void RejectsLineWithWrongNumberOfFields()
        {
            var c = new ColorMapReader();
            List<ColorTranslation>  d = c.Read(new []{"", "1;2;3"});

            Assert.AreEqual(0, d.Count);
        }

        [Test()]
        public void AcceptsLineWithCorrectNumberOfFields()
        {
            var c = new ColorMapReader();
            List<ColorTranslation>  d = c.Read(new []{"#aa00dd;1.2;30;0.2", "1;2;3"});

            Assert.AreEqual(1, d.Count);
        }

        [Test()]
        public void ParsesDoubleFields()
        {
            var c = new ColorMapReader();
            List<ColorTranslation> d = c.Read(new []{"#aa00dd;1.2;30;0.2", "1;2;3"});

            Assert.AreEqual("#aa00dd", d[0].Color);
            Assert.AreEqual(1.2, d[0].StepWidth);
            Assert.AreEqual(30, d[0].StepAngle);
            Assert.AreEqual(0.2, d[0].MoveInside);
        }
    }
}

