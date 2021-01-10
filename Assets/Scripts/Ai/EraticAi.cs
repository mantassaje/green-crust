using Assets.Scripts.Ai.AiMoves;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Ai
{
    public class EraticAi : IAi
    {
        public void Play(Player ai)
        {
            //ai.AddMana((int)(ai.Mana / 3f));

            var riseAiMove = new BlockAbyssWithMountainsAiMove();
            var newGroundsAiMove = new MoldNewGroundsAiMove();

            for (int i = 0; i < 10; i++)
            {
                if (ai.GetManaCost() > ai.Mana) break;

                var ownerBiomes = Singles.Grid.GetAllBiomes().Where(biome => biome.Owner == ai).ToList();

                var totalNaturePop = ownerBiomes.Sum(biome => biome.Nature.Population);
                var totalNatureCap = ownerBiomes.Sum(biome => BiomeNatureService.GetNatureCap(biome));
                var natureRate = totalNaturePop / totalNatureCap;

                float totalRainBiomeCount = ownerBiomes.Count(biome => !biome.IsWater
                    && biome.Weather.RainfallType >= RainfallTypes.Dry);
                float totalBiomeCount = ownerBiomes.Count();
                var rainBiomeRate = totalRainBiomeCount / totalBiomeCount;

                var totalWaterBiomeCount = ownerBiomes.Count(biome => biome.IsWater);
                var totalDeadDryBiomeCount = ownerBiomes.Count(biome => biome.Weather.RainfallType <= RainfallTypes.DeadDry && !biome.IsWater);
                var deadDryRate = totalDeadDryBiomeCount / totalBiomeCount;

                //Debug.Log("totalNatureRate " + totalNatureRate);
                //Debug.Log("totalRainBiomeRate " + totalRainBiomeRate);

                //Create water
                //Grow for a while
                //Then expand again by creating a water
                //Grow a bit again

                Debug.Log(new {
                    deadDryRate,
                    totalRainBiomeCount,
                    rainBiomeRate,
                    totalNaturePop,
                    totalNatureCap,
                    natureRate
                });

                if (totalRainBiomeCount <= 0)
                {
                    new MoreWaterAiMove().Do(ai);
                }
                else if (newGroundsAiMove.CanDo(ai) && Random.Range(0f, 1f) < 0.3f)
                {
                    newGroundsAiMove.Do(ai);
                }
                else if (riseAiMove.CanDo(ai) && Random.Range(0f, 1f) < 0.1f)
                {
                    riseAiMove.Do(ai);
                }
                //Make more water at the start
                else if (Singles.World.Turn < 8 && rainBiomeRate < Random.Range(0.40f, 0.60f)
                    || Singles.World.Turn >= 8 && rainBiomeRate < Random.Range(0.20f, 0.60f))
                {
                    new MoreWaterAiMove().Do(ai);
                }
                else
                {
                    new MoreNatureAiMove().Do(ai);
                }
            }

            Debug.Log("Turn done");
        }
    }
}
