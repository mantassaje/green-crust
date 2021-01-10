using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class TotalManaWinCondition : IWinCondition
{
    private int _manaRequired;

    public TotalManaWinCondition(int manaRequired)
    {
        _manaRequired = manaRequired;
    }

    public string GetGoalText()
    {
        return string.Format("First to collect over the game {0} energy wins", _manaRequired);
    }

    public string GetProgressText()
    {
        var builder = new StringBuilder();
        foreach (var player in Singles.World.Players)
        {
            builder.Append(string.Format("{0}: {1}      ", player.Name, player.TotalManaCollected));
        }
        return builder.ToString();
    }

    public bool IsWinner(Player player)
    {
        return player.TotalManaCollected >= _manaRequired;
    }
}

