using Assets.Scripts.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class StartSandboxGameButton : MonoBehaviour {

    private CustomNetworkManager NetworkManager;

    public PlayerParamModel PlayerParamModelTemplate;

    public void Start()
    {
        this.NetworkManager = FindObjectOfType<CustomNetworkManager>();
        var button = gameObject.GetComponent<Button>();
        button.onClick.AddListener(HostGame);
    }

    private void HostGame()
    {
        this.NetworkManager.IsOnlineMode = false;
        this.NetworkManager.StartHost();

        var playerParam = Instantiate(PlayerParamModelTemplate);

        Singles.WorldStartParamModel = new WorldStartParamModel()
        {
            IsSandbox = true,
            Players = new List<PlayerParamModel>
            {
                playerParam
            }
        };

        GoalService.SetWinCondition(null);

        this.NetworkManager.ServerChangeScene(SceneConstant.GameWorld);
    }
}
