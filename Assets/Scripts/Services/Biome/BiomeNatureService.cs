using System.Collections.Generic;
using System.Linq;

public static class BiomeNatureService
{
    public static List<INatureGrowth> NatureGrowths { get; set; }

    static BiomeNatureService()
    {
        NatureGrowths = new List<INatureGrowth>
        {
            new NoGrowthInDead(),
            new NoGrowthForLonelyMudDesert(),
            new BaseNautreGrowth(),
            new HumidAndWarmNautreGrowth(),
            new ClimateNatureGrowth()
        };
    }

    public static int GetNatureCap(Biome biome)
    {
        var cap = 0;
        if (biome.Spec.IsDead || biome.IsWater || biome.IsAbyss)
        {
            return 0;
        }
        else if (biome.Weather.RainfallType == RainfallTypes.Dry) cap = 2;
        else if (biome.Weather.RainfallType == RainfallTypes.Humid) cap = 4;
        else if (biome.Weather.RainfallType == RainfallTypes.Wet && biome.Weather.HeatType <= HeatTypes.Cold) cap = 4;
        else if (biome.Weather.RainfallType == RainfallTypes.Wet)
        {
            //If rainforest then even bigger cap
            if (biome.Weather.HeatType == HeatTypes.Hot) cap = 7;
            else cap = 6;
        }
        if (cap != 0 
            && biome.Crust.Height >= BiomeService.MinMountainHeight)
        {
            cap -= biome.Crust.Height;
            cap = cap.GetMin(1);
        }
        return cap;
    }
    
    public static float GetNatureGrowth(Biome biome)
    {
        var growthModel = new NatureGrowthModel();
        foreach(var growther in NatureGrowths)
        {
            growther.GetChange(biome, growthModel);
            if (growthModel.IsCanceled) break;
        }
        return growthModel.Growth;
    }

    /// <summary>
    /// Grow all biomes of specific player.
    /// Capture teritory.
    /// Used by end turn.
    /// </summary>
    /// <param name="player"></param>
    public static void Grow(Player player)
    {
        var biomes = Singles.Grid.GetAllBiomes()
            .Where(v => v.IsOwner(player))
            .OrderBy(v=>v.Weather.Sorter);//HACK replace sorter
        foreach (var biome in biomes) Grow(player, biome);
    }

    /// <summary>
    /// Grow specific biome of specific player.
    /// Capture teritory.
    /// Used by end turn.
    /// </summary>
    /// <param name="player"></param>
    /// <param name="biome"></param>
    public static void Grow(Player player, Biome biome)
    {
        if (biome.Owner != player) return;
        var grassAdd = GetNatureGrowth(biome);
        biome.PlantGrass(player, grassAdd);
        biome.Nature.Maturity += grassAdd;
        if (biome.Nature.Population >= 1)
            BiomeService.TryCaptureNearby(biome, player, true);
    }
}

