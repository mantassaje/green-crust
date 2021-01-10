using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[Obsolete]
public static class RainfallAvgService
{
    public static void GlobalHumidityLogic()
    {
        ResetWorldHumidity();
        var allBiomes = Singles.Grid.GetAllBiomes();
        for (int i = 0; i < Singles.World.HumidityDistance; i++)
        {
            var withHumidityBiomes = allBiomes
                .Where(v => v.Weather.Humidity > 0)
                //Start from smallest
                .OrderByDescending(v => v.Weather.Humidity)
                .ToList();

            for (int j = 0; j < Singles.World.HumidityEmitQuality; j++)
            {
                foreach (var biomeEmiter in withHumidityBiomes)
                {
                    EmitHumidityToNearbyBiomes(biomeEmiter);
                }
                foreach (var biome in allBiomes)
                {
                    biome.Weather.CommitHumidity();
                }
            }
            Debug.Log(allBiomes.Sum(v => v.Weather.Humidity));
        }
    }

    private static void EmitHumidityToNearbyBiomes(Biome biomeEmiter)
    {
        var biomes = biomeEmiter.GetNearbyBiomesCache()
            .Where(v=>(v.Weather.Humidity <= biomeEmiter.Weather.Humidity
                || v == biomeEmiter))
            .ToList();
        var fullSystemHumidity = biomes.Sum(v => v.Weather.Humidity);
        var goalHumidityPerTile = fullSystemHumidity / biomes.Sum(v => GetBiomeAbsorbtion(v, biomeEmiter));
        foreach (var biome in biomes)
        {
            biome.Weather.SetHumidity(goalHumidityPerTile * GetBiomeAbsorbtion(biome, biomeEmiter));
        }
    }

    private static float GetBiomeAbsorbtion(Biome biome, Biome emiter)
    {
        var baseAbsorbtion = 1f - biome.Crust.GetHeightRatio();
        if (biome != emiter)
        {
            //To make emisions less flat
            //baseAbsorbtion *= 0.05f;

            //To make with more humidity loose less humidity
            /*var top = (emiter.Humidity / 5f) + (biome.Humidity / 5f);
            var bot = emiter.Humidity;
            var modifierRatio = top / bot;
            baseAbsorbtion *= modifierRatio;*/

            //var modifier = (emiter.Humidity - biome.Humidity) / emiter.Humidity;
            //baseAbsorbtion *= modifier;
        }
        return baseAbsorbtion;
    }

    private static void ResetWorldHumidity()
    {
        var biomes = Singles.Grid.GetAllBiomes();
        foreach(var biome in biomes)
        {
            if(biome.IsWater)
            {
                biome.Weather.ResetHumidity(Singles.World.OceanHumidityBase);
            }
            else
            {
                biome.Weather.ResetHumidity(0);
            }
        }
    }
}

