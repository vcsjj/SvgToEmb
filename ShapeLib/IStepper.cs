using System;
using System.Collections.Generic;

namespace ShapeLib
{
    public interface IStepper
    {
        List<Step> CalculateSteps(double lineHeight);
    }
}

