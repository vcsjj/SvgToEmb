using NUnit.Framework;
using System;
using ShapeLib;
using SvgToEmbCSV;

namespace SvgToEmbCSVTests
{
    [TestFixture()]
    public class CsvStepWriterTests
    {
        [Test()]
        public void WriteLineReturnsValidLine()
        {
            var step = this.CreateStitchStep();
            CsvStepWriter w = new CsvStepWriter(step);
            string line = w.Write();

            Assert.IsTrue(line.Contains("STITCH"));
        }

        [Test()]
        public void WriteLineContainsFourFields()
        {
            var step = this.CreateStitchStep();
            CsvStepWriter w = new CsvStepWriter(step);
            string line = w.Write();
            string[] parts = line.Split(',');
            Assert.AreEqual(4, parts.Length);
        }

        [Test()]
        public void WriteLineEnclosesFieldsWithQuote()
        {
            var step = this.CreateStitchStep();
            CsvStepWriter w = new CsvStepWriter(step);
            string line = w.Write();
            string[] parts = line.Split(',');
            foreach (var part in parts)
            {
                Assert.AreEqual('"', part[0]);
                Assert.AreEqual('"', part[part.Length - 1]);
            }
        }

        [Test()]
        public void XandYHaveTwoDigits()
        {
            var step = this.CreateStitchStep();
            CsvStepWriter w = new CsvStepWriter(step);
            string line = w.Write();
            string[] parts = line.Split(',');
            Assert.AreEqual("1.00", parts[2].Replace("\"", string.Empty));
            Assert.AreEqual("2.00", parts[3].Replace("\"", string.Empty));
        }

        [Test()]
        public void XandYHaveTwoDigits2()
        {
            var step = this.CreateOtherStitchStep();
            CsvStepWriter w = new CsvStepWriter(step);
            string line = w.Write();
            string[] parts = line.Split(',');
            Assert.AreEqual("-4.57", parts[2].Replace("\"", string.Empty));
            Assert.AreEqual("33.10", parts[3].Replace("\"", string.Empty));
        }

        private Step CreateOtherStitchStep() 
        {
            return new Step(Step.StepType.Stitch, new MyPoint(-4.567, 33.1));
        }

        private Step CreateStitchStep() 
        {
            return new Step(Step.StepType.Stitch, new MyPoint(1, 2));
        }
    }
}

