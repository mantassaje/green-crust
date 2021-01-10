using Assets.Scripts.WorldGenerator.Generators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public static class BalancedWorldGenerator
{
    /// <summary>
    /// Players needs to be created. Will create world based on player count.
    /// </summary>
    public static void Generate(int size)
    {
        new AllToAbyssEditor().Apply();
        new CircleForEachPlayerEditor(size).Apply();
        new RoughenAbyssEditor(4).Apply();
        new ArmsEditor(Singles.World.Players.Count * size).Apply();
        new LowHillnesEditor().Apply();
        new MountainRangeEditor(size).Apply();

        BiomeService.DefineAllBiomes();
    }
}

