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
public class HostGameButton : MonoBehaviour {

    private CustomNetworkManager NetworkManager;
    private MenuController MenuController;

    public bool IsOnlineMode;

    public SetupGameCard ChangeToCard;

    public void Start()
    {
        this.MenuController = FindObjectOfType<MenuController>();
        this.NetworkManager = FindObjectOfType<CustomNetworkManager>();
        var button = gameObject.GetComponent<Button>();
        button.onClick.AddListener(HostGame);
    }

    private void HostGame()
    {
        this.NetworkManager.IsOnlineMode = IsOnlineMode;
        this.NetworkManager.StartHost();
        ChangeToCard.IsOnlineMode = IsOnlineMode;
        this.MenuController.ChangeCard(ChangeToCard.gameObject);
    }
}
