using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Nature : NetworkBehaviour
{
    [SyncVar]
    public float Population;

    [SyncVar]
    public float Maturity = 0;

    public bool IsAncient { get
        {
            return Maturity >= BiomeConstant.AncientMax;
        }
    }

    public void BoundValues(float natureCap)
    {
        if (!NetworkService.IsServer()) return;

        Population = Population.GetMinMax(0, natureCap);
        Maturity = Maturity.GetMin(0);
    }
}
