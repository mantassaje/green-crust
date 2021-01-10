using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.WorldGenerator.Generators
{
    public class AbyssHoleEditor : IEditor
    {
        private int _holes;

        public AbyssHoleEditor(int holes)
        {
            _holes = holes;
        }

        public void Apply()
        {
            for(int i = 0; i < _holes; i++)
            {
                CreateAbyssHole();
            }
        }

        private void CreateAbyssHole()
        {
            var biome = GetRandomNonAbyss(Singles.Grid.GetAllBiomes());
            biome.SetAbyss(true);
            for (int i = 0; i < 3; i++)
            {
                biome = GetRandomNonAbyss(biome.GetNearbyBiomesCache());
                if (biome.IsNull()) return;
                biome.SetAbyss(true);
            }
        }

        private static Biome GetRandomNonAbyss(IEnumerable<Biome> biomes)
        {
            return biomes.Where(v => !v.IsAbyss)
                .ToList()
                .PickRandom(1)
                .FirstOrDefault();
        }
    }
}
