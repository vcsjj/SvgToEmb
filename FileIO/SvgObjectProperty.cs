using System;
using ShapeLib;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace FileIO
{
    public enum SvgPropertyType 
    {
        Fill,
        Stroke,
    }

	public class SvgObjectProperty
	{
        public string Color;
        public SvgPropertyType Type;

        public override bool Equals(object obj)
        {
            if (obj is SvgObjectProperty)
            {
                var p2 = obj as SvgObjectProperty;
                return p2.Color == this.Color && p2.Type == this.Type;
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return (this.Color.GetHashCode() + this.Type.GetHashCode()).GetHashCode();
        }
	}
}

