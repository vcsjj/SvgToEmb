using System;

namespace ShapeLib
{
    public struct ColorTranslation
    {
        public static ColorTranslation Default = new ColorTranslation {Color = "#000000", StepAngle = 0, LineHeight=0.5, MaxStepLength = 3.0, MoveInside = 0.0};
        public string Color;
        public double LineHeight;
        public double StepAngle;
        public double MoveInside;
        public double MaxStepLength;

        public override string ToString()
        {
            var culture = System.Globalization.CultureInfo.InvariantCulture;
            return string.Join(",", new string[]
                {
                    Color,
                    LineHeight.ToString(culture),
                    MaxStepLength.ToString(culture),
                    StepAngle.ToString(culture),
                    MoveInside.ToString(culture),
                }
            );
        }

        public const string Header = "Color,LineHeight,MaxStepLength,StepAngle,MoveInside";
    }
}

