using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.WorldGenerator.Generators
{
    public class CircleLandEditor: IEditor
    {
        public void Apply()
        {
            var radius = Singles.Grid.Size / 4f;
            var biomeCenter = Singles.Grid.GetCenterBiome();
            var biomes = Singles.Grid.GetAllBiomes();
            foreach (var biome in biomes)
            {
                if (!MathGeoHelper.IsInCircle(
                    biome.Key,
                    biomeCenter.Key,
                    radius))
                {
                    biome.SetAbyss(true);
                }
            }
        }
    }
}
