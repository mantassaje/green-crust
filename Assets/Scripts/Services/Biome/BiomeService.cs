using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public static class BiomeService
{
    public static List<IBiomeSpec> BiomeSpecs { get; set; }
    public static IBiomeSpec DefaultBiomeSpec { get; private set; }
    public const int MinMountainHeight = 1;

    static BiomeService()
    {
        var coldDesertBiome = new ColdDesertBiomeSpec();
        DefaultBiomeSpec = coldDesertBiome;
        BiomeSpecs = new List<IBiomeSpec>
        {
            new AbyssBiomeSpec(),
            new HotMudDesertBiomeSpec(),
            new WarmMudDesertBiomeSpec(),
            new ColdMudDesertBiomeSpec(),
            new HotDesertBiomeSpec(),
            new WarmDesertBiomeSpec(),
            coldDesertBiome,
            new FrozenDesertBiomeSpec(),
            new ShallowSeaSpec(),
            new DeepSeaSpec(),
            new IcaCapOceanBiomeSpec(),
            new TundraBiomeSpec(),
            new TaigaBiomeSpec(),
            new GrasslandBiomeSpec(),
            new LeafForestBiomeSpec(),
            new WetlandsBiomeSpec(),
            new SavanaBiomeSpec(),
            new TropicsBiomeSpec(),
            new RainforestBiomeSpec(),
            new BuglandBiomeSpec()
        };
    }

    /// <summary>
    /// Calculate all biomes heat, humidity and other properties
    /// </summary>
    public static void DefineAllBiomes()
    {
        DefineBiomesBeforeClimate();
        HeatService.GlobalHeatLogic();
        RainfallEmitService.GlobalHumidityLogic();
        DefineBiomesAfterClimate();
    }

    public static void DefineBiomesBeforeClimate()
    {
        var allBiomes = Singles.Grid.GetAllBiomes();
        foreach(var biome in allBiomes)
        {
            biome.IsFarSpace = false;
            biome.BoundValues();
        }
        //Set inactive abysses
        var spaceBiomes = Singles.Grid.GetAllBiomes().Where(v => v.GetNearbyBiomesCache().All(a => a.IsAbyss));
        foreach (var space in spaceBiomes)
        {
            space.IsFarSpace = true;
        }
    }

    public static void DefineBiomesAfterClimate()
    {
        //Set heat and rainfall values
        var biomes = Singles.Grid.GetAllBiomes();
        foreach (var biome in biomes)
        {
            if (biome.Crust.Height == BiomeService.MaxHeight) biome.Weather.Heat = biome.Weather.Heat * 0.1f;
            if (biome.Crust.Height >= 1) biome.Weather.Heat -= biome.Crust.Height * 1.2f;
            biome.BoundValues();
        }
        RefershData();
    }

    public static void RefershData()
    {
        //Set heat and rainfall values
        var biomes = Singles.Grid.GetAllBiomes();
        foreach (var biome in biomes)
        {
            biome.Weather.HeatType = DefineHeat(biome);
            biome.Weather.RainfallType = DefineRainfall(biome);
        }

        SmoothTemperatureInEdges();

        foreach (var biome in biomes)
        {
            BiomeService.RefreshBiomeType(biome);
        }
    }

    /// <summary>
    /// Sometimes near edges there is single biome hotter then all other biomes near by.
    /// It does not make sense. But because of nature of this emmision algorithm it is not fixable.
    /// This algorithm is a bit random. And if biomes are very close to changing heat type there might be 
    /// one biome that pops out by being hotter and yet near abyss.
    /// </summary>
    private static void SmoothTemperatureInEdges()
    {
        var edgeBiomes = Singles.Grid.GetAllBiomes()
            .Where(biome => !biome.IsAbyss && biome.GetNearbyBiomesCache().Any(near => near.IsAbyss));

        var popingOutBiomes = edgeBiomes.Where(biome =>
            biome.GetNearbyBiomesCache(false)
                .Where(near => !near.IsAbyss)
                .All(near => near.Weather.HeatType < biome.Weather.HeatType)
            ).ToList();

        foreach (var popingOutBiome in popingOutBiomes)
        {
            var nearby = popingOutBiome
                .GetNearbyBiomesCache(false)
                .Where(near => !near.IsAbyss);

            if (nearby.Any())
            {
                popingOutBiome.Weather.Heat = nearby.Max(near => near.Weather.Heat);
                popingOutBiome.Weather.HeatType = DefineHeat(popingOutBiome);
            }
        }
    }

    public static HeatTypes DefineHeat(Biome biome)
    {
        if (biome.Weather.Heat < 1) return HeatTypes.Frozen;
        if (biome.Weather.Heat < 4) return HeatTypes.Cold;
        if (biome.Weather.Heat < 8) return HeatTypes.Warm;
        return HeatTypes.Hot;
    }
    
    public static RainfallTypes DefineRainfall(Biome biome)
    {
        var heatRainDiff = GetHeatRainDiff(biome);
        if (heatRainDiff < 2) return RainfallTypes.Arid;
        if (heatRainDiff < 4) return RainfallTypes.Dry;
        if (heatRainDiff < 7) return RainfallTypes.Humid;
        return RainfallTypes.Wet;
    }

    /// <summary>
    /// Will check based on climate what biome type should it be and set it.
    /// If biome type changes the biome looses some nature and all maturity.
    /// </summary>
    public static void RefreshBiomeType(Biome biome)
    {
        var newSpec = BiomeSpecs.FirstOrDefault(spec => spec.CanApply(biome));
        if (biome.Spec == newSpec) return;

        if (biome.Nature.Population > 1)
        {
            biome.Nature.Population = (biome.Nature.Population - 1).GetMin(1);
        }
        biome.Nature.Maturity = 0;

        biome.SetBiomeSpec(newSpec);
    }

    public static readonly int MaxHeight = 4;

    public static void TryCaptureNearby(Biome biome, Player player, bool isPassiveCapture)
    {
        var nearbyBiomes = biome.GetNearbyBiomesCache();
        foreach (var near in nearbyBiomes)
        {
            TryCapture(near, player, isPassiveCapture);
        }
        GoalService.CheckIfWinner(player);
    }

    public static bool TryCapture(Biome biome, Player player, bool isPassiveCapture)
    {
        if (biome.IsOwner(player)) return true;
        if (biome.Owner.IsNull())
        {
            biome.SetOwner(player);
            return true;
        }
        else
        { 
            if(isPassiveCapture)
            {
                return false;
            }
            else if (biome.IsWater)
            {
                return false;
            }
            else if (biome.Nature.Population < 1)
            {
                biome.SetOwner(player);
                return true;
            }
            return false;
        }
    }

    public static float GetHeatRainDiff(Biome biome)
    {
        var heatRate = biome.Weather.Heat / 10f;
        heatRate = heatRate.GetMinMax(0f, 1f);
        var modifierByHeat = 1f - heatRate;

        return biome.Weather.Humidity * 0.5f
            + biome.Weather.Humidity * 0.5f * modifierByHeat;
    }

    /// <summary>
    /// Return between 0.0 and 1.0
    /// </summary>
    public static float GetHeatRainDiffRate(Biome biome)
    {
        var diff = GetHeatRainDiff(biome);
        var rate = diff / 10f;
        return rate.GetMinMax(0, 1);
    }

    public static int GetDistance(Biome b1, Biome b2)
    {
        return Vector3HexHelper.Distance(b1.Key, b2.Key);
    }
}

