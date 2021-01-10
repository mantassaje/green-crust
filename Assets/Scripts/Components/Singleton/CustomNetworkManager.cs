using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class CustomNetworkManager : NetworkManager
{
    public bool IsOnlineMode;

    public event EventHandler<NetworkConnection> EventOnNewPlayer;
    public event EventHandler<NetworkConnection> EventOnRemovedPlayer;

    public GameObject ClientRpcActionPrefab;

    public override NetworkClient StartHost()
    {
        var client = base.StartHost();

        maxConnections = IsOnlineMode ? 4 : 1;
        networkSceneName = null;
        var clientRpcActon = Instantiate(ClientRpcActionPrefab);
        NetworkServer.Spawn(clientRpcActon);

        return client;
    }

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        var playerController = GameObject.Instantiate(playerPrefab).GetComponent<PlayerController>();
        playerController.Connection = conn;
        NetworkServer.AddPlayerForConnection(conn, playerController.gameObject, playerControllerId);
        playerController.Id = conn.GetId().GetValueOrDefault();
        playerController.ForAllPlayers = !IsOnlineMode;

        if (EventOnNewPlayer != null)
        {
            EventOnNewPlayer.Invoke(this, conn);
        }
    }

    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);
    }

    public override void OnServerRemovePlayer(NetworkConnection conn, UnityEngine.Networking.PlayerController player)
    {
        base.OnServerRemovePlayer(conn, player);
    }

    public override void OnServerError(NetworkConnection conn, int errorCode)
    {
        base.OnServerError(conn, errorCode);
    }

    public override void OnServerConnect(NetworkConnection conn)
    {
        if (!IsOnlineMode)
            conn.Disconnect();

        var playerParamsCount = FindObjectsOfType<PlayerParamModel>().Count();
        if (playerParamsCount >= 4)
            conn.Disconnect();

        if (SceneManager.GetActiveScene().name != SceneConstant.Menu)
            conn.Disconnect();

        base.OnServerConnect(conn);
    }

    public override void OnServerDisconnect(NetworkConnection conn)
    {
        if (EventOnRemovedPlayer != null)
        {
            EventOnRemovedPlayer.Invoke(this, conn);
        }

        if (Singles.World != null && Singles.World.IsLoaded)
        {
            var playerController = FindObjectsOfType<PlayerController>()
                .FirstOrDefault(controller => controller.Connection == conn);

            if (playerController != null && playerController.Player != null)
            {
                playerController.Player.IsAi = true;
                playerController.Player.Name = "AI " + playerController.Player.Name;
            }
        }

        base.OnServerDisconnect(conn);
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
    }

    public override void OnStopServer()
    {
        ServerChangeScene(SceneConstant.Menu);

        // If you host then end game and try to join it you get error that conneciton is already ready.?
        var hostConnection = NetworkClient.allClients.First();
        if (hostConnection != null) hostConnection.connection.isReady = false;

        if (ClientRpcAction.Singleton != null)
        {
            Destroy(ClientRpcAction.Singleton);
        }

        base.OnStopServer();

        networkSceneName = null;
    }

    public override void OnClientDisconnect(NetworkConnection conn)
    {
        base.OnClientDisconnect(conn);
        SceneManager.LoadScene(SceneConstant.Menu);
        networkSceneName = null;
    }

    public override void OnClientError(NetworkConnection conn, int errorCode)
    {
        base.OnClientError(conn, errorCode);
    }

    public override void OnStopClient()
    {
        base.OnStopClient();
        SceneManager.LoadScene(SceneConstant.Menu);
        networkSceneName = null;
    }

    public override void OnClientSceneChanged(NetworkConnection conn)
    {
        base.OnClientSceneChanged(conn);
    }

    public override void OnServerSceneChanged(string sceneName)
    {
        base.OnServerSceneChanged(sceneName);
    }

    public override void ServerChangeScene(string newSceneName)
    {
        base.ServerChangeScene(newSceneName);
    }
}
