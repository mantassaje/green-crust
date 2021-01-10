using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class BiomeDebugCard : ManualUpdateBehaviour
{
    public Text Text;
    public Text CityName;
    public bool ShowStats;
    
    public override void ManualUpdate()
    {
        var biome = Singles.PlayerController.SelectedBiome;
        if (biome.IsNotNull())
        {
            CityName.text = biome.CityState.Name;
            if (ShowStats && Application.isEditor)
            {
                Text.text = biome.CityState.Name + Environment.NewLine;
                Text.text += "Population " + biome.CityState.Population.GetRounded();
                Text.text += ", BaseGrowth " + biome.CityState.CurrentAge.NaturalGrowth.GetRounded();
                Text.text += ", InitTrade " + biome.CityState.CurrentAge.ClientTradeGrowth.GetRounded();
                Text.text += ", Market " + biome.CityState.CurrentAge.MarketTradeGrowth.GetRounded();
                Text.text += ", Booty " + biome.CityState.CurrentAge.BootyGrowth.GetRounded();
                Text.text += ", Lost " + biome.CityState.CurrentAge.LostGrowth.GetRounded();
                Text.text += ", Burned " + biome.CityState.CurrentAge.BurnedGrowth.GetRounded();
                Text.text += ", TOTAL " + biome.CityState.CurrentAge.TotalGrowth.GetRounded();
                Text.text += ", NET " + CityStateInfoService.GetNetGrowth(biome).GetRounded() + Environment.NewLine + Environment.NewLine;
                Text.text += "TradePower " + CityStateInfoService.GetBaseProfit(biome).GetRounded() + Environment.NewLine + Environment.NewLine;
                Text.text += "Worshiper " + biome.CityState.Personality.Worshiper.Rate.GetRounded();
                Text.text += ", Trading " + biome.CityState.Personality.Trading.Rate.GetRounded();
                Text.text += ", Rading " + biome.CityState.Personality.Rading.Rate.GetRounded();
                Text.text += ", Comn. Folk " + biome.CityState.Personality.CommonFolk.Rate.GetRounded() + Environment.NewLine + Environment.NewLine;
                //Text.text += "Explored " + CityStateInfoService.GetExploredCap(biome).GetRounded() + "/" + CityStateInfoService.GetTotalExploreCost(biome).GetRounded() + Environment.NewLine;
            }
            else
            {
                Text.text = CityStateStoryService.GetDescription(biome);
            }
        }
    }
}
