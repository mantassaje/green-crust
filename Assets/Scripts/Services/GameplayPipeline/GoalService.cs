using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public static class GoalService
{
    public static IWinCondition WinCondition { get; private set; }

    public static void SetWinCondition(IWinCondition winCondition)
    {
        WinCondition = winCondition;
    }

    public static void CheckIfWinner(Player player)
    {
        if (WinCondition == null) return;
        if(WinCondition.IsWinner(player))
        {
            SetWinner(player);
        }
    }

    public static void SetWinner(Player player)
    {
        if (Singles.World.Winner.IsNull())
        {
            Singles.World.Winner = player;
            ClientRpcAction.Singleton.RpcShowPopup(player.Name + " is winner!");
        }
    }
}
