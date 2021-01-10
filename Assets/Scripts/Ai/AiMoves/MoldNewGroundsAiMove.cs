using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Ai.AiMoves
{
    public class MoldNewGroundsAiMove
    {
        public void Do(Player ai)
        {
            var abyssBiomes = GetAbyssQuery(ai);

            if (!abyssBiomes.Any()) return;
       
            var toNewGroundBiome = abyssBiomes.Take(5).ToList().PickRandom(1).First();
            Singles.PlayerAction.CmdCreateNewGround(toNewGroundBiome.Key, ai.Id);
        }

        public bool CanDo(Player ai)
        {
            var abyssBiomes = GetAbyssQuery(ai);

            return abyssBiomes.Any();
        }

        private IEnumerable<Biome> GetAbyssQuery(Player ai)
        {
            return from biome in Singles.Grid.GetAllBiomes()
                where biome.IsAbyss
                    && biome.GetNearbyBiomesCache().Any(nearby => nearby.Owner == ai)
                    && biome.GetNearbyBiomesCache().Count(nearby => nearby.IsAbyss) <= 3
                orderby biome.GetNearbyBiomesCache().Count(nearby => nearby.IsAbyss)
                select biome;
        }
    }
}
