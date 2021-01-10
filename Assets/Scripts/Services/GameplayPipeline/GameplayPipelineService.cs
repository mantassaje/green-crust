using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public static class GameplayPipelineService
{
    /// <summary>
    /// Perform all logic after the player ends turn.
    /// Give mana, recalculate biome humidity heat and etc. Grow biomes.
    /// </summary>
    /// <param name="player"></param>
    public static void ProcessPlayerTurn(Player player)
    {
        PlayerService.ResetPlayerForEndTurn(player);
        PlayerService.AddEndTurnMana(player);
        PlayerActionTriggerService.EndTurnReset(Singles.PlayerController);
        BiomeNatureService.Grow(player);
        //CivEndTurnService.EndTurnCityGrowth(player);
        //CivEndTurnService.EndTurnDecisions(player);
        GoalService.CheckIfWinner(player);
        BiomeService.DefineAllBiomes();
    }

    /// <summary>
    /// Perform all logic after all player end their turn.
    /// </summary>
    public static void ProcessTurnCycle()
    {
        Singles.World.Turn++;
        var biomes = Singles.Grid.GetAllBiomes();
        foreach (var biome in biomes)
        {
            biome.IsActionDone = false;
        }

        if (!Singles.World.IsOnlneMode)
        {
            CityStateEndTurnService.EndTurn();
        }
    }
}
