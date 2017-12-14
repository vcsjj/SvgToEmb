using System;

namespace ShapeLib
{
    public enum FillTypes 
    {
        Vertical,
        Horizontal,
    }

    public class Fill
    {
        public FillTypes FillType
        {
            get;
        }

        public double StitchWidth
        {
            get;
        }

        public Fill(FillTypes filltype, double d)
        {
            this.FillType = filltype;
            this.StitchWidth = d;
        }

        public static Fill Default() 
        {
            return new Fill(FillTypes.Horizontal, 0.2);
        }
    }
}

