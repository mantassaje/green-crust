using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public abstract class WeatherEmitComplexHandler
{
    /// <summary>
    /// Total quantity that can be emited
    /// </summary>
    protected abstract float GetEmitionWell(Biome biome);
    protected abstract void AddEmission(Biome biome, float emitVal);
    protected abstract void Commit(Biome biome);
    protected abstract bool IsActive(Biome biome);

    protected int EmitDistance;
    protected int EmitQuality;
    protected float EmitToNearRate;

    public void DoEmisions()
    {
        var allBiomes = Singles.Grid.GetAllBiomes()
            .Where(IsActive)
            .ToList();

        for (int i = 0; i < EmitDistance; i++)
        {
            var withHumidityBiomes = allBiomes
                .Where(v => GetEmitionWell(v) >= 1)
                //Start from smallest
                //.OrderByDescending(v => GetEmitValue(v))
                .OrderBy(v => v.Weather.Sorter);

            for (int j = 0; j < EmitQuality; j++)
            {
                foreach (var biomeEmiter in withHumidityBiomes)
                {
                    EmitToNearbyBiomes(biomeEmiter);
                }
            }
        }
    }

    protected virtual void EmitToNearbyBiomes(Biome biomeEmiter)
    {
        var biomesReceivers = biomeEmiter.GetNearbyBiomesCache()
            .Where(biome => GetEmitionWell(biome) + GetValueToEmit(biomeEmiter) < GetEmitionWell(biomeEmiter)
                || biome == biomeEmiter)
            .OrderBy(v=>v.Weather.Sorter)
            .ToList();
        foreach (var biomeReceiver in biomesReceivers)
        {
            var addValue = GetValueToEmit(biomeEmiter, biomeReceiver);
            AddEmission(biomeEmiter, -addValue);
            AddEmission(biomeReceiver, addValue);
            Commit(biomeReceiver);
        }
        Commit(biomeEmiter);
    }

    /// <summary>
    /// Get value to emit.
    /// </summary>
    protected virtual float GetValueToEmit(Biome emiter)
    {
        return GetEmitionWell(emiter) * GetEmitRate();
    }

    /// <summary>
    /// Get value to emit based on both emiter and receiver.
    /// </summary>
    protected virtual float GetValueToEmit(Biome emiter, Biome receiver)
    {
        return GetValueToEmit(emiter) * GetBiomeAbsorbtion(receiver, emiter);
    }

    protected virtual float GetEmitRate()
    {
        return EmitToNearRate / EmitQuality;
    }

    protected virtual float GetBiomeAbsorbtion(Biome biome, Biome emiter)
    {
        var baseAbsorbtion = 1f - biome.Crust.GetHeightRatioSteped();
        baseAbsorbtion = baseAbsorbtion.GetMinMax(0f, 1f);
        return baseAbsorbtion;
    }
}

