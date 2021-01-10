using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;



public class SeaManaBonus : IManaBonus
{
    public int GetManaBonus(Biome biome)
    {
        if (biome.Spec is ShallowSeaSpec
            || biome.Spec is IcaCapOceanBiomeSpec) return 1;
        return 0;
    }

    public string GetText(Biome biome)
    {
        return string.Format("+{0} from water", GetManaBonus(biome));
    }
}

public class DeepSeaManaBonus : IManaBonus
{
    public int GetManaBonus(Biome biome)
    {
        if (biome.Spec is DeepSeaSpec) return 2;
        return 0;
    }

    public string GetText(Biome biome)
    {
        return string.Format("+{0} from deep sea", GetManaBonus(biome));
    }
}

public class MountainManaBonus : IManaBonus
{
    public int GetManaBonus(Biome biome)
    {
        return biome.Crust.Height;
    }

    public string GetText(Biome biome)
    {
        return string.Format("+{0} from mountains", GetManaBonus(biome));
    }
}

public class NatureManaBonus : IManaBonus
{
    public int GetManaBonus(Biome biome)
    {
        var natureCount = biome.Nature.Population;
        if (natureCount < 1) return 0;
        var manaGainBase = natureCount;
        return (int)manaGainBase;
    }

    public string GetText(Biome biome)
    {
        return string.Format("+{0} from wildlife", GetManaBonus(biome));
    }
}

public class AncientsManaBonus : IManaBonus
{
    public int GetManaBonus(Biome biome)
    {
        if (biome.Nature.IsAncient)
        {
            return 1;
        }
        return 0;
    }

    public string GetText(Biome biome)
    {
        return string.Format("+{0} from ancients", GetManaBonus(biome));
    }
}

public class WorshiperManaBonus : IManaBonus
{
    public int GetManaBonus(Biome biome)
    {
        if (biome.CityState.IsNotable
            && biome.CityState.Personality.Worshiper.Rate > CityStateConstant.PersonalityFaded)
        {
            return (int)(biome.CityState.Population * biome.CityState.Personality.Worshiper.Rate * CityStateConstant.WorshiperManaMultiplyer).GetMin(1);
        }
        return 0;
    }

    public string GetText(Biome biome)
    {
        return string.Format("+{0} from worshipers", GetManaBonus(biome));
    }
}
