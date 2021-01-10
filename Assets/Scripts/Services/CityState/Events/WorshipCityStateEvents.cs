public class IntroduceWorshipCityStateEvent : ICityStateEvent
{
    public string GetTitle(Biome cityBiome) => "Bypassing priest";

    public string GetIntroudctionText(Biome cityBiome)
    {
        return "(Plan) Charismatic pries wonderind in. Tells stories.";
    }

    public string GetBlessedText(Biome cityBiome)
    {
        return "(Plan) People listen. People now think about posibility of gods. Stories stay.";
    }

    public string GetCursedText(Biome cityBiome)
    {
        var priestsFate = cityBiome.CityState.Personality.Rading.Rate >= CityStateConstant.PersonalityHigh 
            ? "Priest killed."
            : "Priest banished.";
        return $"People think he is insane. {priestsFate}";
    }

    public bool CanHappen(Biome cityBiome)
    {
        return cityBiome.CityState.Personality.Worshiper.Rate < CityStateConstant.PersonalityFaded
            && !cityBiome.CityState.IsOnFire;
    }

    public void Bless(Biome cityBiome)
    {
        cityBiome.CityState.Personality.Worshiper.Add(CityStateConstant.PersonalityBigChange);
    }

    public void ApplyIgnored(Biome cityBiome)
    {
        cityBiome.CityState.Personality.Worshiper.Add(CityStateConstant.PersonalityLowChange);
    }

    public void Curse(Biome cityBiome)
    {
        cityBiome.CityState.Personality.Worshiper.AddFromCommonFolk(-CityStateConstant.PersonalityLowChange);
    }

    public string CurseButtonText(Biome cityBiome) => null;
    public string BlesseButtonText(Biome cityBiome) => null;
}

public class WorshipMoreCityStateEvent : ICityStateEvent
{
    public string GetTitle(Biome cityBiome) => "Prophets visions";

    public string GetIntroudctionText(Biome cityBiome)
    {
        return "Every day prophet comes to the city center and talks about his visions. About how the city is going towards doom because of lack of believe. And how everybody should sacrifice their life towards the worship of the gods. But nobody is listening. Nobody is even giving him the attention of an insane person.";
    }

    public string GetBlessedText(Biome cityBiome)
    {
        return "One vision and prophet words begin to resonate with peoples feelings. Every day more and more people come to the city center to hear the prophet. People drop what they used to do before and lend their life to the worship.";
    }

    public string GetCursedText(Biome cityBiome)
    {
        return $"The prophet does start to become famous. But with fame comes and infamy. With a great followship many people started to blame famine and diseases on the prophet and his followers. Was it elders who did not want a new competition for power? It does not matter. Followers are mysteriously disappearing and prophet vanishes as if he never existed. The religion is weakened in the city.";
    }

    public bool CanHappen(Biome cityBiome)
    {
        return true; //Should be removed when all events will be included.

        return cityBiome.CityState.Personality.Worshiper.Rate > CityStateConstant.PersonalityFaded;
    }

    public void Bless(Biome cityBiome)
    {
        cityBiome.CityState.Personality.Worshiper.Add(CityStateConstant.PersonalityBigChange);
    }

    public void ApplyIgnored(Biome cityBiome)
    {
    }

    public void Curse(Biome cityBiome)
    {
        cityBiome.CityState.Personality.Worshiper.AddFromCommonFolk(-CityStateConstant.PersonalityMidChange);
    }

    public string BlesseButtonText(Biome cityBiome) => "Bless prophet";
    public string CurseButtonText(Biome cityBiome) => "Curse prophet";
}

public class SacrificeCityStateEvent : ICityStateEvent
{
    public string GetTitle(Biome cityBiome) => "Human sacrifice";

    public string GetIntroudctionText(Biome cityBiome)
    {
        return "(Plan) People are sacrificed. Top priests talk about mass sacrifice.";
    }

    public string GetBlessedText(Biome cityBiome)
    {
        return $"(Plan) People lined up. Their throats are cut one after another. You gain {cityBiome.Owner.GetManaCost() * 3} mana.";
    }

    public string GetCursedText(Biome cityBiome)
    {
        return $"(Plan) People are angered by idea of so many deaths. Top priests sacrificed.";
    }

    public bool CanHappen(Biome cityBiome)
    {
        return cityBiome.CityState.Personality.Worshiper.Rate >= CityStateConstant.PersonalityHigh;
    }

    public void Bless(Biome cityBiome)
    {
        cityBiome.Owner.AddMana(cityBiome.Owner.GetManaCost() * 3);
    }

    public void ApplyIgnored(Biome cityBiome)
    {
    }

    public void Curse(Biome cityBiome)
    {
        cityBiome.CityState.Personality.Worshiper.AddFromCommonFolk(-CityStateConstant.PersonalityMidChange);
    }

    public string BlesseButtonText(Biome cityBiome) => null;
    public string CurseButtonText(Biome cityBiome) => null;
}