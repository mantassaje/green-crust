using System.Collections;
using System.Linq;


public abstract class WeatherEmitComplexHandler
{
    private const int _numberOfEdges = 6;

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
        var enumerator = DoEmisionsCoroutine();
        while (enumerator.MoveNext()) { }
    }

    public IEnumerator DoEmisionsCoroutine()
    {
        var allBiomes = Singles.Grid.GetAllBiomes()
            .Where(IsActive)
            .ToList();

        for (int i = 0; i < EmitDistance; i++)
        {
            var withHumidityBiomes = allBiomes
                .Where(v => GetEmitionWell(v) >= 1)
                //Sorting desc seems to produce more smooth transitions. But is sensetive to changes.
                //Sorting by random non chaning sorter seems to not so sensetive to changes.
                .OrderBy(v => v.Weather.Sorter)
                .ToList();

            for (int j = 0; j < EmitQuality; j++)
            {
                foreach (var biomeEmiter in withHumidityBiomes)
                {
                    EmitToNearbyBiomes(biomeEmiter);
                }
            }

            yield return null;
        }
    }

    protected virtual void EmitToNearbyBiomes(Biome biomeEmiter)
    {
        var emitionWell = GetEmitionWell(biomeEmiter);

        var biomesReceivers = biomeEmiter.GetNearbyBiomesCache()
            .Where(biomeReceiver => GetEmitionWell(biomeReceiver) + GetEdgeValueToEmit(emitionWell, biomeReceiver) < emitionWell
                && biomeReceiver != biomeEmiter);

        foreach (var biomeReceiver in biomesReceivers)
        {
            var addValue = GetEdgeValueToEmit(emitionWell, biomeReceiver);
            AddEmission(biomeEmiter, -addValue);
            AddEmission(biomeReceiver, addValue);
            Commit(biomeReceiver);
        }
        Commit(biomeEmiter);
    }

    /// <summary>
    /// Get value to emit based on both emiter and receiver.
    /// </summary>
    protected virtual float GetEdgeValueToEmit(float emiterTotalWell, Biome receiver)
    {
        return emiterTotalWell * EmitToNearRate * GetBiomeAbsorbtion(receiver) / _numberOfEdges / EmitQuality;
    }

    protected virtual float GetBiomeAbsorbtion(Biome biome)
    {
        var baseAbsorbtion = 1f - biome.Crust.GetHeightRatioSteped();
        baseAbsorbtion = baseAbsorbtion.GetMinMax(0f, 1f);
        return baseAbsorbtion;
    }
}

