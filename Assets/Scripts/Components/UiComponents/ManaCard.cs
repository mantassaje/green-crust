using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManaCard : ManualUpdateBehaviour
{

    public Text ManaCountText;
    public ShowTooltip Tooltip;

	void Start () {
		
	}

    public override void ManualUpdate()
    {
        if (Singles.PlayerController.Player.IsNull()) return;

        var manaGain = PlayerService.GetTotalManaGain(Singles.PlayerController.Player);
        var manaCap = Singles.PlayerController.Player.GetMaxMana();
        var manaCount = Singles.PlayerController.Player.Mana;

        ManaCountText.text = manaCount.ToString();


        var tooltipTtext = "Gain " + manaGain.GetMin(0).ToString() + Environment.NewLine;
        tooltipTtext = tooltipTtext + "Cap " + manaCap.GetMin(0).ToString() + Environment.NewLine;

        Tooltip.Text = tooltipTtext;
    }
}
