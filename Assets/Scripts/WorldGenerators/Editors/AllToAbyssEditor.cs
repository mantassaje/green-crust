using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.WorldGenerator.Generators
{
    public class AllToAbyssEditor : IEditor
    {
        public void Apply()
        {
            var biomes = Singles.Grid.GetAllBiomes();
            foreach (var biome in biomes)
            {
                biome.SetAbyss(true);
            }
        }
    }
}
