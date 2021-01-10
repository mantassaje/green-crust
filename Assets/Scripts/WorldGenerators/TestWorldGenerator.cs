using Assets.Scripts.WorldGenerator.Generators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public static class TestWorldGenerator
{
    /// <summary>
    /// Players needs to be created. Will create world based on player count.
    /// </summary>
    public static void Generate()
    {
        new AllToAbyssEditor().Apply();
        new CircleForEachPlayerEditor(6).Apply();
        new RoughenAbyssEditor(2).Apply();
        new LowHillnesEditor().Apply();

        BiomeService.DefineAllBiomes();
    }
}

