using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class NatureGrowthModel
{
    public float Growth { get; private set; }
    public bool IsCanceled { get; private set; }

    public NatureGrowthModel Cancel()
    {
        this.IsCanceled = true;
        return this;
    }

    public NatureGrowthModel AddGrowth(float growth)
    {
        Growth += growth.GetMinMax(0f, 1f);
        return this;
    }

    public NatureGrowthModel SetZeroGrowth()
    {
        Growth = 0;
        return this;
    }
}
