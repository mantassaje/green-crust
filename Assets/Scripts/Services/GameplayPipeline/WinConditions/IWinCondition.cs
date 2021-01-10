using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public interface IWinCondition
{
    string GetGoalText();
    string GetProgressText();
    bool IsWinner(Player player);
}

