using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressBar : ManualUpdateBehaviour
{

    public float BarProgressRate;
    public GameObject Bar;

    private float _barOriginalSizeX;

    void Start()
    {
        _barOriginalSizeX = Bar.transform.localScale.x;
    }

    public override void ManualUpdate()
    {
        var x = _barOriginalSizeX * BarProgressRate.GetMinMax(0.02f, 1f);
        Bar.transform.localScale = new Vector3(x, Bar.transform.localScale.y, Bar.transform.localScale.z);
    }
}
