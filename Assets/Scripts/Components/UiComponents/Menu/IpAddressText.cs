using Assets.Scripts.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class IpAddressText : MonoBehaviour {

    private CustomNetworkManager _networkManager;
    private Text _text;

    public void Start()
    {
        _networkManager = FindObjectOfType<CustomNetworkManager>();
        _text = GetComponent<Text>();
    }

    private void Update()
    {
        if (NetworkServer.active && _networkManager.IsOnlineMode)
        {
            _text.text = "Local IP address " + GetLocalIpAddress();// + ":" + _networkManager.networkPort;
        }
        else
        {
            _text.text = null;
        }
    }

    private string GetLocalIpAddress()
    {
        var host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (var ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                return ip.ToString();
            }
        }
        throw new Exception("No network adapters with an IPv4 address in the system!");
    }
}
