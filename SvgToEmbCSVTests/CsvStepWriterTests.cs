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

        private Step CreateStitchStep() 
        {
            return new Step(Step.StepType.Stitch, new MyPoint(1, 2));
        }
    }
}

