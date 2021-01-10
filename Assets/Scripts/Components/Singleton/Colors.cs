using System.Collections.Generic;
using UnityEngine;

public class Colors : MonoBehaviour {

    public Colors()
    {
        Singles.Colors = this;
    }

    public void Awake()
    {
    }

    public Color Artic;
    public Color Desert;
    public Color ColdDesert;
    public Color DeadCold;
    public Color IceCap;
    public Color Tundra;
    public Color Taiga;
    public Color Grassland;
    public Color TemperateForest;
    public Color TemperateRainforest;
    public Color Savana;
    public Color TropicalForest;
    public Color Rainforest;
    public Color Uninhabited;

    public List<Color> PlayerColors;
}
