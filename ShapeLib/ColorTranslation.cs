using System;

namespace ShapeLib
{
    public struct ColorTranslation
    {
        public static ColorTranslation Default = new ColorTranslation {Color = "#000000", StepAngle = 0, StepWidth=0.5, MoveInside = 0.0};
        public string Color;
        public double StepWidth;
        public double StepAngle;
        public double MoveInside;

        public override string ToString()
        {
            var culture = System.Globalization.CultureInfo.InvariantCulture;
            return string.Join(",", new string[]
                {
                    Color,
                    StepWidth.ToString(culture),
                    StepAngle.ToString(culture),
                    MoveInside.ToString(culture),
                }
            );
        }

        public const string Header = "Color,StepWidth,StepAngle,MoveInside";
    }
}

