using System;
using System.Collections.Generic;
using ShapeLib;

namespace FileIO
{
    public class ColorMapReader
    {
        public ColorMapReader()
        {
        }

        public List<ColorTranslation>  Read(IEnumerable<string> lines)
        {
            var culture = System.Globalization.CultureInfo.InvariantCulture;
            var numberStyle = System.Globalization.NumberStyles.Any;

            var d = new List<ColorTranslation>();
            foreach (var line in lines)
            {
                string[] colorspec = line.Split(',');
                if (colorspec.Length != 5)
                    continue;

                string color = colorspec[0];
                string stepWidth = colorspec[1];
                string maxStepLength = colorspec[2];
                string stepAngle = colorspec[3];
                string moveInside = colorspec[4];

                double sw,sa,mi,ma;
                if (!double.TryParse(stepWidth, numberStyle, culture, out sw))
                    continue;
                if (!double.TryParse(maxStepLength, numberStyle, culture, out ma))
                    continue;
                if (!double.TryParse(stepAngle, numberStyle, culture, out sa))
                    continue;
                if (!double.TryParse(moveInside, numberStyle, culture, out mi))
                    continue;
                d.Add(new ColorTranslation{Color = color, StepWidth = sw, StepAngle = sa, MoveInside = mi, MaxStepLength = ma});
            }

            return d;
        }

    }
}

