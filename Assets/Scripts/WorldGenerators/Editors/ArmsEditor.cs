using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.WorldGenerator.Generators
{
    public class ArmsEditor: IEditor
    {
        private int _armCount;

        public ArmsEditor(int armCount)
        {
            _armCount = armCount;
        }

        public void Apply()
        {
            var startBiomes = new List<Biome>();
            for(int i = 0; i < _armCount; i++)
            {
                startBiomes.Add(GetSuitableStartBiome());
            }

            foreach(var startBiome in startBiomes)
            {
                CreateArm(startBiome);
            }
        }

        private void CreateArm(Biome biome)
        {
            var size = UnityEngine.Random.Range(5, 15);
            for (int i = 0; i < size; i++)
            {
                if (biome.IsNull()) return;
                biome.SetAbyss(false);
                biome = biome.GetNearbyBiomesCache()
                    .Where(near => near.IsAbyss)
                    .ToList()
                    .PickRandom(1)
                    .FirstOrDefault();
            }
        }

        private Biome GetSuitableStartBiome()
        {
            return Singles.Grid.GetAllBiomes()
            .Where(v => !v.IsAbyss
                && v.GetNearbyBiomesCache().Count(near => near.IsAbyss) > 2)
            .ToList()
            .PickRandom(1)
            .FirstOrDefault();
        }
    }
}
