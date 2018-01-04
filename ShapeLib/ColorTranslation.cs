﻿using System;

namespace ShapeLib
{
    public class ColorTranslation
    {
        public string Color = "fill:#000000";
        public double LineHeight = 0.5;
        public double StepAngle = 0;
        public double MoveInside = 0.0;
        public double MaxStepLength = 3.0;

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

    public static class ColorInfo 
    {
        public static bool IsFill(this ColorTranslation @this) => @this.Color.StartsWith("fill:");

        public static bool IsStroke(this ColorTranslation @this) => @this.Color.StartsWith("stroke:");
    }
}

