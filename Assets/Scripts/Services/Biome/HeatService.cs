using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public static class HeatService
{
    public static void GlobalHeatLogic()
    {
        ResetWorldHeat();
        var rainfallHandler = new ColdEmitHandler();
        rainfallHandler.DoEmisions();
    }

    private static void ResetWorldHeat()
    {
        var biomes = Singles.Grid.GetAllBiomes();
        foreach (var biome in biomes)
        {
            if (biome.IsAbyss)
            {
                biome.Weather.ResetCold(Singles.World.AbyssColdBase);
            }
            else
            {
                biome.Weather.ResetCold(0);
            }
        }
    }
}

