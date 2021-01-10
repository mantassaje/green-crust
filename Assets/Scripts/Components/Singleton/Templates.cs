using UnityEngine;

public class Templates : MonoBehaviour {

    public Templates()
    {
        Singles.Templates = this;
    }

    public void Start()
    {
    }

    public Biome Biome;
    public Player Player;
    public ClientRpcAction SetupGameNetworkSyncData;
}
