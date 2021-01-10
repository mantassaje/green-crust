using Assets.Scripts.Models;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class JoinGameButton : MonoBehaviour {

    private CustomNetworkManager NetworkManager;

    public InputField IpAddressField;
    public SetupGameCard ChangeToCard;

    public void Start()
    {
        NetworkManager = FindObjectOfType<CustomNetworkManager>();
        var button = gameObject.GetComponent<Button>();
        button.onClick.AddListener(JoinGame);
    }

    private void JoinGame()
    {
        var ip = IpAddressField.text;
        var port = 7777;
        if (string.IsNullOrWhiteSpace(ip)) return;

        var ipParts = ip.Split(':');
        if (ipParts.Length == 2)
        {
            ip = ipParts[0];
            port = int.Parse(ipParts[1]);
        }

        NetworkManager.networkAddress = ip;
        NetworkManager.networkPort = port;
        var client = NetworkManager.StartClient();
        NetworkManager.IsOnlineMode = true;

        var menuController = FindObjectOfType<MenuController>();
        ChangeToCard.IsOnlineMode = true;
        menuController.ChangeCard(ChangeToCard.gameObject);
    }
}
