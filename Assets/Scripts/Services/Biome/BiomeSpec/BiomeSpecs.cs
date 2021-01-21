using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class AbyssBiomeSpec : IBiomeSpec
{
    public SafeGetList<Sprite> FoliageSpriteTop { get { return Singles.Sprites.NoSprite; } }
    public SafeGetList<Sprite> FoliageSpriteBotttom { get { return Singles.Sprites.NoSprite; } }
    public SafeGetList<Sprite> MountainSprite { get { return Singles.Sprites.NoSprite; } }
    public Sprite Ground { get { return null; } }
    public Sprite Ancient { get { return null; } }
    public Color BaseColor { get { return Singles.Colors.Uninhabited; } }
    public AudioClip BackgroundAudioClip { get { return null; } }
    public string Name { get { return "Abyss"; } }
    public bool IsBarren { get { return true; } }
    public bool IsDead { get { return true; } }
    public bool IsWater { get { return false; } }

    public bool CanApply(Biome biome)
    {
        return biome.IsAbyss;
    }

    public string GetDescription(Biome biome)
    {
        return "Cold comes from here.";
    }
}

public class ColdMudDesertBiomeSpec : IBiomeSpec
{
    public SafeGetList<Sprite> FoliageSpriteTop { get { return Singles.Sprites.NoSprite; } }
    public SafeGetList<Sprite> FoliageSpriteBotttom { get { return Singles.Sprites.NoSprite; } }
    public SafeGetList<Sprite> MountainSprite { get { return Singles.Sprites.MountainSprites; } }
    public Sprite Ground {  get { return Singles.Sprites.ColdMud; } }
    public Sprite Ancient { get { return null; } }
    public Color BaseColor { get { return Singles.Colors.Uninhabited; } }
    public AudioClip BackgroundAudioClip { get { return Singles.Audio.DesertWind; } }
    public string Name { get { return "Snow desert"; } }
    public bool IsBarren { get { return true; } }
    public bool IsDead { get { return false; } }
    public bool IsWater { get { return false; } }

    public bool CanApply(Biome biome)
    {
        return !biome.IsWater
            && biome.Nature.Population < 1f
            && biome.Weather.HeatType == HeatTypes.Cold
            && biome.Weather.RainfallType >= RainfallTypes.Dry;
    }

    public string GetDescription(Biome biome)
    {
        return "Life can emerge here.";
    }
}

public class WarmMudDesertBiomeSpec : IBiomeSpec
{
    public SafeGetList<Sprite> FoliageSpriteTop { get { return Singles.Sprites.NoSprite; } }
    public SafeGetList<Sprite> FoliageSpriteBotttom { get { return Singles.Sprites.NoSprite; } }
    public SafeGetList<Sprite> MountainSprite { get { return Singles.Sprites.MountainSprites; } }
    public Sprite Ground { get { return Singles.Sprites.WarmMud; } }
    public Sprite Ancient { get { return null; } }
    public Color BaseColor { get { return Singles.Colors.Uninhabited; } }
    public AudioClip BackgroundAudioClip { get { return Singles.Audio.DesertWind; } }
    public string Name { get { return "Warm mud desert"; } }
    public bool IsBarren { get { return true; } }
    public bool IsDead { get { return false; } }
    public bool IsWater { get { return false; } }

    public bool CanApply(Biome biome)
    {
        return !biome.IsWater
            && biome.Nature.Population < 1f
            && biome.Weather.HeatType == HeatTypes.Warm
            && biome.Weather.RainfallType >= RainfallTypes.Dry;
    }

    public string GetDescription(Biome biome)
    {
        return "Life can emerge here.";
    }
}

public class HotMudDesertBiomeSpec : IBiomeSpec
{
    public SafeGetList<Sprite> FoliageSpriteTop { get { return Singles.Sprites.NoSprite; } }
    public SafeGetList<Sprite> FoliageSpriteBotttom { get { return Singles.Sprites.NoSprite; } }
    public SafeGetList<Sprite> MountainSprite { get { return Singles.Sprites.MountainSprites; } }
    public Sprite Ground { get { return Singles.Sprites.HotMud; } }
    public Sprite Ancient { get { return null; } }
    public Color BaseColor { get { return Singles.Colors.Uninhabited; } }
    public AudioClip BackgroundAudioClip { get { return Singles.Audio.DesertWind; } }
    public string Name { get { return "Hot mud desert"; } }
    public bool IsBarren { get { return true; } }
    public bool IsDead { get { return false; } }
    public bool IsWater { get { return false; } }

    public bool CanApply(Biome biome)
    {
        return !biome.IsWater
            && biome.Nature.Population < 1f
            && biome.Weather.HeatType == HeatTypes.Hot
            && biome.Weather.RainfallType >= RainfallTypes.Dry;
    }

    public string GetDescription(Biome biome)
    {
        return "Life can emerge here.";
    }
}

public class HotDesertBiomeSpec : IBiomeSpec
{
    public SafeGetList<Sprite> FoliageSpriteTop { get { return Singles.Sprites.NoSprite; } }
    public SafeGetList<Sprite> FoliageSpriteBotttom { get { return Singles.Sprites.NoSprite; } }
    public SafeGetList<Sprite> MountainSprite { get { return Singles.Sprites.MountainSprites; } }
    public Sprite Ground { get { return Singles.Sprites.HotDesert; } }
    public Sprite Ancient { get { return null; } }
    public Color BaseColor { get { return Singles.Colors.Desert; } }
    public AudioClip BackgroundAudioClip { get { return Singles.Audio.DesertWind; } }
    public string Name { get { return "Hot desert"; } }
    public bool IsBarren { get { return true; } }
    public bool IsDead { get { return true; } }
    public bool IsWater { get { return false; } }

    public bool CanApply(Biome biome)
    {
        return !biome.IsWater
            && biome.Weather.HeatType == HeatTypes.Hot
            && biome.Weather.RainfallType == RainfallTypes.Arid;
    }

    public string GetDescription(Biome biome)
    {
        if (biome.Crust.Height >= 1)
        {
            return "The bigger the mountains the more rainfall and cold they will block.";
        }
        else
        {
            return "Life needs rainfall here. And more rainfall because of heat.";
        }
    }
}

public class WarmDesertBiomeSpec : IBiomeSpec
{
    public SafeGetList<Sprite> FoliageSpriteTop { get { return Singles.Sprites.NoSprite; } }
    public SafeGetList<Sprite> FoliageSpriteBotttom { get { return Singles.Sprites.NoSprite; } }
    public SafeGetList<Sprite> MountainSprite { get { return Singles.Sprites.MountainSprites; } }
    public Sprite Ground { get { return Singles.Sprites.WarmDesert; } }
    public Sprite Ancient { get { return null; } }
    public Color BaseColor { get { return Singles.Colors.Desert; } }
    public AudioClip BackgroundAudioClip { get { return Singles.Audio.DesertWind; } }
    public string Name { get { return "Warm desert"; } }
    public bool IsBarren { get { return true; } }
    public bool IsDead { get { return true; } }
    public bool IsWater { get { return false; } }

    public bool CanApply(Biome biome)
    {
        return !biome.IsWater
            && biome.Weather.HeatType == HeatTypes.Warm
            && biome.Weather.RainfallType == RainfallTypes.Arid;
    }

    public string GetDescription(Biome biome)
    {
        if (biome.Crust.Height >= 1)
        {
            return "The bigger the mountains the more rainfall and cold they will block.";
        }
        else
        {
            return "Life needs rainfall here.";
        }
    }
}

public class ColdDesertBiomeSpec : IBiomeSpec
{
    public SafeGetList<Sprite> FoliageSpriteTop { get { return Singles.Sprites.NoSprite; } }
    public SafeGetList<Sprite> FoliageSpriteBotttom { get { return Singles.Sprites.NoSprite; } }
    public SafeGetList<Sprite> MountainSprite { get { return Singles.Sprites.MountainSprites; } }
    public Sprite Ground { get { return Singles.Sprites.ColdDesert; } }
    public Sprite Ancient { get { return null; } }
    public Color BaseColor { get { return Singles.Colors.Desert; } }
    public AudioClip BackgroundAudioClip { get { return Singles.Audio.DesertWind; } }
    public string Name { get { return "Cold desert"; } }
    public bool IsBarren { get { return true; } }
    public bool IsDead { get { return true; } }
    public bool IsWater { get { return false; } }

    public bool CanApply(Biome biome)
    {
        return !biome.IsWater
            && biome.Weather.HeatType == HeatTypes.Cold
            && biome.Weather.RainfallType == RainfallTypes.Arid;
    }

    public string GetDescription(Biome biome)
    {
        if (biome.Crust.Height >= 1)
        {
            return "The bigger the mountains the more rainfall and cold they will block.";
        }
        else
        {
            return "Life needs rainfall here.";
        }
    }
}

public class FrozenDesertBiomeSpec : IBiomeSpec
{
    public SafeGetList<Sprite> FoliageSpriteTop { get { return Singles.Sprites.NoSprite; } }
    public SafeGetList<Sprite> FoliageSpriteBotttom { get { return Singles.Sprites.NoSprite; } }
    public SafeGetList<Sprite> MountainSprite { get { return Singles.Sprites.MountainSprites; } }
    public Sprite Ground { get { return Singles.Sprites.DeadCold; } }
    public Sprite Ancient { get { return null; } }
    public Color BaseColor { get { return Singles.Colors.DeadCold; } }
    public AudioClip BackgroundAudioClip { get { return Singles.Audio.DesertWind; } }
    public string Name { get { return "Frozen desert"; } }
    public bool IsBarren { get { return true; } }
    public bool IsDead { get { return true; } }
    public bool IsWater { get { return false; } }

    public bool CanApply(Biome biome)
    {
        return !biome.IsWater
            && biome.Weather.HeatType == HeatTypes.Frozen;
    }

    public string GetDescription(Biome biome)
    {
        if (biome.Crust.Height >= 1)
        {
            return "The bigger the mountains the more rainfall and cold they will block.";
        }
        else
        {
            return "Life needs more heat.";
        }
    }
}

public class DeepSeaSpec : IBiomeSpec
{
    public SafeGetList<Sprite> FoliageSpriteTop { get { return Singles.Sprites.NoSprite; } }
    public SafeGetList<Sprite> FoliageSpriteBotttom { get { return Singles.Sprites.NoSprite; } }
    public SafeGetList<Sprite> MountainSprite { get { return Singles.Sprites.MountainSprites; } }
    public Sprite Ground { get { return Singles.Sprites.DeepOcean; } }
    public Sprite Ancient { get { return Singles.Sprites.Ancient_Whale; } }
    public Color BaseColor { get { return new Color(0.027f, 0.18f, 0.455f); } }
    public AudioClip BackgroundAudioClip { get { return Singles.Audio.SeaWaves; } }
    public string Name { get { return "Deep sea"; } }
    public bool IsBarren { get { return false; } }
    public bool IsDead { get { return false; } }
    public bool IsWater { get { return true; } }

    public bool CanApply(Biome biome)
    {
        return biome.IsWater
            && biome.Weather.HeatType != HeatTypes.Frozen
            && biome.GetNearbyBiomesCache().All(near => near.IsWater);
    }

    public string GetDescription(Biome biome)
    {
        if (biome.Nature.IsAncient)
        {
            return "Strong ancient ecosystem is here. Changed waters will destroy it.";
        }
        else if (biome.Weather.HeatType <= HeatTypes.Cold)
        {
            return "Sea is cold and therefore a small source of rainfall.";
        }
        else
        {
            return "The hotter the sea the bigger source of rainfall it will be.";
        }
    }
}

public class ShallowSeaSpec : IBiomeSpec
{
    public SafeGetList<Sprite> FoliageSpriteTop { get { return Singles.Sprites.NoSprite; } }
    public SafeGetList<Sprite> FoliageSpriteBotttom { get { return Singles.Sprites.NoSprite; } }
    public SafeGetList<Sprite> MountainSprite { get { return Singles.Sprites.MountainSprites; } }
    public Sprite Ground { get { return Singles.Sprites.ShallowSea; } }
    public Sprite Ancient { get { return Singles.Sprites.Ancient_UnderwaterTurtle; } }
    public Color BaseColor { get { return new Color(0.027f, 0.18f, 0.455f); } }
    public AudioClip BackgroundAudioClip { get { return Singles.Audio.SeaWaves; } }
    public string Name { get { return "Sea"; } }
    public bool IsBarren { get { return false; } }
    public bool IsDead { get { return false; } }
    public bool IsWater { get { return true; } }

    public bool CanApply(Biome biome)
    {
        return biome.IsWater
            && biome.Weather.HeatType != HeatTypes.Frozen
            && biome.GetNearbyBiomesCache().Any(near => !near.IsWater);
    }

    public string GetDescription(Biome biome)
    {
        if (biome.Nature.IsAncient)
        {
            return "Strong ancient ecosystem is here. Changed waters will destroy it.";
        }
        else if (biome.Weather.HeatType <= HeatTypes.Cold)
        {
            return "Sea is cold and therefore a small source of rainfall.";
        }
        else
        {
            return "The hotter the sea the bigger source of rainfall it will be.";
        }
    }
}

public class IcaCapOceanBiomeSpec : IBiomeSpec
{
    public SafeGetList<Sprite> FoliageSpriteTop { get { return Singles.Sprites.NoSprite; } }
    public SafeGetList<Sprite> FoliageSpriteBotttom { get { return Singles.Sprites.NoSprite; } }
    public SafeGetList<Sprite> MountainSprite { get { return Singles.Sprites.MountainSprites; } }
    public Sprite Ground { get { return Singles.Sprites.IceCap; } }
    public Sprite Ancient { get { return Singles.Sprites.Ancient_PolarBear; } }
    public Color BaseColor { get { return Singles.Colors.IceCap; } }
    public AudioClip BackgroundAudioClip { get { return Singles.Audio.DesertWind; } }
    public string Name { get { return "Ice caps"; } }
    public bool IsBarren { get { return false; } }
    public bool IsDead { get { return false; } }
    public bool IsWater { get { return true; } }

    public bool CanApply(Biome biome)
    {
        return biome.IsWater
            && biome.Weather.HeatType == HeatTypes.Frozen;
    }

    public string GetDescription(Biome biome)
    {
        if (biome.Nature.IsAncient)
        {
            return "Strong ancient ecosystem is here. Changed climate will destroy it.";
        }
        else
        {
            return "Is frozen and does not increases rainfall.";
        }
    }
}

public class TundraBiomeSpec : IBiomeSpec
{
    public SafeGetList<Sprite> FoliageSpriteTop { get { return Singles.Sprites.TundraGrassTopSprites; } }
    public SafeGetList<Sprite> FoliageSpriteBotttom { get { return Singles.Sprites.TundraGrassBotSprites; } }
    public SafeGetList<Sprite> MountainSprite { get { return Singles.Sprites.MountainSprites; } }
    public Sprite Ground { get { return Singles.Sprites.TundraGround; } }
    public Sprite Ancient { get { return Singles.Sprites.Ancient_PolarFox; } }
    public Color BaseColor { get { return Singles.Colors.Tundra; } }
    public AudioClip BackgroundAudioClip { get { return Singles.Audio.DesertWind; } }
    public string Name { get { return "Tundra"; } }
    public bool IsBarren { get { return false; } }
    public bool IsDead { get { return false; } }
    public bool IsWater { get { return false; } }

    public bool CanApply(Biome biome)
    {
        return !biome.IsWater
            && biome.Nature.Population >= 1f
            && biome.Weather.HeatType == HeatTypes.Cold
            && biome.Weather.RainfallType == RainfallTypes.Dry;
    }

    public string GetDescription(Biome biome)
    {
        if (biome.Nature.IsAncient)
        {
            return "Strong ancient ecosystem is here. Changed climate will destroy it.";
        }
        else if (biome.Crust.Height >= CityStateConstant.InpassableHeight)
        {
            return "Alpine tundra. These mountains are too big for humans to pass.";
        }
        else if (biome.Crust.Height >= 1)
        {
            return "Alpine tundra. Less wildlife because of mountains.";
        }
        else
        {
            return "Dry and cold biome. Not a lot can live here and everything grows slowly.";
        }
    }
}

public class TaigaBiomeSpec : IBiomeSpec
{
    public SafeGetList<Sprite> FoliageSpriteTop { get { return Singles.Sprites.TaigaForestTopSprites; } }
    public SafeGetList<Sprite> FoliageSpriteBotttom { get { return Singles.Sprites.TaigaForestBotSprites; } }
    public SafeGetList<Sprite> MountainSprite { get { return Singles.Sprites.MountainSprites; } }
    public Sprite Ground { get { return Singles.Sprites.TaigaGround; } }
    public Sprite Ancient { get { return Singles.Sprites.Ancient_WinterWolf; } }
    public Color BaseColor { get { return Singles.Colors.Taiga; } }
    public AudioClip BackgroundAudioClip { get { return Singles.Audio.DesertWind; } }
    public string Name { get { return "Boreal"; } }
    public bool IsBarren { get { return false; } }
    public bool IsDead { get { return false; } }
    public bool IsWater { get { return false; } }

    public bool CanApply(Biome biome)
    {
        var requiredConditions = !biome.IsWater
            && biome.Nature.Population >= 1f
            && biome.Weather.HeatType == HeatTypes.Cold;

        return requiredConditions
            && (biome.Weather.RainfallType == RainfallTypes.Humid
                || biome.Weather.RainfallType == RainfallTypes.Wet);
    }

    public string GetDescription(Biome biome)
    {
        if (biome.Nature.IsAncient)
        {
            return "Strong ancient ecosystem is here. Changed climate will destroy it.";
        }
        else if (biome.Crust.Height >= CityStateConstant.InpassableHeight)
        {
            return "Alpine forest. These mountains are too big for humans to pass.";
        }
        else if (biome.Crust.Height >= 1)
        {
            return "Alpine forest. Less wildlife because of mountains.";
        }
        else
        {
            return "Dry and cold forest. Wildlife grows slowly here.";
        }
    }
}

public class GrasslandBiomeSpec : IBiomeSpec
{
    public SafeGetList<Sprite> FoliageSpriteTop { get { return Singles.Sprites.GrasslandSpriteTop; } }
    public SafeGetList<Sprite> FoliageSpriteBotttom { get { return Singles.Sprites.GrasslandSpriteBotttom; } }
    public SafeGetList<Sprite> MountainSprite { get { return Singles.Sprites.MountainSprites; } }
    public Sprite Ground { get { return Singles.Sprites.GrasslandGround; } }
    public Sprite Ancient { get { return Singles.Sprites.Ancient_Bison; } }
    public Color BaseColor { get { return Singles.Colors.Grassland; } }
    public AudioClip BackgroundAudioClip { get { return Singles.Audio.GrasslandsGrasshopers; } }
    public string Name { get { return "Grasslands"; } }
    public bool IsBarren { get { return false; } }
    public bool IsDead { get { return false; } }
    public bool IsWater { get { return false; } }

    public bool CanApply(Biome biome)
    {
        return !biome.IsWater
            && biome.Nature.Population >= 1f
            && biome.Weather.HeatType == HeatTypes.Warm
            && biome.Weather.RainfallType == RainfallTypes.Dry;
    }

    public string GetDescription(Biome biome)
    {
        if (biome.Nature.IsAncient)
        {
            return "Strong ancient ecosystem is here. Changed climate will destroy it.";
        }
        else if (biome.Crust.Height >= CityStateConstant.InpassableHeight)
        {
            return "Alpine grasslands. These mountains are too big for humans to pass.";
        }
        else if (biome.Crust.Height >= 1)
        {
            return "Alpine grasslands. Less wildlife because of mountains.";
        }
        else
        {
            return "Warm but dry grasslands. Other biomes can have a lot more wildlife.";
        }
    }
}

public class LeafForestBiomeSpec : IBiomeSpec
{
    public SafeGetList<Sprite> FoliageSpriteTop { get { return Singles.Sprites.LeafForestSpriteTop; } }
    public SafeGetList<Sprite> FoliageSpriteBotttom { get { return Singles.Sprites.LeafForestSpriteBotttom; } }
    public SafeGetList<Sprite> MountainSprite { get { return Singles.Sprites.MountainSprites; } }
    public Sprite Ground { get { return Singles.Sprites.ForestGround; } }
    public Sprite Ancient { get { return Singles.Sprites.Ancient_Deer; } }
    public Color BaseColor { get { return Singles.Colors.TemperateForest; } }
    public AudioClip BackgroundAudioClip { get { return Singles.Audio.ForestBirds; } }
    public string Name { get { return "Temperate forest"; } }
    public bool IsBarren { get { return false; } }
    public bool IsDead { get { return false; } }
    public bool IsWater { get { return false; } }

    public bool CanApply(Biome biome)
    {
        return !biome.IsWater
            && biome.Nature.Population >= 1f
            && biome.Weather.HeatType == HeatTypes.Warm
            && biome.Weather.RainfallType == RainfallTypes.Humid;
    }

    public string GetDescription(Biome biome)
    {
        if (biome.Nature.IsAncient)
        {
            return "Strong ancient ecosystem is here. Changed climate will destroy it.";
        }
        else if (biome.Crust.Height >= CityStateConstant.InpassableHeight)
        {
            return "Alpine forest. These mountains are too big for humans to pass.";
        }
        else if (biome.Crust.Height >= 1)
        {
            return "Alpine forest. Less wildlife because of mountains.";
        }
        else
        {
            return "Forest in the middle by its climate.";
        }
    }
}

public class WetlandsBiomeSpec : IBiomeSpec
{
    public SafeGetList<Sprite> FoliageSpriteTop { get { return Singles.Sprites.LeafForestSpriteTop; } }
    public SafeGetList<Sprite> FoliageSpriteBotttom { get { return Singles.Sprites.LeafForestSpriteBotttom; } }
    public SafeGetList<Sprite> MountainSprite { get { return Singles.Sprites.MountainSprites; } }
    public Sprite Ground { get { return Singles.Sprites.WetlandGround; } }
    public Sprite Ancient { get { return Singles.Sprites.Ancient_Beaber; } }
    public Color BaseColor { get { return Singles.Colors.TemperateRainforest; } }
    public AudioClip BackgroundAudioClip { get { return Singles.Audio.ForestBirds; } }
    public string Name { get { return "Wetlands"; } }
    public bool IsBarren { get { return false; } }
    public bool IsDead { get { return false; } }
    public bool IsWater { get { return false; } }

    public bool CanApply(Biome biome)
    {
        return !biome.IsWater
            && biome.Nature.Population >= 1f
            && biome.Weather.HeatType == HeatTypes.Warm
            && biome.Weather.RainfallType == RainfallTypes.Wet;
    }

    public string GetDescription(Biome biome)
    {
        if(biome.Nature.IsAncient)
        {
            return "Strong ancient ecosystem is here. Changed climate will destroy it.";
        }
        else if (biome.Crust.Height >= CityStateConstant.InpassableHeight)
        {
            return "Alpine wetlands. These mountains are too big for humans to pass.";
        }
        else if (biome.Crust.Height >= 1)
        {
            return "Alpine wetlands. Less wildlife because of mountains.";
        }
        else
        {
            return "Always rainy and warm swamps. Full of wildlife.";
        }
    }
}

public class SavanaBiomeSpec : IBiomeSpec
{
    public SafeGetList<Sprite> FoliageSpriteTop { get { return Singles.Sprites.SavanaSpriteTop; } }
    public SafeGetList<Sprite> FoliageSpriteBotttom { get { return Singles.Sprites.SavanaSpriteBotttom; } }
    public SafeGetList<Sprite> MountainSprite { get { return Singles.Sprites.MountainSprites; } }
    public Sprite Ground { get { return Singles.Sprites.SavanaGround; } }
    public Sprite Ancient { get { return Singles.Sprites.Ancient_Lion; } }
    public Color BaseColor { get { return Singles.Colors.Savana; } }
    public AudioClip BackgroundAudioClip { get { return Singles.Audio.GrasslandsGrasshopers; } }
    public string Name { get { return "Savanna"; } }
    public bool IsBarren { get { return false; } }
    public bool IsDead { get { return false; } }
    public bool IsWater { get { return false; } }

    public bool CanApply(Biome biome)
    {
        return !biome.IsWater
            && biome.Nature.Population >= 1f
            && biome.Weather.HeatType == HeatTypes.Hot
            && biome.Weather.RainfallType == RainfallTypes.Dry;
    }

    public string GetDescription(Biome biome)
    {
        if (biome.Nature.IsAncient)
        {
            return "Strong ancient ecosystem is here. Changed climate will destroy it.";
        }
        else if (biome.Crust.Height >= CityStateConstant.InpassableHeight)
        {
            return "Alpine savanna. These mountains are too big for humans to pass.";
        }
        else if (biome.Crust.Height >= 1)
        {
            return "Alpine savanna. Less wildlife because of mountains.";
        }
        else
        {
            return "Hot and dry savanna. Other biomes can have a lot more wildlife.";
        }
    }
}

public class TropicsBiomeSpec : IBiomeSpec
{
    public SafeGetList<Sprite> FoliageSpriteTop { get { return Singles.Sprites.TropicSpriteTop; } }
    public SafeGetList<Sprite> FoliageSpriteBotttom { get { return Singles.Sprites.TropicSpriteBotttom; } }
    public SafeGetList<Sprite> MountainSprite { get { return Singles.Sprites.MountainSprites; } }
    public Sprite Ground { get { return Singles.Sprites.TropicGround; } }
    public Sprite Ancient { get { return Singles.Sprites.Ancient_Elephant; } }
    public Color BaseColor { get { return Singles.Colors.TropicalForest; } }
    public AudioClip BackgroundAudioClip { get { return Singles.Audio.JungleForest; } }
    public string Name { get { return "Tropics"; } }
    public bool IsBarren { get { return false; } }
    public bool IsDead { get { return false; } }
    public bool IsWater { get { return false; } }

    public bool CanApply(Biome biome)
    {
        return !biome.IsWater
            && biome.Nature.Population >= 1f
            && biome.Weather.HeatType == HeatTypes.Hot
            && biome.Weather.RainfallType == RainfallTypes.Humid;
    }

    public string GetDescription(Biome biome)
    {
        if (biome.Nature.IsAncient)
        {
            return "Strong ancient ecosystem is here. Changed climate will destroy it.";
        }
        else if (biome.Crust.Height >= CityStateConstant.InpassableHeight)
        {
            return "Alpine tropics. These mountains are too big for humans to pass.";
        }
        else if (biome.Crust.Height >= 1)
        {
            return "Alpine tropics. Less wildlife because of mountains.";
        }
        else
        {
            return "Hot, but has enough of rainfall to have forests.";
        }
    }
}

public class RainforestBiomeSpec : IBiomeSpec
{
    public SafeGetList<Sprite> FoliageSpriteTop { get { return Singles.Sprites.TropicSpriteTop; } }
    public SafeGetList<Sprite> FoliageSpriteBotttom { get { return Singles.Sprites.TropicSpriteBotttom; } }
    public SafeGetList<Sprite> MountainSprite { get { return Singles.Sprites.MountainSprites; } }
    public Sprite Ground { get { return Singles.Sprites.RainforestGround; } }
    public Sprite Ancient { get { return Singles.Sprites.Ancient_TreePython; } }
    public Color BaseColor { get { return Singles.Colors.Rainforest; } }
    public AudioClip BackgroundAudioClip { get { return Singles.Audio.JungleForest; } }
    public string Name { get { return "Rainforest"; } }
    public bool IsBarren { get { return false; } }
    public bool IsDead { get { return false; } }
    public bool IsWater { get { return false; } }

    public bool CanApply(Biome biome)
    {
        return !biome.IsWater
            && biome.Nature.Population >= 1f
            && biome.Weather.HeatType == HeatTypes.Hot
            && biome.Weather.RainfallType == RainfallTypes.Wet;
    }

    public string GetDescription(Biome biome)
    {
        if (biome.Nature.IsAncient)
        {
            return "Strong ancient ecosystem is here. Changed climate will destroy it.";
        }
        else if (biome.Crust.Height >= CityStateConstant.InpassableHeight)
        {
            return "Alpine rainforest. These mountains are too big for humans to pass.";
        }
        else if (biome.Crust.Height >= 1)
        {
            return "Alpine rainforest. Less wildlife because of mountains.";
        }
        else
        {
            return "Always rainy and hot rainforest. Full of wildlife.";
        }
    }
}

public class BuglandBiomeSpec : IBiomeSpec
{
    public SafeGetList<Sprite> FoliageSpriteTop { get { return Singles.Sprites.NoSprite; } }
    public SafeGetList<Sprite> FoliageSpriteBotttom { get { return Singles.Sprites.NoSprite; } }
    public SafeGetList<Sprite> MountainSprite { get { return Singles.Sprites.MountainSprites; } }
    public Sprite Ground { get { return Singles.Sprites.RainforestGround; } }
    public Sprite Ancient { get { return null; } }
    public Color BaseColor { get { return Singles.Colors.Rainforest; } }
    public AudioClip BackgroundAudioClip { get { return Singles.Audio.DesertWind; } }
    public string Name { get { return "Bugland"; } }
    public bool IsBarren { get { return true; } }
    public bool IsDead { get { return true; } }
    public bool IsWater { get { return false; } }

    public bool CanApply(Biome biome)
    {
        return true;
    }

    public string GetDescription(Biome biome)
    {
        return "This should not happen. Take a screenshot and report me to game dev.";
    }
}
