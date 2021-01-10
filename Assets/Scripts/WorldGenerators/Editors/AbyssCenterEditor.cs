using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.WorldGenerator.Generators
{
    public class AbyssCenterEditor : IEditor
    {
        private int _strength;

        public AbyssCenterEditor(int strength)
        {
            _strength = strength;
        }

        public void Apply()
        {
            var biome = Singles.Grid.GetCenterBiome();
            for(int i = 0; i < _strength; i++)
            {
                biome = biome.GetNearbyBiomesCache().ToList().PickRandom(1).FirstOrDefault();
                if (biome.IsNull()) return;
                biome.SetAbyss(true);
            }
        }
    }
}
