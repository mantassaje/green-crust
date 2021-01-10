using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public static class EnumHelper
{
    public static BomeClimateVisual Next(BomeClimateVisual enumVal)
    {
        enumVal++;
        if (!Enum.IsDefined(typeof(BomeClimateVisual), enumVal))
            enumVal = BomeClimateVisual.Brighten;
        return enumVal;
    }
}

