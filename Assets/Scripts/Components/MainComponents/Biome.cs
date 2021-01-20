using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(Weather))]
[RequireComponent(typeof(Crust))]
[RequireComponent(typeof(Nature))]
[RequireComponent(typeof(CityState))]
[RequireComponent(typeof(AudioSource))]
public class Biome : NetworkBehaviour
{
    [SyncVar]
    public bool IsWater = false;

    [SyncVar]
    public bool IsAbyss = false;

    [SyncVar]
    public bool IsFarSpace = false;

    [SyncVar]
    public bool IsActionDone = false;

    public SpriteRenderer Hills;
    public SpriteRenderer FoliageTop;
    public SpriteRenderer FoliageBottom;
    public SpriteRenderer Ground;
    public SpriteRenderer Edge;
    public SpriteOptions UiLines;

    public SpriteOptions TopBorder;
    public SpriteOptions BotBorder;
    public SpriteOptions TopRightBorder;
    public SpriteOptions BotRightBorder;
    public SpriteOptions TopLeftBorder;
    public SpriteOptions BotLeftBorder;

    public SpriteOptions ManaGain;
    public SpriteRenderer Highlight;
    public SpriteRenderer Cloud;
    public SpriteRenderer Ancients;
    public CityStateHighlight CityStateHighlight;

    public Animator MeteorAnimator;

    public Weather Weather;
    public Crust Crust;
    public Nature Nature;
    public CityState CityState;

    public AudioSource AudioSource;

    [SyncVar]
    public int OwnerId;
    public Player Owner
    {
        get
        {
            return Singles.Cache.GetPlayer(OwnerId);
        }
        set
        {
            OwnerId = value.IsNull() ? 0 : value.Id;
        }
    }

    [SyncVar]
    public XYZKey Key;

    [SyncVar]
    public bool DrawTopBorder;
    [SyncVar]
    public bool DrawBotBorder;
    [SyncVar]
    public bool DrawTopLeftBorder;
    [SyncVar]
    public bool DrawBotLeftBorder;
    [SyncVar]
    public bool DrawTopRightBorder;
    [SyncVar]
    public bool DrawBotRightBorder;

    public BomeClimateVisual ClimateVisual = BomeClimateVisual.Brighten;
    public Timing ClimateVisualTiming = new Timing(1, TimingAddType.AddOnCurrentTime);

    public Timing MeteorEffectTiming = new Timing(0.14f, TimingAddType.AddOnCurrentTime);

    [SyncVar]
    public string SpecTypeName;

    public IBiomeSpec Spec { get; private set; }

    private IEnumerable<Biome> _nearbyBiomesCache;

	void Awake () {
        Spec = BiomeService.DefaultBiomeSpec;
        Weather = GetComponent<Weather>();
        Crust = GetComponent<Crust>();
        Nature = GetComponent<Nature>();
        CityState = GetComponent<CityState>();
        AudioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        if (!isServer)
        {
            Singles.Grid.AddLoadedBiome(this);
        }
        Edge.sortingOrder -= (int)transform.position.y;
        CityStateService.CreateNew(this);
    }

    /// <summary>
    /// Returns all touching this biome.
    /// </summary>
    public IEnumerable<Biome> GetNearbyBiomesCache(bool includeSelf = true)
    {
        if(_nearbyBiomesCache == null)
        {
            _nearbyBiomesCache = Singles.Grid.GetAllNearby(this);
        }
        return includeSelf 
            ? _nearbyBiomesCache
            : _nearbyBiomesCache.Where(biome => biome != this);
    }

    private void DrawBorders()
    {
        if (NetworkService.IsServer())
        {
            BiomeRenderService.BorderDrawLogic(this);
        }

        BiomeRenderService.BorderDraw(this);
    }

    /// <summary>
    /// TODO Currently not used. Maybe should be deleted.
    /// </summary>
    public void SetOrder(int order)
    {
        Ground.sortingOrder += order;
        Edge.sortingOrder += order;
        Edge.sortingOrder += UnityEngine.Random.Range(-100, 100);
        Cloud.sortingOrder += order;
        Ancients.sortingOrder += order;
        Hills.sortingOrder += order;
        FoliageTop.sortingOrder += order;
        FoliageBottom.sortingOrder += order;
        UiLines.SetOrder(order);
        ManaGain.SetOrder(order);

        TopBorder.SetOrder(order);
        BotBorder.SetOrder(order);
        BotRightBorder.SetOrder(order);
        TopRightBorder.SetOrder(order);
        TopLeftBorder.SetOrder(order);
        BotLeftBorder.SetOrder(order);

    }

    public void SetAbyss(bool isAbyss)
    {
        if (NetworkService.IsServer())
        {
            IsAbyss = isAbyss;
            BiomeService.RefreshBiomeType(this);
        }
    }

    public void SetOwner(Player player)
    {
        if (NetworkService.IsServer())
        { 
            if (IsAbyss) return;
            Owner = player;
        }
    }

    public Bounds GetBounds()
    {
        return Ground.bounds;
    }
	
	void Update () {

        if (NetworkService.IsServer())
        {
            IfWaterOrAbyss();
        }
        else
        {
            Spec = BiomeService.BiomeSpecs.FirstOrDefault(spec => spec.GetType().Name == SpecTypeName);
            if (Spec == null) return;
        }

        MeteorEffectTiming.CallbackIfElapsed();

        Highlight.enabled = false;

        //Slow flicker effect disabled
        /*if(ClimateVisualTiming.IsElapsed)
        {
            ClimateVisualTiming.Start();
            ClimateVisual = EnumHelper.Next(ClimateVisual);
        }*/

        
        Hills.sprite = Spec.MountainSprite.GetSafe((int)Crust.Height);
        FoliageTop.sprite = Spec.FoliageSpriteTop.GetSafe((int)Nature.Population);
        FoliageBottom.sprite = Spec.FoliageSpriteBotttom.GetSafe((int)Nature.Population);
        Cloud.sprite = BiomeRenderService.GetCloudSprite(this);
        Cloud.color = BiomeRenderService.GetCloudColor(this);
        Ground.sprite = Spec.Ground;
        Edge.enabled = !IsAbyss;

        if (!Singles.PlayerController.Player.IsAi && Singles.PlayerController.Player == Owner
            || Singles.World.IsSandbox)
        {
            ManaGain.SetLevel(GetManaOutput());
        }
        else
        {
            ManaGain.SetLevel(0);
        }

        SetColor();
        BiomeRenderService.SetUiLines(this);
        DrawBorders();
        Ancients.enabled = Nature.IsAncient;
        Ancients.sprite = Spec.Ancient;
        if(Singles.PlayerController.AncientMigrateChoose 
            && Singles.PlayerController.SelectedBiome.IsNotNull()
            && Singles.PlayerAction.CanMigrateAncients(Singles.PlayerController.Player, Singles.PlayerController.SelectedBiome, this))
        {
            Highlight.enabled = true;
        }

        if (Singles.PlayerController.HideInWorldUiElements) BiomeRenderService.HideAllUiElements(this);
    }

    private void SetColor()
    {
        var heatFilter = BiomeRenderService.GetHeatFilter(this);
        FoliageBottom.color = heatFilter;
        FoliageTop.color = heatFilter;
        Ground.color = heatFilter;
        Edge.color = heatFilter;
    }

    public void SetBiomeSpec(IBiomeSpec spec)
    {
        if (!NetworkService.IsServer()) return;

        Spec = spec;
        SpecTypeName = spec.GetType().Name;
        BoundValues();
    }

    public bool IsOwner(Player player)
    {
        if (ReferenceEquals(Owner, null)) return false;
        return ReferenceEquals(Owner, player);
    }

    public int GetManaOutput()
    {
        var mana = 0;
        mana += ManaService.GetManaBonuses(this);
        return mana;
    }

    /// <summary>
    /// Reset values accordingly if this biome is water or abyss
    /// </summary>
    private void IfWaterOrAbyss()
    {
        if (!NetworkService.IsServer()) return;

        if (IsAbyss)
        {
            Crust.Height = 0;
            Nature.Population = 0;
            IsWater = false;
            Owner = null;
        }
        else if (IsWater)
        {
            Crust.Height = 0;
            Nature.Population = 0;
        }
    }

    public void BoundValues()
    {
        if (!NetworkService.IsServer()) return;

        IfWaterOrAbyss();
        Nature.BoundValues(BiomeNatureService.GetNatureCap(this));
        Crust.BoundValues();
    }

    public void Rise(int amount = 1)
    {
        if (!NetworkService.IsServer()) return;

        if (amount < 0) amount = 0;
        if (IsWater)
        {
            IsWater = false;
            CityStateService.RemoveAsExplored(this);
            CityStateService.CreateNew(this);
        }
        else
        {
            Crust.Height += amount;

            if (!CityStateInfoService.CanBeExplored(this))
                CityStateService.RemoveAsExplored(this);
        }
        BoundValues();
        BiomeService.RefershData();
    }

    public void Lower(int amount = 1)
    {
        if (!NetworkService.IsServer()) return;

        if (amount < 0) amount = 0;
        Crust.Height -= amount;
        if (Crust.Height < 0)
        {
            IsWater = true;
            CityStateService.RemoveAsExplored(this);
        }
        BoundValues();
        BiomeService.RefershData();
    }

    public void PlantGrass(Player player, float amount)
    {
        if (!NetworkService.IsServer()) return;

        if (IsWater || IsAbyss) return;

        if (Nature.Population < 1)
        {
            CityStateService.CreateNew(this);
        }

        Nature.Population += amount;
        BiomeService.RefreshBiomeType(this);
        BoundValues();
    }

    public void KillPopulation()
    {
        if (!NetworkService.IsServer()) return;

        Nature.Population = 0;
        CityState.Population = 0;
        CityState.IsOnFire = true;
    }

    public void OnBiomeClick()
    {
        CityState.OnBiomeClick();
    }

    void OnMouseOver()
    {
        if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()) return;
        Singles.PlayerController.HoverBiome = this;
    }

    void OnMouseExit()
    {
        if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()) return;
        if (ReferenceEquals(this, Singles.PlayerController.HoverBiome))
            Singles.PlayerController.HoverBiome = this;
    }

    public void PlayMeteorAnimation(Action meteorEffectCallback)
    {
        if (Singles.World.PlayersTurn.IsAi)
        {
            meteorEffectCallback();
            MeteorAnimator.SetBool("doFall", true);
        }
        else
        {
            MeteorAnimator.SetBool("doFall", true);
            MeteorEffectTiming.Start(meteorEffectCallback);
        }
    }

    public void PlayAudioClip(AudioClip clip)
    {
        if (!Singles.World.PlayersTurn.IsAi)
        {
            AudioSource.clip = clip;
            AudioSource.Play();
        }
    }
}
