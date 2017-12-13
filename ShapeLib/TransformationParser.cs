using System;
using System.Text.RegularExpressions;

namespace ShapeLib
{
    public class TransformationParser
    {
        private static System.Globalization.CultureInfo culture = System.Globalization.CultureInfo.InvariantCulture;
        private static  System.Globalization.NumberStyles numberStyle = System.Globalization.NumberStyles.Any;

        Regex singleNumberExpression = new Regex("([0-9.e\\-]+)");
        Regex twoNumbersExpression = new Regex("([0-9.e\\-]+), *([0-9.e\\-]+)");
        Regex sevenNumbersExpression = new Regex("([0-9.e\\-]+), *([0-9.e\\-]+), *([0-9.e\\-]+), *([0-9.e\\-]+), *([0-9.e\\-]+), *([0-9.e\\-]+)");

        private readonly string transformationstring;

        public TransformationParser(string transform)
        {
            this.transformationstring = transform;
        }

        double[,] GetUnitMatrix()
        {
            return new double[,] {
                {
                    1,
                    0,
                    0
                },
                {
                    0,
                    1,
                    0
                },
                {
                    0,
                    0,
                    1
                }
            };
        }

        double[,] ExtractRotation(string numbers)
        {
            double[,] m = GetUnitMatrix();
            var match = singleNumberExpression.Match(numbers);
            if (match.Groups.Count == 2)
            {
                double alpha = 0.0;
                if (double.TryParse(match.Groups[1].Value, numberStyle, culture, out alpha))
                {
                    double a = alpha / 180.0 * Math.PI;
                    m[0, 0] = Math.Cos(a);
                    m[0, 1] = Math.Sin(a);
                    m[1, 0] = Math.Cos(a);
                    m[1, 1] = -Math.Sin(a);
                }
            }
            return m;
        }

        double[,] ExtractTranslation(string numbers)
        {
            
            double[,] m = GetUnitMatrix();
            var match = twoNumbersExpression.Match(numbers);
            if (match.Groups.Count == 3)
            {
                double x = 0.0;
                double y = 0.0;
                if (double.TryParse(match.Groups[1].Value, numberStyle, culture, out x) && double.TryParse(match.Groups[2].Value, numberStyle, culture, out y))
                {
                    m[2, 0] = x;
                    m[2, 1] = y;
                }
            }
            return m;
        }

        double[,] ExtractMatrix(string numbers)
        {
            
            double[,] m = GetUnitMatrix();
            var match = sevenNumbersExpression.Match(numbers);
            if (match.Groups.Count == 7)
            {
                double a, b, c, d, e, f;
                if (double.TryParse(match.Groups[1].Value, numberStyle, culture, out a) 
                    && double.TryParse(match.Groups[2].Value, numberStyle, culture, out b)
                    && double.TryParse(match.Groups[3].Value, numberStyle, culture, out c)
                    && double.TryParse(match.Groups[4].Value, numberStyle, culture, out d)
                    && double.TryParse(match.Groups[5].Value, numberStyle, culture, out e)
                    && double.TryParse(match.Groups[6].Value, numberStyle, culture, out f))
                {
                    m[0, 0] = a;
                    m[0, 1] = b;
                    m[1, 0] = c;
                    m[1, 1] = d;
                    m[2, 0] = e;
                    m[2, 1] = f;
                }
            }

            return m;
        }

        double[,] ExtractScale(string numbers)
        {
            double[,] m = GetUnitMatrix();
            var match = this.singleNumberExpression.Match(numbers);
            if (match.Groups.Count == 2)
            {
                double s;
                if (double.TryParse(match.Groups[1].Value, numberStyle, culture, out s))
                {
                    m[0, 0] = s;
                    m[1, 1] = s;
                }
            }

            return m;
        }

        double[,] ExtractSkewX(string numbers)
        {
            double[,] m = GetUnitMatrix();
            var match = this.singleNumberExpression.Match(numbers);
            if (match.Groups.Count == 2)
            {
                double s;
                if (double.TryParse(match.Groups[1].Value, numberStyle, culture, out s))
                {
                    m[1, 0] = Math.Tan(Math.PI * s/ 180.0);
                }
            }

            return m;
        }

        double[,] ExtractSkewY(string numbers)
        {
            double[,] m = GetUnitMatrix();
            var match = this.singleNumberExpression.Match(numbers);
            if (match.Groups.Count == 2)
            {
                double s;
                if (double.TryParse(match.Groups[1].Value, numberStyle, culture, out s))
                {
                    m[0, 1] = Math.Tan(Math.PI * s/ 180.0);
                }
            }

            return m;
        }

        public double[] GetTransformation() 
        {
            Regex transformationsRegex = new Regex("(translate|rotate|matrix|scale|skewX|skewY)\\((.*?)\\)");
            var mc = transformationsRegex.Matches(this.transformationstring);
            var m0 = GetUnitMatrix();

            foreach (Match match in mc){
                double [,] m;
                var numbers = match.Groups[2].Value;
                switch (match.Groups[1].Value)
                {
                    case "translate":
                        m = ExtractTranslation(numbers);
                        break;
                    case "matrix":
                        m = ExtractMatrix(numbers);
                        break;
                    case "rotate":
                        m = ExtractRotation(numbers);
                        break;
                    case "scale":
                        m = ExtractScale(numbers);
                        break;
                    case "skewX":
                        m = ExtractSkewX(numbers);
                        break;
                    case "skewY":
                        m = ExtractSkewY(numbers);
                        break;
                    default:
                        m = GetUnitMatrix();
                        break;
                }

                m0 = Multiply(m0, m);
            }

            return new double[] {
                m0[0,0],
                m0[0,1],
                m0[1,0],
                m0[1,1],
                m0[2,0],
                m0[2,1]};
        }

        double[,] Multiply(double[,] A, double[,] B) 
        {
            int rA = A.GetLength(0);
            int cA = A.GetLength(1);
            int rB = B.GetLength(0);
            int cB = B.GetLength(1);
            double temp = 0;
            double[,] kHasil = new double[rA, cB];
            if (cA != rB)
            {
                return null;
            }
            else
            {
                for (int i = 0; i < rA; i++)
                {
                    for (int j = 0; j < cB; j++)
                    {
                        temp = 0;
                        for (int k = 0; k < cA; k++)
                        {
                            temp += A[i, k] * B[k, j];
                        }

                        kHasil[i, j] = temp;
                    }
                }

                return kHasil;
            }
        }
    }
}

