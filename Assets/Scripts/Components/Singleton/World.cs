using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class World : NetworkBehaviour {

    public float SecondsPerUpdate = 0.02f;

    [SyncVar]
    public float CurrentSecond = 0f;

    [SyncVar]
    public int Turn = 1;

    [SyncVar]
    public bool IsLoaded = false;

    [SyncVar]
    public bool IsSandbox = false;

    public ClimateDebugMode ClimateDebugMode = ClimateDebugMode.None;

    //Should be in NetworkManager
    [SyncVar]
    public bool IsOnlneMode;

    public int HumidityDistance = 10;
    public int OceanHumidityBase = 10;
    public int HumidityEmitQuality = 10;
    /// <summary>
    /// Rate of emitter. Should be between 0 and 1
    /// </summary>
    public float HumidityEmitToNearbyConst = 0.04f;

    public int ColdDistance = 10;
    public int AbyssColdBase = 10;
    public int ColdEmitQuality = 10;
    /// <summary>
    /// Rate of emitter. Should be between 0 and 1
    /// </summary>
    public float ColdEmitToNearbyConst = 0.04f;

    [SyncVar]
    public string GoalProgressString;

    [SyncVar]
    public string GoalSummary;

    public List<Player> Players;

    [SyncVar]
    public int WinnerId;
    public Player Winner
    {
        get
        {
            return Singles.Cache.GetPlayer(WinnerId);
        }
        set
        {
            WinnerId = value.IsNull() ? 0 : value.Id;
        }
    }

    [SyncVar]
    public int PlayersTurnId;
    public Player PlayersTurn
    {
        get
        {
            return Singles.Cache.GetPlayer(PlayersTurnId);
        }
        set
        {
            PlayersTurnId = value.IsNull() ? 0 : value.Id;
        }
    }

    [SyncVar]
    public bool ActionInProgress;

    public World()
    {
        
    }

    private void Awake()
    {
        Singles.World = this;
    }

    private void Update()
    {
        if (!NetworkService.IsServer()) return;

        CurrentSecond += SecondsPerUpdate;

        if (GoalService.WinCondition != null)
        {
            GoalProgressString = GoalService.WinCondition.GetProgressText();
            GoalSummary = GoalService.WinCondition.GetGoalText();
        }
    }


    /// <summary>
    /// Prevents error spam when scene is reloaded
    /// </summary>
    public void DisableAll()
    {
        Singles.Grid.GetAllBiomes().ToList().ForEach(biome => biome.gameObject.SetActive(false));
    }
}
