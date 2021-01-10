using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public interface IBiomeSpec
{
    SafeGetList<Sprite> FoliageSpriteTop { get; }
    SafeGetList<Sprite> FoliageSpriteBotttom { get; }
    SafeGetList<Sprite> MountainSprite { get; }
    Sprite Ground { get; }
    Sprite Ancient { get; }
    Color BaseColor { get; }
    AudioClip BackgroundAudioClip { get; }
    string Name { get; }
    string GetDescription(Biome biome);

    /// <summary>
    /// Nature can grow here but it does not.
    /// </summary>
    bool IsBarren { get; }

    /// <summary>
    /// Nothing can live here.
    /// </summary>
    bool IsDead { get; }
    bool IsWater { get; }
    bool CanApply(Biome biome);
}

