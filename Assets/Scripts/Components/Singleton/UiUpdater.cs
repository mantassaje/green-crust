using Assets.Scripts.Ai;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UiUpdater : MonoBehaviour {

    public List<ManualUpdateBehaviour> Behaviours;
    public bool RequirePlayerController = true;

    void Start()
    {
        Behaviours = Resources.FindObjectsOfTypeAll<ManualUpdateBehaviour>().ToList();
    }

    private void Update()
    {
        if(RequirePlayerController && Singles.PlayerController == null)
        {
            Debug.LogError("PlayerController is null. Not updating UI", this);
            return;
        }

        foreach(var behaviour in Behaviours)
        {
            behaviour.ManualUpdate();
        }
    }
}