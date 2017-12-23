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
                if (colorspec.Length != 4)
                    continue;

                string color = colorspec[0];
                string stepWidth = colorspec[1];
                string stepAngle = colorspec[2];
                string moveInside = colorspec[3];

                double sw,sa,mi;
                if (!double.TryParse(stepWidth, numberStyle, culture, out sw))
                    continue;
                if (!double.TryParse(stepAngle, numberStyle, culture, out sa))
                    continue;
                if (!double.TryParse(moveInside, numberStyle, culture, out mi))
                    continue;
                d.Add(new ColorTranslation{Color = color, StepWidth = sw, StepAngle = sa, MoveInside = mi});
            }

            return d;
        }

    }
}

