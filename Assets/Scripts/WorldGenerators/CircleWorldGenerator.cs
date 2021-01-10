using Assets.Scripts.WorldGenerator.Generators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public static class CircleWorldGenerator
{
    public static void Generate()
    {
        new CircleLandEditor().Apply();
        new RoughenAbyssEditor(4).Apply();
        new ArmsEditor(8).Apply();
        new AbyssHoleEditor(4).Apply();
        new LowHillnesEditor().Apply();

        Singles.World.Players.ForEach(v => PlayerService.SetRandomStartingPosition(v));

        BiomeService.DefineAllBiomes();
    }
}

