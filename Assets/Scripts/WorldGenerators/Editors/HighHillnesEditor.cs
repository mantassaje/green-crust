using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.WorldGenerator.Generators
{
    public class HighHillnesEditor : IEditor
    {
        public void Apply()
        {
            var dryBiomes = Singles.Grid.GetAllBiomes()
                .Where(v => !v.IsWater && !v.IsAbyss);

            foreach(var dryBiome in dryBiomes)
            {
                if (UnityEngine.Random.Range(0f, 1f) > 0.9f)
                {
                    dryBiome.Crust.Height = UnityEngine.Random.Range(1, BiomeService.MaxHeight);
                }
            }
        }
    }
}
