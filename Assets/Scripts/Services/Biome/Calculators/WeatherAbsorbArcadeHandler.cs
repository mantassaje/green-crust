using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public abstract class WeatherAbsorbArcadeHandler
{
    protected abstract float GetValueFunc(Biome biome);
    protected abstract void SetAction(Biome biome, float emitVal);
    protected abstract void CommitAction(Biome biome);

    public void DoEmisions()
    {
        var allBiomes = Singles.Grid.GetAllBiomes();
        for (int i = 0; i < Singles.World.HumidityDistance; i++)
        {
            var withEmiterNeighbours = allBiomes
                .Where(v => !v.IsAbyss
                    && GetValueFunc(v) == 0
                    && v.GetNearbyBiomesCache().Any(n=>GetValueFunc(n) > 0))
                .ToList();

            foreach (var biomeAbsorber in withEmiterNeighbours)
            {
                AbsorbFromNearby(biomeAbsorber);
            }
            foreach (var biomeAbsorber in withEmiterNeighbours)
            {
                CommitAction(biomeAbsorber);
            }
        }
    }

    protected virtual void AbsorbFromNearby(Biome biomeAbsorber)
    {
        var biomeValSum = biomeAbsorber.GetNearbyBiomesCache()
            .Sum(v => GetValueFunc(v));
        var rateOfMax = biomeValSum / 80f;
        var newValue = 10f * rateOfMax - 0.05f;
        newValue.GetMinMax(0, 10);
        newValue = newValue * GetBiomeAbsorbtion(biomeAbsorber);
        SetAction(biomeAbsorber, newValue);
    }

    protected virtual float GetBiomeAbsorbtion(Biome biome)
    {
        var baseAbsorbtion = biome.Crust.GetHeightRatio();
        baseAbsorbtion = baseAbsorbtion.GetMinMax(0f, 0.9f);
        return baseAbsorbtion;
    }
}

