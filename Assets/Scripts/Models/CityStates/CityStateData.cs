using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class CityStateData
{
    public TallyCounter TradeActionCount;
    public TallyCounter RaidActionCount;
    public TallyCounter MissionaryActionCount;

    public float NaturalGrowth;

    /// <summary>
    /// Your traders traveled to other city.
    /// </summary>
    public float ClientTradeGrowth;

    /// <summary>
    /// Someones trader traveled to your city.
    /// </summary>
    public float MarketTradeGrowth;

    /// <summary>
    /// Stolen from other city.
    /// </summary>
    public float BootyGrowth;

    /// <summary>
    /// Lost from raids and will be subtracted from growth.
    /// This growth went to other city.
    /// </summary>
    public float LostGrowth;

    /// <summary>
    /// Burned from raids and will be subtracted from growth.
    /// Growth burned and nobody got it.
    /// </summary>
    public float BurnedGrowth;

    public bool IsNotable;

    /// <summary>
    /// This should be used by population logic.
    /// </summary>
    public float TotalGrowth
    {
        get
        {
            return NaturalGrowth + ClientTradeGrowth + MarketTradeGrowth + BootyGrowth - TotalLoss;
        }
    }

    public float TotalLoss
    {
        get
        {
            return LostGrowth + BurnedGrowth;
        }
    }

    /// <summary>
    /// This should be used by story logic when needed.
    /// DO NOT USE for population logic.
    /// </summary>
    public float TotalGrowthWithoutLostGrowth
    {
        get
        {
            return NaturalGrowth + ClientTradeGrowth + MarketTradeGrowth + BootyGrowth;
        }
    }

    public List<TradeData> Trades { get; set; }

    public List<RaidData> RaidedCities { get; set; }
    public List<RaidData> DefendedFromCities { get; set; }

    public CityStateData(int maxTradeActions, int maxRaidActions, int maxMissionaryActions)
    {
        TradeActionCount = new TallyCounter(maxTradeActions);
        RaidActionCount = new TallyCounter(maxRaidActions);
        MissionaryActionCount = new TallyCounter(maxMissionaryActions);

        Trades = new List<TradeData>();

        RaidedCities = new List<RaidData>();
        DefendedFromCities = new List<RaidData>();
    }
}

