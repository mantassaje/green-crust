using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class NoGrowthInDead : INatureGrowth
{
    public NatureGrowthModel GetChange(Biome biome, NatureGrowthModel model)
    {
        if (biome.Spec.IsDead)
        {
            return model.SetZeroGrowth().Cancel();
        }

        return model;
    }
}

public class NoGrowthForLonelyMudDesert : INatureGrowth
{
    public NatureGrowthModel GetChange(Biome biome, NatureGrowthModel model)
    {
        if (biome.Spec.IsBarren
            && biome.GetNearbyBiomesCache().All(near =>
                near.Spec.IsBarren
                || near.IsWater 
                || near.IsAbyss))
        {
            return model.SetZeroGrowth().Cancel();
        }

        return model;
    }
}

public class BaseNautreGrowth : INatureGrowth
{
    public NatureGrowthModel GetChange(Biome biome, NatureGrowthModel model)
    {
        return model.AddGrowth(0.2f);
    }
}

public class HumidAndWarmNautreGrowth : INatureGrowth
{
    public NatureGrowthModel GetChange(Biome biome, NatureGrowthModel model)
    {
        return model.AddGrowth(0.1f);
    }
}

public class ClimateNatureGrowth : INatureGrowth
{
    public NatureGrowthModel GetChange(Biome biome, NatureGrowthModel model)
    {
        var heatEffect = BiomeService.GetHeatRainDiffRate(biome);
        var grwoth = (heatEffect * 0.25f).GetMinMax(0f, 0.1f);
        return model.AddGrowth(grwoth);
    }
}
