using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public static class CityStateActionService
{
    /// <summary>
    /// Can not explore sea biome from ground and vice versa.
    /// </summary>
    public static void ExploreNewBiome(Biome cityBiome)
    {
        if (!cityBiome.CityState.Explored.Contains(cityBiome))
        {
            cityBiome.CityState.Explored.Add(cityBiome);
        }

        var toExploreBiomes= cityBiome.CityState.Explored.SelectMany(explored => explored.GetNearbyBiomesCache().Where(nearby => nearby.IsWater == explored.IsWater))
            .Where(nearby => !cityBiome.CityState.Explored.Contains(nearby))
            .Where(CityStateInfoService.CanBeExplored)
            .ToList();

        var toExploreBiome = toExploreBiomes.PickRandom(1).FirstOrDefault();
        if (toExploreBiome == null) return;

        cityBiome.CityState.Explored.Add(toExploreBiome);
    }

    /// <summary>
    /// Explore near city sea biome. Then state will be able to explore the seas from here.
    /// </summary>
    public static bool ExploreNearCityWaters(Biome cityBiome)
    {
        var unexploredWaters = GetUnexploredNearbyWaters(cityBiome);

        if (unexploredWaters.Count() == 0) return false;

        var toExploreBiome = unexploredWaters.PickRandom(1).FirstOrDefault();
        if (toExploreBiome == null) return false;

        cityBiome.CityState.Explored.Add(toExploreBiome);

        return true;
    }

    public static List<Biome> GetUnexploredNearbyWaters(Biome cityBiome)
    {
        var nearbyWaters = cityBiome.GetNearbyBiomesCache().Where(nearby => nearby.IsWater);

        var unexploredWaters = nearbyWaters.Where(water => !cityBiome.CityState.Explored.Contains(water))
            .Where(CityStateInfoService.CanBeExplored);

        return unexploredWaters.ToList();
    }

    public static void Trade(Biome cityBiome)
    {
        if (cityBiome.CityState.CurrentAge.TradeActionCount.IsOver)
            return;

        var tradeCity = cityBiome.CityState.Explored.SelectMany(expolred => expolred.GetNearbyBiomesCache())
            .Distinct()
            .Where(other => !other.CityState.IsDead
                && other != cityBiome
                && !cityBiome.CityState.CurrentAge.Trades.Any(trade => trade.Client == cityBiome))
            .OrderByDescending(other => CityStateInfoService.GetClientProfit(other, cityBiome))
            .FirstOrDefault();

        if (tradeCity == null) return;

        cityBiome.CityState.CurrentAge.TradeActionCount.Tick();
        ApplyTradeGrowth(cityBiome, tradeCity);
    }

    public static void ApplyTradeGrowth(Biome client, Biome market)
    {
        var marketProfit = CityStateInfoService.GetMarketProfit(market, client) * CityStateConstant.InitiatedTradeMultiplyer;
        var clientProfit = CityStateInfoService.GetClientProfit(market, client) * CityStateConstant.MarketTradeMultiplyer;

        if (float.IsNaN(marketProfit))
        {
            Debug.LogError("tradeGrowth is NAN!!!", client);
        }
        if (float.IsNaN(clientProfit))
        {
            Debug.LogError("clientProfit is NAN!!!", client);
        }

        client.CityState.CurrentAge.ClientTradeGrowth += clientProfit;
        market.CityState.CurrentAge.MarketTradeGrowth += marketProfit;

        if (float.IsNaN(client.CityState.CurrentAge.ClientTradeGrowth))
        {
            Debug.LogError("InitiatedTradeGrowth is NAN!!!", client);
        }
        if (float.IsNaN(client.CityState.CurrentAge.MarketTradeGrowth))
        {
            Debug.LogError("WondererTradeGrowth is NAN!!!", client);
        }

        var tradeData = new TradeData()
        {
            Market = market,
            Client = client,
            MarketGain = marketProfit,
            ClientGain = clientProfit,
        };

        client.CityState.CurrentAge.Trades.Add(tradeData);
        market.CityState.CurrentAge.Trades.Add(tradeData);

        //To fix up all tribes being with raid 1 and trader trading with them and getting zero.
        if (market.CityState.Personality.Trading.Rate <= CityStateConstant.PersonalityLow)
        {
            //market.CityState.Personality.Trading.Add(0.1f);
        }

        Debug.Log(
            new
            {
                MarketName = tradeData.Market.CityState.Name,
                MarketPop = tradeData.Market.CityState.Population.GetRounded(),
                MarketGain = tradeData.MarketGain.GetRounded(),
                ClientName = tradeData.Client.CityState.Name,
                ClientPop = tradeData.Client.CityState.Population.GetRounded(),
                ClientGain = tradeData.ClientGain.GetRounded()
            }
        );
    }

    public static void GoToMissionary(Biome missionerBiome)
    {
        if (missionerBiome.CityState.CurrentAge.MissionaryActionCount.IsOver)
            return;

        var missionToCity = missionerBiome.CityState.Explored.SelectMany(expolred => expolred.GetNearbyBiomesCache())
            .Where(other => !other.CityState.IsDead
                && other != missionerBiome
                && other.CityState.Personality.Worshiper.Rate <= CityStateConstant.PersonalityLow)
            .OrderBy(other => other.CityState.Personality.Worshiper.Rate)
            .FirstOrDefault();

        if (missionToCity != null)
        {
            //missionToCity.CityState.Personality.Worshiper.Add(0.1f);
            missionerBiome.CityState.CurrentAge.MissionaryActionCount.Tick();
        }
    }

    /// <summary>
    /// Choose city state to raid and then choose what type of attack to do. Either ambushes or full attack on city walls.
    /// </summary>
    public static void Raid(Biome raiderBiome)
    {
        if (raiderBiome.CityState.CurrentAge.RaidActionCount.IsOver)
            return;

        var victimBiome = raiderBiome.CityState.Explored.SelectMany(expolred => expolred.GetNearbyBiomesCache())
            .Distinct()
            .Where(other => !other.CityState.IsDead
                && other != raiderBiome
                && PossibleToWin(raiderBiome, other))
                //TODO should city trade with his raider?
                //&& !raiderBiome.CityState.CurrentAge.InitiatedTradeWith.Contains(other))
            .OrderByDescending(victim => GetAttackDesirePoints(raiderBiome, victim))
            .FirstOrDefault();

        if (victimBiome == null) return;

        raiderBiome.CityState.CurrentAge.RaidActionCount.Tick();
        ApplyRaid(raiderBiome, victimBiome);
    }

    /// <summary>
    /// Full attack on city walls.
    /// </summary>
    public static void ApplyCitySack(Biome raiderBiome, Biome victimBiome)
    {
        var baseAttack = CityStateInfoService.GetAttack(raiderBiome);
        var baseDefense = CityStateInfoService.GetDefense(victimBiome);
        
        var realAttack = UnityEngine.Random.Range(0.8f, 1f) * baseAttack;
        // Attacks should be risky
        var realDefense = UnityEngine.Random.Range(0.8f, 1.4f) * baseDefense;
        var success = realAttack > realDefense;
        var raidData = new RaidData()
        {
            GuerrillaRaid = false,
            RaiderBiome = raiderBiome,
            VictimBiome = victimBiome,
            RaidIsSuccess = success
        };

        var raidedBooty = 0f;

        if (success)
        {
            victimBiome.CityState.Population *= CityStateConstant.FullRaidVictimPopModifier;
            victimBiome.CityState.IsOnFire = true;

            raidedBooty = Math.Min(
                CityStateInfoService.GetMaxCarryBooty(raiderBiome, false),
                CityStateInfoService.GetMaxBooty(victimBiome, false)
            );
            victimBiome.CityState.CurrentAge.BurnedGrowth += CityStateInfoService.GetBurn(victimBiome);
            victimBiome.CityState.CurrentAge.LostGrowth += raidedBooty;
            raiderBiome.CityState.CurrentAge.BootyGrowth += raidedBooty;
            raidData.Booty = raidedBooty;

            /*if (victimBiome.CityState.Personality.Rading.Rate <= CityStateConstant.PersonalityLow)
                victimBiome.CityState.Personality.Rading.AddFromCommonFolk(0.1f);
            else victimBiome.CityState.Personality.Rading.AddFromCommonFolk(0.05f);*/
        }
        else
        {
            //raiderBiome.CityState.Population *= CityStateConstant.FullRaidRaiderPopModifier;
            // So that tribes would not end up all full agressvive
            //if (!raiderBiome.CityState.IsNotable)
            //    raiderBiome.CityState.Personality.Rading.AddHalfFromCommonFolk(-0.1f);
        }

        victimBiome.CityState.CurrentAge.DefendedFromCities.Add(raidData);
        raiderBiome.CityState.CurrentAge.RaidedCities.Add(raidData);

        if (raiderBiome.CityState.IsNotable || victimBiome.CityState.IsNotable)
        {
            if (success) Debug.Log($"{raiderBiome.CityState.Name} attacks {victimBiome.CityState.Name}. Full raid success.");
            else Debug.Log($"{raiderBiome.CityState.Name} attacks {victimBiome.CityState.Name}. Full raid failed.");
            Debug.Log(new
            {
                AttackerPop = raiderBiome.CityState.Population.GetRounded(),
                VictimPop = victimBiome.CityState.Population.GetRounded(),
                AttackerBaseAttack = baseAttack.GetRounded(),
                VictimBaseDefense = baseDefense.GetRounded(),
                AttackerRealAttack = realAttack.GetRounded(),
                VictimRealDefense = realDefense.GetRounded(),
                RaidedBooty = raidedBooty.GetRounded()
            });
        }
    }

    /// <summary>
    /// Low risk low reward. Tell stories as attacks around city.
    /// </summary>
    public static void ApplyGuerrillaRaid(Biome raiderBiome, Biome victimBiome)
    {
        var baseAttack = CityStateInfoService.GetAttack(raiderBiome);
        var baseDefense = CityStateInfoService.GetDefense(victimBiome);
        
        var realAttack = UnityEngine.Random.Range(0.75f, 1f);
        var realDefense = UnityEngine.Random.Range(0.25f, 1f);
        var success = realAttack > realDefense;
        var raidData = new RaidData()
        {
            GuerrillaRaid = true,
            RaiderBiome = raiderBiome,
            VictimBiome = victimBiome,
            RaidIsSuccess = success
        };

        var raidedBooty = 0f;

        if (success)
        {
            raidedBooty = Math.Min(
                CityStateInfoService.GetMaxCarryBooty(raiderBiome, true),
                CityStateInfoService.GetMaxBooty(victimBiome, true)
            );
            victimBiome.CityState.CurrentAge.LostGrowth += raidedBooty;
            raiderBiome.CityState.CurrentAge.BootyGrowth += raidedBooty;
            raidData.Booty = raidedBooty;
        }
        else
        {
            // So that tribes would not end up all full agressvive
            //if (!raiderBiome.CityState.IsNotable)
            //    raiderBiome.CityState.Personality.Rading.AddHalfFromCommonFolk(-0.05f);
        }

        if (raiderBiome.CityState.IsNotable || victimBiome.CityState.IsNotable)
        {
            if (success) Debug.Log($"{raiderBiome.CityState.Name} attacks {victimBiome.CityState.Name}. Guerrilla raid success.");
            else Debug.Log($"{raiderBiome.CityState.Name} attacks {victimBiome.CityState.Name}. Guerrilla raid failed.");
            Debug.Log(new
            {
                AttackerPop = raiderBiome.CityState.Population.GetRounded(),
                VictimPop = victimBiome.CityState.Population.GetRounded(),
                AttackerBaseAttack = baseAttack.GetRounded(),
                VictimBaseDefense = baseDefense.GetRounded(),
                AttackerRealAttack = realAttack.GetRounded(),
                VictimRealDefense = realDefense.GetRounded(),
                RaidedBooty = raidedBooty.GetRounded()
            });
        }

        victimBiome.CityState.CurrentAge.DefendedFromCities.Add(raidData);
        raiderBiome.CityState.CurrentAge.RaidedCities.Add(raidData);
    }

    public static void ApplyRaid(Biome raiderBiome, Biome victimBiome)
    {
        var baseAttack = CityStateInfoService.GetAttack(raiderBiome);
        var baseDefense = CityStateInfoService.GetDefense(victimBiome);

        // Tribes should not be able to rize notables.
        var canSackCity = raiderBiome.CityState.IsNotable
            || !victimBiome.CityState.IsNotable;

        if (canSackCity
            && baseAttack > baseDefense)
        {
            ApplyCitySack(raiderBiome, victimBiome);
        }
        else
        {
            ApplyGuerrillaRaid(raiderBiome, victimBiome);
        }
    }

    /// <summary>
    /// Used by city state AI to pick city to attack.
    /// For consistency to understand always divide attack to defense
    /// </summary>
    public static float GetAttackDesirePoints(Biome raiderBiome, Biome victimBiome)
    {
        return victimBiome.CityState.CurrentAge.TotalGrowth;
    }

    public static bool PossibleToWin(Biome raiderBiome, Biome victimBiome)
    {
        var attack = CityStateInfoService.GetAttack(raiderBiome);
        var defense = CityStateInfoService.GetDefense(victimBiome);

        return (attack / defense) > 0.7f;
    }
}
