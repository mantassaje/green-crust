using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Ai.AiMoves
{
    public class MoreWaterAiMove
    {
        public void Do(Player ai)
        {
            var dropMeteor = Singles.World.Turn <= 5
                && !ai.IsDisasterCasted
                && ai.GetDisasterManaCost() <= ai.Mana
                && ai.GetDisasterManaCost() < ai.GetManaCost() * 7;
            var useBiome = GetBiomeForAction(ai);

            if (useBiome.IsNull()) return;
            else if(dropMeteor)
            {
                Debug.Log("Ai droping ice meteor", useBiome);
                Debug.Log(ai.Mana);
                Singles.PlayerAction.CmdDropIceMeteor(useBiome.Key, ai.Id);
            }
            else
            {
                Debug.Log("Ai lowering ground", useBiome);
                Debug.Log(ai.Mana);
                Singles.PlayerAction.CmdLower(useBiome.Key, ai.Id);
            }
        }

        private Biome GetBiomeForAction(Player ai)
        {
            IEnumerable<Biome> selectedBiomes;

            selectedBiomes =
                from biome in Singles.Grid.GetAllBiomes()
                where biome.Owner == ai
                    && Singles.PlayerAction.CanLower(biome, ai)
                select biome;

            var prioritizedBiomes = 
                from biome in selectedBiomes
                orderby GetPoints(biome) descending
                select biome;

            return prioritizedBiomes.Take(5).ToList().PickRandom(1).FirstOrDefault();       
        }

        /// <summary>
        /// The more the better
        /// </summary>
        private float GetPoints(Biome biome)
        {
            if (biome.Weather.RainfallType >= RainfallTypes.Humid && biome.Nature.Population >= 2) return -9999999;

            var height = biome.Crust.Height * -3f; //max 4
            var pop = biome.Nature.Population * -2f;//max 5
            var heat = biome.Weather.Heat;//from 0 to 5
            var nearbyWaters = biome.GetNearbyBiomesCache().Count(near => near.IsWater && !(near.Spec is IcaCapOceanBiomeSpec));//from 0 to 6

            return height + pop + heat + nearbyWaters;
        }
    }
}
