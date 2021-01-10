using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public static class EndTurnService
{
    public static void EndTurn()
    {
        if (Singles.World.ActionInProgress) return;
        var player = Singles.World.PlayersTurn;
        GameplayPipelineService.ProcessPlayerTurn(player);

        var nextPlayer = GetNextPlayer(player);
        if (nextPlayer.IsNull())
        {
            GameplayPipelineService.ProcessTurnCycle();
            nextPlayer = Singles.World.Players.First();
        }

        SetPlayerTurn(nextPlayer);
    }

    public static void SetPlayerTurn(Player player)
    {
        if (!player.IsAi
            && Singles.PlayerController.ForAllPlayers)
        {
            Singles.CameraControlls.transform.position = player.LastCamPos;
        }

        Singles.World.PlayersTurn = player;
    }

    public static Player GetNextPlayer(Player endPlayer)
    {
        bool nextReturn = false;
        foreach (var pl in Singles.World.Players)
        {
            if (nextReturn) return pl;
            if (pl == endPlayer) nextReturn = true;
        }
        return null;
    }
}
