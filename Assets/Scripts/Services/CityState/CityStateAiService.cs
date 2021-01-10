using UnityEngine;

public static class CityStateAiService
{
    public static void Explore(Biome cityBiome)
    {
        if (CityStateInfoService.GetExploredCap(cityBiome) > CityStateInfoService.GetTotalExploreCost(cityBiome))
        {
            if (Random.Range(1, 2) == 1)
            {
                var success = CityStateActionService.ExploreNearCityWaters(cityBiome);
                if (!success) CityStateActionService.ExploreNewBiome(cityBiome);
            }
            else
            {
                CityStateActionService.ExploreNewBiome(cityBiome);
            }
        }
    }

    public static void ActOnce(Biome cityBiome)
    {
        if (!cityBiome.CityState.CurrentAge.RaidActionCount.IsOver)
        {
            CityStateActionService.Raid(cityBiome);
        }
        else if (!cityBiome.CityState.CurrentAge.TradeActionCount.IsOver)
        {
            CityStateActionService.Trade(cityBiome);
        }
        else if (!cityBiome.CityState.CurrentAge.MissionaryActionCount.IsOver)
        {
            CityStateActionService.GoToMissionary(cityBiome);
        }
    }
}

