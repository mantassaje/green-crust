using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Weather : NetworkBehaviour {

    private float NewHumidityToCommit { get; set; }
    private float ColdToCommit { get; set; }

    [SyncVar]
    public int Sorter;

    [SyncVar]
    public float Humidity = 0;

    public bool IsLastToReceiveHumidity = false;

    [SyncVar]
    public HeatTypes HeatType;

    [SyncVar]
    public RainfallTypes RainfallType;

    [SyncVar]
    public float Heat = 0;

    //Cold used only for emmisions because cold comes from abyss
    //It is converted to Heat for more intuitive wildlife and etc logic
    public float Cold = 0;
    
    public bool IsLastToReceiveCold = false;

    public void Awake()
    {
        if (!NetworkService.IsServer()) return;

        Sorter = Random.Range(0, 100000);
    }

    public void CommitHumidity()
    {
        if (!NetworkService.IsServer()) return;

        IsLastToReceiveHumidity = Humidity == 0 && NewHumidityToCommit != 0;
        Humidity = NewHumidityToCommit;
    }

    public void AddHumidity(float humidity)
    {
        NewHumidityToCommit += humidity;
    }

    public void SetHumidity(float humidity)
    {
        NewHumidityToCommit = humidity;
    }

    public void ResetHumidity(float humidity = 0)
    {
        Humidity = humidity;
        NewHumidityToCommit = humidity;
        IsLastToReceiveHumidity = true;
    }

    public void CommitCold()
    {
        IsLastToReceiveCold = Cold == 0 && ColdToCommit != 0;
        Cold = ColdToCommit;
        ColdToHeat();
    }

    public void AddCold(float cold)
    {
        ColdToCommit += cold;
    }

    public void SetCold(float cold)
    {
        ColdToCommit = cold;
    }

    public void ResetCold(float cold = 0)
    {
        Cold = cold;
        ColdToCommit = cold;
        ColdToHeat();
        IsLastToReceiveCold = true;
    }

    public void ColdToHeat()
    {
        if (!NetworkService.IsServer()) return;

        //10 cold is 0 heat
        //9 cold is 1 heat
        //0 cold is 10 heat
        Heat = 10f - Cold;
    }

    public float GetHeatRate()
    {
        return Heat / 10f;
    }

    public float GetRainfallRate()
    {
        return (Humidity / 10f).GetMinMax(0, 1);
    }
}
