using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.WorldGenerator.Generators
{
    public class MountainRangeEditor : IEditor
    {
        private float _streangth;

        public MountainRangeEditor(float streangth)
        {
            _streangth = streangth;
        }

        public void Apply()
        {
            int mountainCount = (int)(Math.Pow(Singles.Grid.Size, 2) / 1000f * _streangth);
            var dryBiomes = Singles.Grid.GetAllBiomes()
                   .Where(v => !v.IsWater && !v.IsAbyss)
                   .ToList()
                   .PickRandom(mountainCount);
            foreach (var biome in dryBiomes)
            {
                GenerateMountainRange(biome);
            }
        }

        private static void GenerateMountainRange(Biome center)
        {
            var biomes = Singles.Grid.GetAllNearby(center, 2)
                .ToList()
                .PickRandom(6);
            foreach (var biome in biomes)
            {
                biome.Crust.Height = UnityEngine.Random.Range(BiomeService.MinMountainHeight, BiomeService.MaxHeight);
            }
        }
    }
}
