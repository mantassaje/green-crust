public class CommonFolkToRaidCityStateEvent : ICityStateEvent
{
    public string GetTitle(Biome cityBiome) => "Better then boredom";

    public string GetIntroudctionText(Biome cityBiome)
    {
        return "(Plan) People are bored. People pick sticks and compete. Few friends talk about great skill they showed. And about riches in one settlement.";
    }

    public string GetBlessedText(Biome cityBiome)
    {
        return "(Plan) Casual matches turn over the city popular tournaments. More great fighters from matches.";
    }

    public string GetCursedText(Biome cityBiome)
    {
        return $"(Plan) Elders son tries to fight in match. Gets beaten badly and barely survives. Elders ban matches.";
    }

    public string CurseButtonText(Biome cityBiome) => null;
    public string BlesseButtonText(Biome cityBiome) => null;

    public bool CanHappen(Biome cityBiome)
    {
        return cityBiome.CityState.Personality.CommonFolk.Rate > CityStateConstant.PersonalityAvarage
            && cityBiome.CityState.Personality.Rading.Rate > CityStateConstant.PersonalityFaded;
    }

    public void Bless(Biome cityBiome)
    {
        cityBiome.CityState.Personality.Rading.AddFromCommonFolk(CityStateConstant.PersonalityBigChange);
    }

    public void ApplyIgnored(Biome cityBiome)
    {
        cityBiome.CityState.Personality.Rading.AddFromCommonFolk(CityStateConstant.PersonalityLowChange);
    }

    public void Curse(Biome cityBiome)
    {
        cityBiome.CityState.Personality.Rading.AddFromCommonFolk(-CityStateConstant.PersonalityLowChange);
    }
}

public class CommonFolkToTradeCityStateEvent : ICityStateEvent
{
    public string GetTitle(Biome cityBiome) => "Elders trade";

    public string GetIntroudctionText(Biome cityBiome)
    {
        return "(Plan) Elder in tavern always tells stories when he was merchant traveling around the world. And verious interesting places and interesting stuff.";
    }

    public string GetBlessedText(Biome cityBiome)
    {
        return "(Plan) Stories stayed with people and they want to see those places. Some look what interesting they have. Pack the cart and travel away.";
    }

    public string GetCursedText(Biome cityBiome)
    {
        return $"(Plan) One day elder drank too much. Triped over and split his head. He stood up. Crawled to his house and layed on his bed. And never woke up.";
    }

    public string CurseButtonText(Biome cityBiome) => null;
    public string BlesseButtonText(Biome cityBiome) => null;

    public bool CanHappen(Biome cityBiome)
    {
        return cityBiome.CityState.Personality.CommonFolk.Rate > CityStateConstant.PersonalityAvarage
            && cityBiome.CityState.Personality.Trading.Rate > CityStateConstant.PersonalityFaded;
    }

    public void Bless(Biome cityBiome)
    {
        cityBiome.CityState.Personality.Trading.AddFromCommonFolk(CityStateConstant.PersonalityBigChange);
    }

    public void ApplyIgnored(Biome cityBiome)
    {
        cityBiome.CityState.Personality.Trading.AddFromCommonFolk(CityStateConstant.PersonalityLowChange);
    }

    public void Curse(Biome cityBiome)
    {
        cityBiome.CityState.Personality.Trading.AddFromCommonFolk(-CityStateConstant.PersonalityLowChange);
    }
}

public class CommonFolkToWorshipCityStateEvent : ICityStateEvent
{
    public string GetTitle(Biome cityBiome) => "Afterlife";

    public string GetIntroudctionText(Biome cityBiome)
    {
        return "(Plan) One day after hard work in fields people gather with ale and talked. They talked about work, about women and later started discusing what will happen to you when you die.";
    }

    public string GetBlessedText(Biome cityBiome)
    {
        return "(Plan) Discussion heated up and one started telling stories of gods and their constant fights between them. Stories lived on.";
    }

    public string GetCursedText(Biome cityBiome)
    {
        return $"(Plan) People drank more and started discusing even more about it. Everybody started to shout out each other. The evening became into a bunch of drunks shouting at each other. The next morning everybody woke up and didn't wanted to remember what happend yesterday.";
    }

    public string CurseButtonText(Biome cityBiome) => null;
    public string BlesseButtonText(Biome cityBiome) => null;

    public bool CanHappen(Biome cityBiome)
    {
        return cityBiome.CityState.Personality.CommonFolk.Rate > CityStateConstant.PersonalityAvarage
            && cityBiome.CityState.Personality.Worshiper.Rate > CityStateConstant.PersonalityFaded;
    }

    public void Bless(Biome cityBiome)
    {
        cityBiome.CityState.Personality.Worshiper.AddFromCommonFolk(CityStateConstant.PersonalityBigChange);
    }

    public void ApplyIgnored(Biome cityBiome)
    {
        cityBiome.CityState.Personality.Worshiper.AddFromCommonFolk(CityStateConstant.PersonalityLowChange);
    }

    public void Curse(Biome cityBiome)
    {
        cityBiome.CityState.Personality.Worshiper.AddFromCommonFolk(-CityStateConstant.PersonalityLowChange);
    }
}