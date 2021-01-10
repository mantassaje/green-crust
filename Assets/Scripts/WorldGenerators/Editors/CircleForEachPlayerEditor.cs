using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.WorldGenerator.Generators
{
    public class CircleForEachPlayerEditor : IEditor
    {
        private int _size;

        public CircleForEachPlayerEditor(int size)
        {
            _size = size;
        }

        public void Apply()
        {
            Singles.World.Players.ForEach(Apply);
        }

        private void Apply(Player player)
        {
            var biome = Singles.Grid.GetAllBiomes()
                .Where(IsProperSpawn)
                .ToList()
                .PickRandom(1)
                .FirstOrDefault();

            if (biome.IsNull()) return;

            PlayerService.SetStartingPosition(player, biome);

            var nearBiomes = Singles.Grid.GetAllNearby(biome, _size);
            foreach (var near in nearBiomes)
            {
                if (near.Crust.Height >= BiomeService.MinMountainHeight)
                {
                    near.Crust.Height = BiomeService.MinMountainHeight - 1;
                }
                if (near.IsAbyss)
                {
                    near.SetAbyss(false);
                }
            }

        }

        private bool IsProperSpawn(Biome biome)
        {
            var radius = Singles.Grid.Size / 5f;
            var biomeCenter = Singles.Grid.GetCenterBiome();
            var isDistanceOk = MathGeoHelper.IsInCircleEdge(
                biome.Key,
                biomeCenter.Key,
                radius);

            if (!isDistanceOk)
            {
                return false;
            }

            //Lazy fix of sometimes no 4th player placed into map
            var minDistance = Singles.World.Players.Count() >= 4 ? 7 : 8;

            var isNearbyPlayer = Singles.Grid.GetAllNearby(biome, minDistance).Any(near => near.Owner.IsNotNull());

            return !isNearbyPlayer;
        }
    }
}
