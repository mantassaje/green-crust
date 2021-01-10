using Assets.Scripts.Models;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InGameMenuCard : ManualUpdateBehaviour
{

    public Button ContinueButton;
    public Button EndGameButton;
    public Button RestartButton;
    public Button ExitButton;

    void Start () {

        ContinueButton.onClick.AddListener(() => gameObject.SetActive(false));
        EndGameButton.onClick.AddListener(() => 
        {
            Singles.World.DisableAll();
            if (NetworkServer.active) NetworkManager.singleton.StopHost();
            else NetworkManager.singleton.StopClient();
        });

        RestartButton.onClick.AddListener(() =>
        {
            Singles.World.DisableAll();
            SceneManager.LoadScene(SceneConstant.GameWorld);
            //NetworkManager.singleton.ServerChangeScene(SceneConstant.GameWorld);
        });

        var disableRestart = FindObjectOfType<CustomNetworkManager>().IsOnlineMode || !NetworkServer.active;
        RestartButton.interactable = !disableRestart;

        ExitButton.onClick.AddListener(() => Application.Quit());
    }
}
