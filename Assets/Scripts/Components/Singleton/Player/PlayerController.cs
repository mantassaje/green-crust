using Assets.Scripts.Ai;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PlayerController : NetworkBehaviour {

    public Biome SelectedBiome { get; private set; }
    public Biome HoverBiome { get; set; }

    [SyncVar]
    public uint Id;
    public NetworkConnection Connection { get; set; }

    private int PreviousUpdatePlayerId;
    public bool IsCameraMovedToStart;

    /// <summary>
    /// Might not be correct in singleplayer. For multiplayer sync only.
    /// </summary>
    [SyncVar]
    public int PlayerId;
    public Player Player
    {
        get
        {
            return ForAllPlayers
                ? Singles.World.PlayersTurn
                : Singles.Cache.GetPlayer(PlayerId);
        }
        set
        {
            PlayerId = value.IsNull() ? 0 : value.Id;
        }
    }

    public bool ForAllPlayers;

    public bool Rise;
    public bool Lower;
    public bool PlantGrass;
    public bool EndTurn;
    public bool IceMeteor;
    public bool RockMeteor;
    public bool NewGrounds;
    public bool TestTrigger;
    public Biome AncientMigrateToBiome;

    public bool HideInWorldUiElements;

    public bool AncientMigrateChoose;

    void Start()
    {
        DontDestroyOnLoad(this);
        if (hasAuthority)
        {
            Singles.PlayerController = this;
        }
    }

    public void SetVacantPlayer()
    {
        if (NetworkService.IsServer())
        {
            var player = PlayerService.GetVacant();
            PlayerId = player.Id;
        }
    }

    private void SetCameraPositionIfPossible()
    {
        if (!IsCameraMovedToStart)
        {
            if (Player != null && Player.LastCamPos != default)
            {
                Singles.CameraControlls.transform.position = Player.LastCamPos;
                IsCameraMovedToStart = true;
            }
        }
    }

    private void Update()
    {
        if (!hasAuthority) return;
        if (Singles.World == null || !Singles.World.IsLoaded) return;
        if (!ForAllPlayers && Player == null) return;

        SetCameraPositionIfPossible();

        if (ForAllPlayers && Singles.World.PlayersTurn.IsAi)
        {
            Singles.UiController.HideAll();
        }
        else
        {
            if (Singles.UiController.Tutorial.IsOpen)
            {
                return;
            }

            Singles.UiController.ShowAll();
            Singles.UiController.BiomeButtonsCard.SetActive(SelectedBiome.IsNotNull());
            Singles.UiController.BiomeInfoCard.SetActive(SelectedBiome.IsNotNull());

            PlayerActionTriggerService.CheckPresses(this);
            PlayerActionTriggerService.ActTriggeredActions(this);
            PlayerActionTriggerService.Reset(this);
        }
    }

    public void SetSelectedBiome(Biome biome)
    {
        if (Singles.PlayerController.Player.IsAi) return;

        SelectedBiome = biome;

        if (SelectedBiome != null)
        {
            SelectedBiome.OnBiomeClick();
        }
    }
}