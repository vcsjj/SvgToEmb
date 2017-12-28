using System;
using System.Collections.Generic;

namespace ShapeLib
{
    public interface IStepper
    {
        List<Step> CalculateAllSteps();
        List<Step> CalculateFillSteps();
        List<Step> CalculateOutlineSteps();
    }
}

