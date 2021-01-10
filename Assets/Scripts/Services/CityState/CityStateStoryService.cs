using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public static class CityStateStoryService
{
    private static List<ICityStateSory> _stories = new List<ICityStateSory>
    {
        new IntroNameCityStateStory(),

        new CommonFolkCityStateStory(),
        new WarfareCityStateStory(),

        new TradeCityStateStory(),
        new GrowthStateStory(),

        new ReligionCityStateStory(),

        new ExplorationCityStateStory()
    };

    public static IReadOnlyList<ICityStateEvent> _events = new List<ICityStateEvent>
    {
        //All lack proper texts. For a first release there will be few events.
        /*new IntroduceRaidCityStateEvent(),
        new MoreRaidCityStateEvent(),
        new BlindAngerCityStateEvent(),
        new IntroduceTradeCityStateEvent(),
        new MoreTradeCityStateEvent(),
        new PyramidSchemeStateEvent(),
        new IntroduceWorshipCityStateEvent(),
        new WorshipMoreCityStateEvent(),
        new SacrificeCityStateEvent(),
        new CommonFolkToRaidCityStateEvent(),
        new CommonFolkToTradeCityStateEvent(),
        new CommonFolkToWorshipCityStateEvent(),
        new OnFireCityStateEvent()*/

        new MoreRaidCityStateEvent(),
        new MoreTradeCityStateEvent(),
        new WorshipMoreCityStateEvent(),
        new OnFireCityStateEvent()
    };

    public static string GetDescription(Biome cityBiome)
    {
        return string.Concat(_stories.Select(story => story.Get(cityBiome)));
    }

    public static ICityStateEvent GetRandomEvent(Biome cityBiome)
    { 
        return _events.Where(ev => ev.CanHappen(cityBiome))
            .ToList()
            .PickRandom(1)
            .FirstOrDefault();
    }

    public static void SetRandomEvent(Biome cityBiome)
    {
        var cityEvent = GetRandomEvent(cityBiome);
        cityBiome.CityState.Event = cityEvent;
    }
}

