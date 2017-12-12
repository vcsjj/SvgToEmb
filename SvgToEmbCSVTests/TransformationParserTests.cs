using NUnit.Framework;
using System;
using ShapeLib;

namespace SvgToEmbCSVTests
{
    [TestFixture()]
    public class TransformationParserTests
    {
        [Test()]
        public void Translate()
        {
            TransformationParser tp = new TransformationParser("translate(15, -34.8)");
            double[] tr = tp.GetTransformation();
            var p = new MyPoint(1, 2, tr);

            Assert.AreEqual(16, p.X);
            Assert.AreEqual(-32.8, p.Y);
        }

        [Test()]
        public void Matrix()
        {
            TransformationParser tp = new TransformationParser("matrix(1,0,0,2,-3, 3)");
            double[] tr = tp.GetTransformation();
            var p = new MyPoint(1, 2, tr);

            Assert.AreEqual(-2, p.X);
            Assert.AreEqual(7, p.Y);
        }

        [Test()]
        public void TranslateAndMatrix()
        {
            TransformationParser tp = new TransformationParser("translate(3,-4.0) matrix(2,0,0,2,0,0) ");
            double[] tr = tp.GetTransformation();
            var p = new MyPoint(1, 2, tr);

            Assert.AreEqual(8, p.X);
            Assert.AreEqual(-4, p.Y);
        }

        [Test()]
        public void MatrixAndTranslate()
        {
            TransformationParser tp = new TransformationParser("matrix(2,0,0,2,0,0) translate(3,-4.0)");
            double[] tr = tp.GetTransformation();
            var p = new MyPoint(1, 2, tr);

            Assert.AreEqual(5, p.X);
            Assert.AreEqual(0, p.Y);
        }

        [Test()]
        public void Rotate()
        {
            TransformationParser tp = new TransformationParser("rotate(90)");
            double[] tr = tp.GetTransformation();
            var p = new MyPoint(1, 0, tr);

            Assert.AreEqual(0, Math.Round(p.X, 8));
            Assert.AreEqual(1, Math.Round(p.Y, 8));
        }

        [Test()]
        public void Rotate2()
        {
            TransformationParser tp = new TransformationParser("rotate(-90)");
            double[] tr = tp.GetTransformation();
            var p = new MyPoint(1, 0, tr);

            Assert.AreEqual(0, Math.Round(p.X, 8));
            Assert.AreEqual(-1, Math.Round(p.Y, 8));
        }

        [Test()]
        public void Rotate3()
        {
            TransformationParser tp = new TransformationParser("rotate(180)");
            double[] tr = tp.GetTransformation();
            var p = new MyPoint(1, 0, tr);

            Assert.AreEqual(-1, Math.Round(p.X, 8));
            Assert.AreEqual(0, Math.Round(p.Y, 8));
        }

        [Test()]
        public void Rotate4()
        {
            TransformationParser tp = new TransformationParser("rotate(360)");
            double[] tr = tp.GetTransformation();
            var p = new MyPoint(1, 0, tr);

            Assert.AreEqual(1, Math.Round(p.X, 8));
            Assert.AreEqual(0, Math.Round(p.Y, 8));
        }
    }
}

