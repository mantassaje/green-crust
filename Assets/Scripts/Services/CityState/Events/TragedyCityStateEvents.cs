public class OnFireCityStateEvent : ICityStateEvent
{
    public string GetTitle(Biome cityBiome) => "City razed";

    public string GetIntroudctionText(Biome cityBiome)
    {
        return "City attacked. The walls are breached and enemies run into the streets killing anybody in sight. For days various enemy parties are breaking into the houses, storages and halls to kill and pillage. Random people with any weapons they could find desperately try to block them out. Few bloody days have passed and attackers leave the city in smokes.";
    }

    public string GetBlessedText(Biome cityBiome)
    {
        return "Many hidden supplies are intact. People in farther settlements come to an aid. They find and help injured people in the debris. More survivors are found and treated. Next time people will be stronger.";
    }

    public string GetCursedText(Biome cityBiome)
    {
        return $"Those who can walk they pack what is left. They walk past the collapsed gateway and never return back.";
    }

    public bool CanHappen(Biome cityBiome)
    {
        return cityBiome.CityState.IsOnFire;
    }

    public void Bless(Biome cityBiome)
    {
        cityBiome.CityState.Population *= 1.1f;
        cityBiome.CityState.Personality.Rading.Add(CityStateConstant.PersonalityLowChange);
    }

    public void ApplyIgnored(Biome cityBiome)
    {
    }

    public void Curse(Biome cityBiome)
    {
        cityBiome.CityState.Population *= 0.6f;
    }

    public string BlesseButtonText(Biome cityBiome) => null;
    public string CurseButtonText(Biome cityBiome) => null;
}