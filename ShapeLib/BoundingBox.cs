using System;

namespace ShapeLib
{
    public class BoundingBox
    {
        public double Bottom
        {
            get;
            private set;
        }

        public double Top
        {
            get;
            private set;
        }

        public double Left
        {
            get;
            private set;
        }

        public double Right
        {
            get;
            private set;
        }

        public BoundingBox(double minX, double minY, double width, double height)
        {
            this.Bottom = minY;
            this.Top = minY + height;
            this.Left = minX;
            this.Right = minX + width;
        }
    }
}

