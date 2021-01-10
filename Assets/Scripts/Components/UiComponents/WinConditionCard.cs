using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinConditionCard : ManualUpdateBehaviour
{

    public Text ProgressText;
    public Text GoalText;

    void Start () {
		
	}

    public override void ManualUpdate()
    {
        if (Singles.World.IsNull()) return;

        if (Singles.World.Winner.IsNull())
        {
            GoalText.text = Singles.World.GoalSummary;
        }
        else
        {
            GoalText.text = "WINNER " + Singles.World.Winner.Name;
        }

        ProgressText.text = Singles.World.GoalProgressString;
    }
}
