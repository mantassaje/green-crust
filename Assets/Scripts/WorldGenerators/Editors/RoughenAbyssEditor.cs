using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.WorldGenerator.Generators
{
    public class RoughenAbyssEditor: IEditor
    {
        private int _strength;

        public RoughenAbyssEditor(int strength)
        {
            _strength = strength;
        }

        public void Apply()
        {
            for(int i = 0; i < _strength; i++)
            {
                RoughenAbyss();
            }
        }

        private void RoughenAbyss()
        {
            var edges = Singles.Grid.GetAllBiomes()
            .Where(v => v.IsAbyss
                && v.GetNearbyBiomesCache().Any(near => !near.IsAbyss))
            .ToList();
            foreach (var edge in edges)
            {
                var revert = UnityEngine.Random.Range(0, 3) == 0;
                if (revert) edge.IsAbyss = false;
            }
        }
    }
}
