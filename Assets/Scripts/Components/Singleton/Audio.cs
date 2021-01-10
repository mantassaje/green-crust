using UnityEngine;

public class Audio : MonoBehaviour {

    public Audio()
    {
        Singles.Audio = this;
    }

    public AudioClip ForestBirds;
    public AudioClip DesertWind;
    public AudioClip SynthWind;
    public AudioClip GrasslandsGrasshopers;
    public AudioClip SeaWaves;
    public AudioClip JungleForest;

    public AudioClip MeteorExplosion;
    public AudioClip Earthquake;
    public AudioClip GrowthCracking;

    public void Start()
    {
    }
}
