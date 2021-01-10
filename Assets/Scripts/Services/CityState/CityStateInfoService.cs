using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public static class CityStateInfoService
{
    public static float GetBaseGrowth(Biome biome)
    {
        //Biomes in edges will emerge slower.
        var surroundings = biome.GetNearbyBiomesCache();
        var basePopGrwoth = surroundings.Sum(GetSingleBiomeGrowth);
        //To reduce bonus from additional population. It is too powerfull.
        var bonusPerPop = basePopGrwoth * CityStateConstant.BonusPerPopMultiplyer;

        var result = basePopGrwoth + bonusPerPop * biome.CityState.Population.GetMinMax(0f, 4f);
        result *= CityStateConstant.NatureGrowthMultiplyer;
        return result.GetMin(0f);
    }

    /// <summary>
    /// Growth minus losses and population consumption.
    /// </summary>
    /// <param name="cityBiome"></param>
    /// <returns></returns>
    public static float GetNetGrowth(Biome cityBiome)
    {
        var growth = cityBiome.CityState.CurrentAge.TotalGrowth - (cityBiome.CityState.Population * CityStateConstant.PopulationConsumptionMultiplyer);
        
        //If in starvation then kill population quickly to not be stuck with permanent text that population is starving.
        if (growth < 0)
        {
            return growth * 2f;
        }
        else
        {
            return growth;
        }
    }

    private static float GetSingleBiomeGrowth(Biome biome)
    {
        if (biome.IsWater)
        {
            //Same quantity as forests.
            return 3f;
        }
        else
        {
            return (biome.Nature.Population - 1).GetMin(0);
        }
    }

    /// <summary>
    /// Maybe it should not be supper big so that trading cities whould be forced to choose less prone to trade cities and those would grow.
    /// </summary>
    public static float GetExploredCap(Biome cityBiome)
    {
        return cityBiome.CityState.Population + 2f;
    }

    public static float GetTotalExploreCost(Biome cityBiome)
    {
        return cityBiome.CityState.Explored.Sum(GetBiomeExploreCost);
    }

    public static float GetBiomeExploreCost(Biome biome)
    {
        if (biome.IsWater) return 0.3f;
        if (biome.Weather.RainfallType == RainfallTypes.Dry) return 0.5f;
        if (biome.Weather.RainfallType == RainfallTypes.Humid) return 1f;
        if (biome.Weather.RainfallType == RainfallTypes.Wet) return 2f;
        return 1f;
    }

    /// <summary>
    /// Total trades that the city can initaite.
    /// </summary>
    public static int GetMaxTradeActions(Biome cityBiome)
    {
        var add = 0;
        if (cityBiome.CityState.Population >= 4)
            add = 2;

        if (cityBiome.CityState.Personality.Trading.Rate <= CityStateConstant.PersonalityFaded)
            return 0;

        if (cityBiome.CityState.Personality.Trading.Rate <= CityStateConstant.PersonalityLow)
            return 2 + add;

        if (cityBiome.CityState.Personality.Trading.Rate <= CityStateConstant.PersonalityAvarage)
            return 4 + add;

        if (cityBiome.CityState.Personality.Trading.Rate <= CityStateConstant.PersonalityHigh)
            return 6 + add;

        return 8 + add;
    }

    /// <summary>
    /// Total raids that the state can initaite.
    /// </summary>
    public static int GetMaxRaidActions(Biome cityBiome)
    {
        var add = 0;
        if (cityBiome.CityState.Population >= 4)
            add = 1;

        if (cityBiome.CityState.Personality.Rading.Rate > CityStateConstant.PersonalityHigh)
            return 4 + add;

        if (cityBiome.CityState.Personality.Rading.Rate > CityStateConstant.PersonalityAvarage)
            return 2 + add;

        /*if (cityBiome.CityState.Personality.Rading.Rate > CityStateConstant.PersonalityLow)
        {
            var result = new List<int> { 0, 1 }.PickRandom();
            return result;
        }*/

        return 0;
    }

    public static int GetMaxMissionaryActions(Biome cityBiome)
    {
        var add = 0;
        if (cityBiome.CityState.Population >= 4)
            add = 1;

        if (cityBiome.CityState.Personality.Worshiper.Rate > CityStateConstant.PersonalityHigh)
            return 2 + add;

        if (cityBiome.CityState.Personality.Worshiper.Rate > CityStateConstant.PersonalityAvarage)
            return 1 + add;

        return 0;
    }

    /// <summary>
    /// Should be affected by population. Small state should not get massive growth add from big state.
    /// Small states trade will be capped by its own low selling/buying power.
    /// Big state will also not gonna be able to get massive trade from small states because of small offers.
    /// There is no pop in equation, but bigger pop generates bigger base growth witch as a result increases trade power.
    /// </summary>
    public static float GetBaseProfit(Biome marketBiome)
    {
        // Sqrt to make small traders a bit more usefull. They trade way to little.
        return (float)Math.Sqrt(marketBiome.CityState.Personality.Trading.Rate) * marketBiome.CityState.CurrentAge.TotalGrowth.GetMin(0f) * CityStateConstant.BaseTradeMultiplyer;
    }

    public static float GetTradeValue(Biome market, Biome initiator)
    {
        var marketValue = GetBaseProfit(market);
        var initiatorValue = GetBaseProfit(initiator);

        return (marketValue + marketValue + initiatorValue) / 3f;
    }

    /// <summary>
    /// Let smaller market to profit more.
    /// </summary>
    public static float GetMarketProfit(Biome market, Biome client)
    {
        // Get pop diff. If market smaller number should be positive.
        // Max diff to 4. Min to 0. Divide by 4 to get diff rate.
        // Base * DiffRate + Base.
        var popDifference = (client.CityState.Population - market.CityState.Population).GetMinMax(0f, 4f);
        var popDifferenceRate = popDifference / 4f;
        var profitBase = GetBaseProfit(market);
        var profit = profitBase * popDifferenceRate + profitBase;
        return profit;
    }

    /// <summary>
    /// Let smaller client to profit more.
    /// </summary>
    public static float GetClientProfit(Biome market, Biome client)
    {
        // Get pop diff. If client smaller number should be positive.
        // Max diff to 4. Min to 0. Divide by 4 to get diff rate.
        // Base * DiffRate + Base.
        var popDifference = (market.CityState.Population - client.CityState.Population).GetMinMax(0f, 4f);
        var popDifferenceRate = popDifference / 4f;
        var profitBase = GetBaseProfit(market);
        var profit = profitBase * popDifferenceRate + profitBase;
        return profit;
    }

    public static float GetDefense(Biome cityBiome)
    {
        return cityBiome.CityState.Population * cityBiome.CityState.Personality.Rading.Rate * CityStateConstant.DefenseMultiplyer;
    }

    public static float GetAttack(Biome cityBiome)
    {
        return cityBiome.CityState.Population * cityBiome.CityState.Personality.Rading.Rate * CityStateConstant.AttackMultiplyer;
    }

    /// <summary>
    /// Image as the max booty raiders can carry.
    /// </summary>
    public static float GetMaxCarryBooty(Biome raiderBiome, bool isGuerrillaRaid)
    {
        var stealPower = isGuerrillaRaid 
            ? CityStateConstant.GuerrillaRaidCarryBootyMultiplyer 
            : CityStateConstant.FullRaidCarryBootyMultiplyer;
        return raiderBiome.CityState.Population * raiderBiome.CityState.Personality.Rading.Rate * stealPower;
    }

    /// <summary>
    /// Imagine as the max booty that can be taken from city.
    /// </summary>
    /// <returns></returns>
    public static float GetMaxBooty(Biome victimBiome, bool isGuerrillaRaid)
    {
        var stealPower = isGuerrillaRaid
            ? CityStateConstant.GuerrillaRaidBootyMultiplyer
            : CityStateConstant.FullRaidBootyMultiplyer;
        return victimBiome.CityState.CurrentAge.TotalGrowth.GetMin(0f) * stealPower;
    }

    /// <summary>
    /// Imagine as things that raider would burn if city attack was succesfull.
    /// Should not happen on guerrilla.
    /// </summary>
    /// <returns></returns>
    public static float GetBurn(Biome victimBiome)
    {
        return victimBiome.CityState.CurrentAge.TotalGrowth.GetMin(0f) * CityStateConstant.RaidBurnMultiplyer;
    }

    public static int GetSpriteIndex(CityState cityState)
    {
        if (cityState.Population < 1f) return 0;
        if (cityState.Population < 2f) return 1;
        if (cityState.Population < 4f) return 2;
        if (cityState.Population < 8f) return 3;
        return 4;
    }

    public static bool CanBeExplored(Biome biome)
    {
        if (biome.Spec.IsBarren
            || biome.Spec.IsDead)
            return false;

        if (biome.Crust.Height >= CityStateConstant.InpassableHeight)
            return false;

        if (biome.Spec.GetType() == typeof(IcaCapOceanBiomeSpec))
            return false;

        return true;
    }
}

