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
public class OfflineNewGameButton : MonoBehaviour {

    private NetworkManager NetworkManager;
    private MenuController MenuController;

    public SetupGameCard ChangeToCard;

    public void Start()
    {
        this.MenuController = FindObjectOfType<MenuController>();
        this.NetworkManager = FindObjectOfType<NetworkManager>();
        var button = gameObject.GetComponent<Button>();
        button.onClick.AddListener(HostGame);
    }

    private void HostGame()
    {
        ChangeToCard.IsOnlineMode = false;
        this.MenuController.ChangeCard(ChangeToCard.gameObject);
    }
}
