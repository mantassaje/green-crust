public class IntroduceRaidCityStateEvent : ICityStateEvent
{
    public string GetTitle(Biome cityBiome) => "Fear of neighbours";

    public string GetIntroudctionText(Biome cityBiome)
    {
        return "(Plan) Everybody feel unese. There are many aggresive tribes. We should prepare.";
    }

    public string GetBlessedText(Biome cityBiome)
    {
        return "(Plan) Tools transformed to weapons. More training. More aggresion.";
    }

    public string GetCursedText(Biome cityBiome)
    {
        return $"(Plan) Ruler calms everybody down with great speech.";
    }

    public string CurseButtonText(Biome cityBiome) => null;
    public string BlesseButtonText(Biome cityBiome) => null;

    public bool CanHappen(Biome cityBiome)
    {
        return cityBiome.CityState.Personality.Rading.Rate < CityStateConstant.PersonalityFaded;
    }

    public void Bless(Biome cityBiome)
    {
        cityBiome.CityState.Personality.Rading.AddHalfFromCommonFolk(CityStateConstant.PersonalityBigChange);
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

public class MoreRaidCityStateEvent : ICityStateEvent
{
    public string GetTitle(Biome cityBiome) => "Warlords wishes";

    public string GetIntroudctionText(Biome cityBiome)
    {
        return "Elders gather to another weekly meeting. Everybody discusses about various issues. The warlord is very active today. He is asking for a bigger army. He wants more gold and other support. He promises that it all will yield great riches. There are so many rich enemies. But other elders are not listening.";
    }

    public string GetBlessedText(Biome cityBiome)
    {
        return $"Finally the elders became interested. Influencers go to city center and give speeches about threats far away and about riches owned by those who do not deserve it. Those who own metal industries offer weapons. Others offer gold. More focus is placed to military. Those who used to do something else are now soldiers. People are stronger.";
    }

    public string GetCursedText(Biome cityBiome)
    {
        return $"Elders refuse warlord offers and on top of that started mocking him. He almost cut them with his sword, but guards stopped and forced him away from the hall. Warlord mobilizes his close fighters and storms the city hall. The hall was well guarded, but few very popular elders did die during an assault. This sparked an uprising in the city against other warlords and military institutions. People are skeptical against military and avoid military";
    }

    

    public bool CanHappen(Biome cityBiome)
    {
        return true; //Should be removed when all events will be included.

        return cityBiome.CityState.Personality.Rading.Rate > CityStateConstant.PersonalityFaded
            && cityBiome.CityState.Personality.Rading.Rate < CityStateConstant.PersonalityHigh;
    }

    public void Bless(Biome cityBiome)
    {
        cityBiome.CityState.Personality.Rading.AddHalfFromCommonFolk(CityStateConstant.PersonalityBigChange);
    }

    public void ApplyIgnored(Biome cityBiome)
    {
    }

    public void Curse(Biome cityBiome)
    {
        cityBiome.CityState.Personality.Rading.AddFromCommonFolk(-CityStateConstant.PersonalityMidChange);
    }

    public string CurseButtonText(Biome cityBiome) => "Curse warlord";
    public string BlesseButtonText(Biome cityBiome) => "Bless warlord";
}

public class BlindAngerCityStateEvent : ICityStateEvent
{
    public string GetTitle(Biome cityBiome) => "Blind anger";

    public string GetIntroudctionText(Biome cityBiome)
    {
        return "(Plan) One worrior was ofended by clansman. He kills clansman. Picks his sword and goes towords clansmans clans hall.";
    }

    public string GetBlessedText(Biome cityBiome)
    {
        return $"(Plan) Great fight. Becomes famous. Other imitate the role figure.";
    }

    public string GetCursedText(Biome cityBiome)
    {
        return $"(Plan) He is defeated and killed. Clan is popural. Random fight looks bad. Clan pushes more discipline. Less random violence.";
    }

    public string CurseButtonText(Biome cityBiome) => null;
    public string BlesseButtonText(Biome cityBiome) => null;

    public bool CanHappen(Biome cityBiome)
    {
        return cityBiome.CityState.Personality.Rading.Rate >= CityStateConstant.PersonalityHigh;
    }

    public void Bless(Biome cityBiome)
    {
        cityBiome.CityState.Personality.Rading.Add(CityStateConstant.PersonalityLowChange);
    }

    public void ApplyIgnored(Biome cityBiome)
    {
    }

    public void Curse(Biome cityBiome)
    {
        cityBiome.CityState.Personality.Rading.AddFromCommonFolk(-CityStateConstant.PersonalityMidChange);
    }
}