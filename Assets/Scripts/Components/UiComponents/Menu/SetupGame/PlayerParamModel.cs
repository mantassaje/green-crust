using Assets.Scripts.Models;
using System;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PlayerParamModel : NetworkBehaviour
{
    [SyncVar]
    public string Name;

    [SyncVar]
    public bool IsAi;

    [SyncVar]
    public int PlayerControllerId;

    [SyncVar]
    public int SortOrder;

    void Start()
    {
        if(!NetworkServer.active)
        {
            var gameSetupCard = FindObjectOfType<SetupGameCard>();
            if (gameSetupCard != null)
            {
                gameSetupCard.ReaddExistingPlayers();
            }
        }
    }
}
