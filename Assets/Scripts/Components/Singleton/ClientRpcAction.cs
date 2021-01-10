using Assets.Scripts.Models;
using System;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ClientRpcAction : NetworkBehaviour
{
    public static ClientRpcAction Singleton { get; private set; }

    private SetupGameCard _setupGameCard;

    //HACK Is an evil hack and should be changed when goals will be updated
    [SyncVar]
    public string GoalText;

    private void Start()
    {
        Singleton = this;
        DontDestroyOnLoad(this);
        _setupGameCard = FindObjectOfType<SetupGameCard>();
    }

    [ClientRpc]
    public void RpcRefreshUi()
    {
        if (_setupGameCard != null && !NetworkService.IsServer())
        {
            _setupGameCard.ReaddExistingPlayers();
        }
    }

    [ClientRpc]
    public void RpcWorldLoaded()
    {
        Singles.UiController.ShowAll();
    }

    [ClientRpc]
    public void RpcShowPopup(string text)
    {
        Singles.UiController.ShowPopup(text);
    }
}
