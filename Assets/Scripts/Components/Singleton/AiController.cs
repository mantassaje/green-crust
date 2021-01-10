using Assets.Scripts.Ai;
using System;
using UnityEngine;

public class AiController : MonoBehaviour {

    private void Update()
    {
        if (NetworkService.IsServer() && Singles.World.PlayersTurn.IsAi)
        {
            try
            {
                new EraticAi().Play(Singles.World.PlayersTurn);
            }
            catch(Exception ex)
            {
                Debug.LogError("AI failed with exception.");
                Debug.LogError(ex);
            }
            Singles.PlayerAction.CmdEndTurn(Singles.World.PlayersTurn.Id);
        }
    }
}