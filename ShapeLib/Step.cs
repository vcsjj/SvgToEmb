using System;

namespace ShapeLib
{
    public class Step : IEquatable<Step>
    {
        public enum StepType 
        {
            Stitch,
            Trim
        }

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

        private readonly Point point;
        public Point Point
        {
            get
            {
                return point;
            }
        }


        public Step(StepType type, Point p)
        {
            this.type = type;
            this.point = p;
        }

        public bool Equals(Step other)
        {
            return this.Type == other.Type
                && this.Point.X == other.Point.X
                && this.Point.Y == other.Point.Y;
        }
    }
}

