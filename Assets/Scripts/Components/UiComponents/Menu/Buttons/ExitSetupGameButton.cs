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
public class ExitSetupGameButton : MonoBehaviour {
    
    private MenuController MenuController;

    public void Start()
    {
        this.MenuController = FindObjectOfType<MenuController>();
        var button = gameObject.GetComponent<Button>();
        button.onClick.AddListener(ExitHostGame);
    }

    private void ExitHostGame()
    {
        if (NetworkServer.active)
        {
            NetworkManager.singleton.StopHost();
        }
        else if (NetworkClient.active)
        {
            NetworkManager.singleton.StopClient();
        }

        SceneManager.LoadScene(SceneConstant.Menu);
    }
}
