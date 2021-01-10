using Assets.Scripts.Models;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SetupGameCard : MonoBehaviour {

    private WorldStartParamModel _worldStartParams;
    private List<PlayerInput> _playerInputs;
    private CustomNetworkManager _networkManager;
    private bool _isInit = false;
    private int _nonNetworkId = -1;
    private int _sortOrder = 0;

    public bool IsOnlineMode;

    public Button StartButton;
    public Button AddPlayerButton;
    public Button AddAiButton;
    public PlayerInput PlayerInputTemplate;
    public PlayerParamModel PlayerParamModelTemplate;
    public Dropdown GoalDropdown;
    public GameObject SetupGameNetworkSyncDataTemplate;
    public Text GoalText;
    public Text NoCityCtatesWarning;

    private void Init()
    {
        if(!_isInit)
        {
            _networkManager = FindObjectOfType<CustomNetworkManager>();
            _networkManager.EventOnNewPlayer += AddNetworkPlayer;
            _networkManager.EventOnRemovedPlayer += RemoveNetworkPlayer;

            _worldStartParams = new WorldStartParamModel()
            {
                IsOnlneMode = IsOnlineMode,
                Players = new List<PlayerParamModel>()
            };
            
            StartButton.onClick.AddListener(StartClick);
            AddPlayerButton.onClick.AddListener(AddHumanPlayer);
            AddAiButton.onClick.AddListener(AddAiPlayer);

            GoalDropdown.onValueChanged.AddListener(SetGoal);

            RefreshUi();

            _isInit = true;
        }
    }

    void Start () {
        Init();
    }

    void OnDestroy()
    {
        _networkManager.EventOnNewPlayer -= AddNetworkPlayer;
        _networkManager.EventOnRemovedPlayer -= RemoveNetworkPlayer;
    }

    private void SpawnSetupGameNetworkSyncData()
    {
        var setupGameSync = Instantiate(SetupGameNetworkSyncDataTemplate);
        NetworkServer.Spawn(setupGameSync);
    }

    private void OnEnable()
    {
        Init();
        SetGoal(1);

        var existingPlayer = FindObjectsOfType<PlayerParamModel>();

        foreach(var player in existingPlayer)
        {
            RemovePlayer(player);
        }

        if (IsOnlineMode)
        {
            if (NetworkServer.active)
            {
                foreach (var conn in NetworkServer.connections)
                {
                    var id = conn.GetId();
                    if (id == null) continue;

                    AddPlayer((int)id.Value, false, "");
                }
            }
            else
            {
                ReaddExistingPlayers();
            }
        }
        else
        {
            AddHumanPlayer();
            AddAiPlayer();
        }

        RefreshUi();
    }

    private void AddNetworkPlayer(object sender, NetworkConnection conn)
    {
        if (!IsOnlineMode) return;

        var id = conn.GetId();
        if (id == null) return;

        var existingPlayer = _worldStartParams.Players.FirstOrDefault(player => player.PlayerControllerId == id);
        if (existingPlayer != null) return;

        AddPlayer((int)id.Value, false, "");
    }

    private void RemoveNetworkPlayer(object sender, NetworkConnection conn)
    {
        if (!IsOnlineMode) return;

        var removePlayer = _worldStartParams.Players.FirstOrDefault(player => player.PlayerControllerId == conn.GetId());

        if(removePlayer != null)
            RemovePlayer(removePlayer);
    }

    private void StartClick()
    {
        if (NetworkServer.active)
        {
            Singles.WorldStartParamModel = _worldStartParams;
            NetworkManager.singleton.ServerChangeScene(SceneConstant.GameWorld);
        }
    }

    public void AddHumanPlayer()
    {
        _nonNetworkId--;
        AddPlayer(_nonNetworkId, false, "");
    }

    public void AddAiPlayer()
    {
        _nonNetworkId--;
        AddPlayer(_nonNetworkId, true, "");
    }

    private void AddPlayer(int id, bool isAi, string name)
    {
        if(_worldStartParams.Players.Count >= 4)
        {
            return;
        }

        var player = Instantiate<PlayerParamModel>(PlayerParamModelTemplate);
        NetworkServer.Spawn(player.gameObject);
        DontDestroyOnLoad(player);

        player.PlayerControllerId = id;
        player.IsAi = isAi;
        player.Name = name;
        player.SortOrder = _sortOrder++;

        _worldStartParams.Players.Add(player);
        RefreshUi();
    }

    public void RemovePlayer(PlayerParamModel player)
    {
        _worldStartParams.Players.Remove(player);
        NetworkServer.Destroy(player.gameObject);
        Destroy(player.gameObject);

        RefreshUi();
    }

    /// <summary>
    /// Used by clients to add server synced PlayerParamModel list
    /// </summary>
    public void ReaddExistingPlayers()
    {
        var players = FindObjectsOfType<PlayerParamModel>();
        _worldStartParams.Players = players.OrderBy(player => player.SortOrder).ToList();

        RefreshUi();
    }

    public void RefreshUi()
    {
        NoCityCtatesWarning.gameObject.SetActive(IsOnlineMode);

        if (NetworkServer.active && ClientRpcAction.Singleton != null)
        {
            ClientRpcAction.Singleton.RpcRefreshUi();
        }

        if (IsOnlineMode)
        {
            StartButton.gameObject.SetActive(NetworkServer.active);
            AddAiButton.gameObject.SetActive(NetworkServer.active);
            AddPlayerButton.gameObject.SetActive(false);

            if (NetworkServer.active)
            {
                GoalDropdown.gameObject.SetActive(true);
                GoalText.gameObject.SetActive(false);
            }
            else
            {
                GoalDropdown.gameObject.SetActive(false);
                GoalText.gameObject.SetActive(true);
            }
        }
        else
        {
            StartButton.gameObject.SetActive(true);
            AddPlayerButton.gameObject.SetActive(true);
            AddAiButton.gameObject.SetActive(true);
            GoalDropdown.gameObject.SetActive(true);
            GoalText.gameObject.SetActive(false);
        }
        

        if (_playerInputs != null)
        {
            foreach (var playerInput in _playerInputs)
            {
                DestroyObject(playerInput.gameObject);
            }
        }

        _playerInputs = new List<PlayerInput>();

        for (int i = 0; i < _worldStartParams.Players.Count; i++)
        {
            var newPlayerInput = Instantiate(PlayerInputTemplate, this.transform);
            newPlayerInput.SetPlayer(_worldStartParams.Players[i], i == 0);
            newPlayerInput.transform.localPosition = new Vector3(
                PlayerInputTemplate.transform.localPosition.x,
                PlayerInputTemplate.transform.localPosition.y + i * -60,
                PlayerInputTemplate.transform.localPosition.z
            );
            newPlayerInput.gameObject.SetActive(true);

            _playerInputs.Add(newPlayerInput);
        }
    }

    private void Update()
    {
        if (ClientRpcAction.Singleton != null)
        {
            if (NetworkServer.active)
                ClientRpcAction.Singleton.GoalText = GoalService.WinCondition.GetGoalText();
            else GoalText.text = ClientRpcAction.Singleton.GoalText;
        }
    }

    private void RefreshPlayersByPlayerControler()
    {
        var controllers = FindObjectOfType<PlayerController>();

    }

    private void SetGoal(int selection)
    {
        switch (selection)
        {
            case 0:
                GoalService.SetWinCondition(new TotalManaWinCondition(3000));
                break;
            case 1:
                GoalService.SetWinCondition(new TotalManaWinCondition(6000));
                break;
            default:
                GoalService.SetWinCondition(new TotalManaWinCondition(10000));
                break;
        }
    }
}
