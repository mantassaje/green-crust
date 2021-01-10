using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Cache : MonoBehaviour {

    private IEnumerable<Player> _cachedPlayers;

    public Cache()
    {
        Singles.Cache = this;
    }

    public void Awake()
    {
    }

    public IEnumerable<Player> GetPlayers()
    {
        if (_cachedPlayers == null)
        {
            _cachedPlayers = GameObject.FindObjectsOfType<Player>();
        }

        return _cachedPlayers;
    }

    public Player GetPlayer(int id)
    {
        if (id == 0) return null;

        return GetPlayers().FirstOrDefault(player => player.Id == id);
    }
}
