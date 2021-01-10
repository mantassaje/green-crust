using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class RainfallEmitHandler : WeatherEmitComplexHandler
{
    public RainfallEmitHandler()
    {
        EmitDistance = Singles.World.HumidityDistance;
        EmitQuality = Singles.World.HumidityEmitQuality;
        EmitToNearRate = Singles.World.HumidityEmitToNearbyConst;
    }

    protected override void AddEmission(Biome biome, float rainfall)
    {
        biome.Weather.AddHumidity(rainfall);
    }

    protected override void Commit(Biome biome)
    {
        biome.Weather.CommitHumidity();
    }

    protected override float GetEmitionWell(Biome biome)
    {
        return biome.Weather.Humidity;
    }

    protected override bool IsActive(Biome biome)
    {
        return !biome.IsFarSpace;
    }
}

