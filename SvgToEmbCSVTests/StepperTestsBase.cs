using System;
using ShapeLib;
using System.Collections.Generic;

namespace SvgToEmbCSVTests
{
    public class StepperTestsBase
    {
        protected Polygon createDefaultPolygon()
        {
            return new Polygon(
                new List<Point> {
                new Point(0, 0),
                new Point(0, 1),
                new Point(1, 1),
                new Point(1, 0),
            }
            );
        }
    }
}

