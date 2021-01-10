using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Ai.AiMoves
{
    public class MoreNatureAiMove
    {
        public void Do(Player ai)
        {
            if (TryMigrate(ai)) return;

            var growBiome = GetGrowBiome(ai);
            if (growBiome.IsNull()) return;

            Debug.Log("Ai creating wildlife", growBiome);
            Debug.Log(ai.Mana);
            Singles.PlayerAction.CmdPlantGrass(growBiome.Key, ai.Id);
        }

        private bool TryMigrate(Player ai)
        {
            var migratableBiomes =
                from biome in Singles.Grid.GetAllBiomes()
                where  biome.Owner == ai
                    && biome.GetNearbyBiomesCache().Any(near => Singles.PlayerAction.CanMigrateAncients(ai, biome, near))
                select biome;

            if(migratableBiomes.Any())
            {
                var migrateFrom = migratableBiomes.ToList().PickRandom(1).FirstOrDefault();
                if (migrateFrom.IsNull()) return false;
                var migrateTo = migrateFrom.GetNearbyBiomesCache().First(near => near.Spec == migrateFrom.Spec);
                if (migrateTo.IsNull()) return false;
                Debug.Log("Ai migrating", migrateFrom);
                Debug.Log(ai.Mana);
                Singles.PlayerAction.CmdMigrateAncients(ai.Id, migrateFrom.Key, migrateTo.Key);
                return true;
            }

            return false;
        }

        private Biome GetGrowBiome(Player ai)
        {
            var biomes =
                from biome in Singles.Grid.GetAllBiomes()
                where biome.Owner == ai
                    && Singles.PlayerAction.CanPlantGrass(biome, ai)
                    && biome.Nature.Maturity < BiomeConstant.AncientMax
                orderby biome.Weather.Humidity + biome.Weather.Heat descending
                select biome;

            return biomes.Take(10).ToList().PickRandom(1).FirstOrDefault();
        }
    }
}
