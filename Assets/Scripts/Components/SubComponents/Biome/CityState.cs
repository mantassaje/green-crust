using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CityState : NetworkBehaviour
{
    private Biome _biome;

    [SyncVar]
    public float Population;

    [SyncVar]
    public string Name;

    public bool IsNotable
    {
        get
        {
            return Population > 1f && !IsMuted && !IsDead;
        }
    }

    [SyncVar]
    public bool IsDead;

    /// <summary>
    /// Bound to be a tribe and can not grow large and visible to player.
    /// </summary>
    [SyncVar]
    public bool IsMuted;

    [SyncVar]
    public int IsRuinTurns;

    /// <summary>
    /// Shadow of past self.
    /// </summary>
    public bool IsRuin
    {
        get
        {
            return IsRuinTurns > 0;
        }
    }

    [SyncVar]
    public bool IsOnFire;

    [SyncVar]
    public bool EventIsRead;

    public ICityStateEvent Event;
    public CityStateEventStatus EventStatus;

    public bool IsVisible
    {
        get
        {
            return !IsDead && (IsNotable || IsRuin);
        }
    }

    public SpriteOptions CitySprite;
    public Sprite RuinsSprite;
    public SpriteRenderer Fire;

    public CityStateData LastAge { get; set; }
    public CityStateData CurrentAge { get; set; }

    public CityStatePersonality Personality { get; set; }

    public List<CityStateHistory> History { get; set; }

    public List<Biome> Explored { get; set; }

    private void Start()
    {
        _biome = GetComponent<Biome>();
    }

    private void Update()
    {
        //For now disables in multiplayer
        if (Singles.World.IsOnlneMode)
        {
            IsDead = true;
        }
        else
        {
            IsDead = _biome.Spec.IsBarren || _biome.Spec.IsWater || _biome.Spec.IsDead;
        }

        if (IsMuted)
        {
            Population = Population.GetMinMax(0f, 0.5f);
        }

        if (IsDead)
        {
            Population = 0;
            CitySprite.SetLevel(0);
        }
        else if (IsNotable)
        {
            CitySprite.SetLevel(CityStateInfoService.GetSpriteIndex(this));
        }
        else if (IsRuin)
        {
            CitySprite.SetSprite(RuinsSprite);
        }
        else
        {
            CitySprite.SetLevel(0);
        }

        Fire.enabled = IsVisible && IsOnFire;
    }

    public void OnBiomeClick()
    {
        if (Event != null 
            && EventStatus == CityStateEventStatus.Ignored
            && (Singles.World.IsSandbox || _biome.Owner == Singles.PlayerController.Player))
        {
            Singles.UiController.CityStateEventCard.Open(_biome);
        }
    }
}
