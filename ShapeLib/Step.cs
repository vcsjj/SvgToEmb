using System;

namespace ShapeLib
{
    public class Step
    {
        private readonly StepType type;
        public double X => this.point.X;
        public double Y => this.point.Y;
        public StepType Type
        {
            get
            {
                return type;
            }
        }

        private readonly MyPoint point;
        public MyPoint Point
        {
            get
            {
                return point;
            }
        }

        public enum StepType 
        {
            Stitch,
            Jump
        }

        public Step(StepType type, MyPoint p)
        {
            this.type = type;
            this.point = p;
        }
    }
}

