using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Ai.AiMoves
{
    public class BlockAbyssWithMountainsAiMove
    {
        public void Do(Player ai)
        {
            //The more points the likely
            var prioritizedBiome = GetBiomes(ai).OrderByDescending(biome => biome.GetNearbyBiomesCache().Sum(near => (int)near.Weather.HeatType) * -1
                + Singles.Grid.GetAllNearby(biome, 2).Count(near => near.IsAbyss));

            var riseBiome = prioritizedBiome.Take(10).ToList().PickRandom(1).FirstOrDefault();
            if (riseBiome.IsNull()) return;
            Singles.PlayerAction.CmdRise(riseBiome.Key, ai.Id);
        }

        public bool CanDo(Player ai)
        {
            return GetBiomes(ai).Any();
        }

        private IEnumerable<Biome> GetBiomes(Player ai)
        {
            var ownerBiomes = Singles.Grid.GetAllBiomes().Where(biome => biome.Owner == ai);
            return ownerBiomes.Where(biome => Singles.PlayerAction.CanRise(biome, ai)
                && !biome.IsWater
                && biome.GetNearbyBiomesCache().Any(near => near.IsAbyss));
        }
    }
}
