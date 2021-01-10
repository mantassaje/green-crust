using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Crust : NetworkBehaviour
{
    [SyncVar]
    public int Height = 0;

    void Start () {
		
	}

    /// <summary>
    /// From 0.0 to 1.0
    /// </summary>
    public float GetHeightRatio()
    {
        return (float)Height / (float)BiomeService.MaxHeight;
    }

    /// <summary>
    /// From 0.0 to 1.0
    /// </summary>
    public float GetHeightRatioSteped()
    {
        return (float)Height / (float)BiomeService.MaxHeight;
    }

    public void BoundValues()
    {
        if (!NetworkService.IsServer()) return;

        Height = Height.GetMinMax(0, BiomeService.MaxHeight);
    }
}
