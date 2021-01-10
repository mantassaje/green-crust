public class IntroduceTradeCityStateEvent : ICityStateEvent
{
    public string GetTitle(Biome cityBiome) => "Tempting wares";

    public string GetIntroudctionText(Biome cityBiome)
    {
        return "(Plan) Neighbouring tribe has intersting wares. Some people go there to trade it for something.";
    }

    public string GetBlessedText(Biome cityBiome)
    {
        return "(Plan) Some pack their cart and travel away and see what they can get for their stuff.";
    }

    public string GetCursedText(Biome cityBiome)
    {
        var fate = cityBiome.CityState.Personality.Rading.Rate >= CityStateConstant.PersonalityHigh 
            ? "Neighbouring tribe sacked."
            : "Wares burned.";
        return $"(Plan) Other are ofended by such acts. They think it is immoral. The life of sharing and spliting everything equaly should not be changed. {fate}";
    }

    public bool CanHappen(Biome cityBiome)
    {
        return cityBiome.CityState.Personality.Trading.Rate < CityStateConstant.PersonalityFaded
            && !cityBiome.CityState.IsOnFire;
    }

    public void Bless(Biome cityBiome)
    {
        cityBiome.CityState.Personality.Trading.AddHalfFromCommonFolk(CityStateConstant.PersonalityBigChange);
    }

    public void ApplyIgnored(Biome cityBiome)
    {
        cityBiome.CityState.Personality.Trading.AddFromCommonFolk(CityStateConstant.PersonalityLowChange);
    }

    public void Curse(Biome cityBiome)
    {
        cityBiome.CityState.Personality.Trading.AddFromCommonFolk(-CityStateConstant.PersonalityLowChange);
    }

    public string BlesseButtonText(Biome cityBiome) => null;
    public string CurseButtonText(Biome cityBiome) => null;
}

public class MoreTradeCityStateEvent : ICityStateEvent
{
    public string GetTitle(Biome cityBiome) => "Surplus to sell";

    public string GetIntroudctionText(Biome cityBiome)
    {
        return "There is surplus of some goods. Most of it lays in storage unused. Some talk about selling it somewhere in other cities. Others critic crafters of these goods as pointless people and maybe we have too many of them. The conflict of opinions is in stalemate and no elder is taking any action.";
    }

    public string GetBlessedText(Biome cityBiome)
    {
        return "Some noble managed to sell those goods far away and made a fortune. Others noticed it. A lot more people moved from their professions and started crafting those goods or hire others to craft those goods. Then they sold them in other lands and made a good living. Trading culture became stronger among people.";
    }

    public string GetCursedText(Biome cityBiome)
    {
        return $"Every inn is filled with shouts and mocks towards crafters of that crap. Those crafters live in dream land. Crafting what they like and they should find a real craftship. Any traders who used to sell those goods are also looked upon as lazy. Elders motivate people to be self-sufficient and craft everything they need them self. Trading culture vanished a bit.";
    }

    public bool CanHappen(Biome cityBiome)
    {
        return true; //Should be removed when all events will be included.

        return cityBiome.CityState.Personality.Trading.Rate > CityStateConstant.PersonalityFaded
            && !cityBiome.CityState.IsOnFire;
    }

    public void Bless(Biome cityBiome)
    {
        cityBiome.CityState.Personality.Trading.AddHalfFromCommonFolk(CityStateConstant.PersonalityBigChange);
    }

    public void ApplyIgnored(Biome cityBiome)
    {
    }

    public void Curse(Biome cityBiome)
    {
        cityBiome.CityState.Personality.Trading.AddFromCommonFolk(-CityStateConstant.PersonalityMidChange);
    }

    public string BlesseButtonText(Biome cityBiome) => "Bless goods";
    public string CurseButtonText(Biome cityBiome) => "Curse goods";
}

public class PyramidSchemeStateEvent : ICityStateEvent
{
    public string GetTitle(Biome cityBiome) => "Pyramid scheme";

    public string GetIntroudctionText(Biome cityBiome)
    {
        return "(Plan) Elite travels around neighbouring settlements. Asks for investments. Return will be massive. Find a friend that will also invest and returns will be bigger.";
    }

    public string GetBlessedText(Biome cityBiome)
    {
        return $"(Plan) Scheme grows massive and finaly bursts. The initiator dissapears with fortune. People dissapointed that there was no massive yeld, but maybe there will be more luck next time.";
    }

    public string GetCursedText(Biome cityBiome)
    {
        return $"(Plan) People started to invest massively into scheme. Scheme finaly bursts. Initiator tries to run with fortune, but is cought. He is killed. " +
            $"(Plan) People trust less those who seek investments. Merchendise are simplier";
    }

    public bool CanHappen(Biome cityBiome)
    {
        return cityBiome.CityState.Personality.Trading.Rate >= CityStateConstant.PersonalityHigh
            && !cityBiome.CityState.IsOnFire;
    }

    public void Bless(Biome cityBiome)
    {
        cityBiome.CityState.Personality.Trading.Add(CityStateConstant.PersonalityLowChange);
    }

    public void ApplyIgnored(Biome cityBiome)
    {
    }

    public void Curse(Biome cityBiome)
    {
        cityBiome.CityState.Personality.Trading.AddFromCommonFolk(-CityStateConstant.PersonalityMidChange);
    }

    public string BlesseButtonText(Biome cityBiome) => null;
    public string CurseButtonText(Biome cityBiome) => null;
}