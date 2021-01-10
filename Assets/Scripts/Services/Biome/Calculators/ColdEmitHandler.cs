using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class ColdEmitHandler : WeatherEmitComplexHandler
{
    public ColdEmitHandler()
    {
        EmitDistance = Singles.World.ColdDistance;
        EmitQuality = Singles.World.ColdEmitQuality;
        EmitToNearRate = Singles.World.ColdEmitToNearbyConst;
    }

    protected override void AddEmission(Biome biome, float cold)
    {
        biome.Weather.AddCold(cold);
    }

    protected override void Commit(Biome biome)
    {
        biome.Weather.CommitCold();
    }

    protected override float GetEmitionWell(Biome biome)
    {
        return biome.Weather.Cold;
    }

    protected override bool IsActive(Biome biome)
    {
        return !biome.IsFarSpace;
    }
}

