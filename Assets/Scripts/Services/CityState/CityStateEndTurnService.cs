using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public static class CityStateEndTurnService
{
    public static void EndTurn()
    {
        var biomes = Singles.Grid.GetAllBiomes().Where(biome => !biome.CityState.IsDead)
            .OrderBy(biome => biome.Weather.Sorter)//TODO should be some common sorter that all can use.
            .ToList();

        var unexplorableBiomes = biomes.Where(biome => !CityStateInfoService.CanBeExplored(biome)).ToList();
        foreach(var biome in unexplorableBiomes)
        {
            biome.CityState.Population = 0;
            biomes.Remove(biome);
        }

        //Set base grwoth. Prepare data.
        foreach (var biome in biomes)
        {
            biome.CityState.LastAge = biome.CityState.CurrentAge;
            biome.CityState.CurrentAge = new CityStateData(
                CityStateInfoService.GetMaxTradeActions(biome),
                CityStateInfoService.GetMaxRaidActions(biome),
                CityStateInfoService.GetMaxMissionaryActions(biome)
            );

            var growth = CityStateInfoService.GetBaseGrowth(biome);
            biome.CityState.CurrentAge.NaturalGrowth = growth;
            biome.CityState.IsRuinTurns = (biome.CityState.IsRuinTurns - 1).GetMin(0);
            biome.CityState.IsOnFire = false;
            biome.CityState.EventIsRead = false;
            biome.CityState.Event = null;
            biome.CityState.EventStatus = CityStateEventStatus.Ignored;
        }

        //Explore, trade, raid.
        foreach (var biome in biomes)
        {
            CityStateService.ValidateExploredBiomes(biome);
            CityStateAiService.Explore(biome);
        }

        for (int i = 0; i < 10; i++)
        {
            foreach (var biome in biomes)
            {
                CityStateAiService.ActOnce(biome);
            }
        }

        //Apply growth.
        foreach (var biome in biomes)
        {
            if (float.IsNaN(biome.CityState.Population))
            {
                Debug.LogError("Population is already NAN!!!", biome);
            }

            var netGrowth = CityStateInfoService.GetNetGrowth(biome);
            if (float.IsNaN(netGrowth))
            {
                Debug.LogError("netGrowth is NAN!!!", biome);
            }

            biome.CityState.Population += netGrowth;

            //If not notable then check if it was notable previous turn. If so then show ruins sprite for few turns.
            if (!biome.CityState.IsNotable)
            {
                biome.CityState.IsRuinTurns = biome.CityState.LastAge.IsNotable ? 2 : 0;
            }
        }

        //Mute ones that are near notable city state.
        foreach (var biome in biomes)
        {
            if (biome.GetNearbyBiomesCache().Any(nearby => nearby.CityState.IsNotable && nearby != biome))
            {
                biome.CityState.IsMuted = true;
            }
            else
            {
                biome.CityState.IsMuted = false;
            }

            biome.CityState.CurrentAge.IsNotable = biome.CityState.IsNotable;
        }

        //Apply events
        foreach (var biome in biomes.Where(value => value.CityState.IsVisible))
        {
            if (UnityEngine.Random.Range(0f, 1f) < 0.35f)
            {
                CityStateStoryService.SetRandomEvent(biome);
            }
        }

        Debug.Log($"Age {Singles.World.Turn} city states done.");
    }
}

