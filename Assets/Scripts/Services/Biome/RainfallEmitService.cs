using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public static class RainfallEmitService
{
    public static void GlobalHumidityLogic()
    {
        ResetWorldHumidity();
        var rainfallHandler = new RainfallEmitHandler();
        rainfallHandler.DoEmisions();
    }

    private static void ResetWorldHumidity()
    {
        var biomes = Singles.Grid.GetAllBiomes();
        foreach(var biome in biomes)
        {
            if(biome.IsWater)
            {
                var rainfallRate = biome.Weather.GetHeatRate();
                rainfallRate += 0.2f;
                rainfallRate = rainfallRate.GetMinMax(0, 1);
                biome.Weather.ResetHumidity(Singles.World.OceanHumidityBase * rainfallRate);
            }
            else
            {
                biome.Weather.ResetHumidity(0);
            }
        }
    }
}

