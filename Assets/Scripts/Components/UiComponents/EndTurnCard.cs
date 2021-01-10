using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndTurnCard : ManualUpdateBehaviour
{

    public Text PlayerNameText;
    public Text TurnText;
    public Button EndTurnButton;

    private void Start()
    {
        EndTurnButton.onClick.AddListener(() => Singles.PlayerAction.CmdEndTurn(Singles.PlayerController.Player.Id));
    }

    public override void ManualUpdate()
    {
        if (Singles.PlayerController.Player.IsNull()) return;

        EndTurnButton.interactable = Singles.PlayerAction.CanEndTurn(Singles.PlayerController.Player);

        PlayerNameText.text = Singles.PlayerController.Player.Name;
        TurnText.text = "Age " + Singles.World.Turn;
    }
}
