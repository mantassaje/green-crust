using Assets.Scripts.Models;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PlayerInput : MonoBehaviour
{
    public InputField NameInputField;
    public Button RemoveButton;
    public Text InfoText;

    public SetupGameCard MenuParent;

    public PlayerParamModel _player;

    void Start ()
    {
        RemoveButton.onClick.AddListener(RemoveClick);
        NameInputField.onValueChanged.AddListener((string value) => 
        {
            _player.Name = value;
            if (!NetworkServer.active)
            {
                Singles.PlayerAction.CmdSetName(_player.PlayerControllerId, value);
            }
        });
        Refresh();
    }

    private void RemoveClick()
    {
        MenuParent.RemovePlayer(_player);
    }

    public void SetPlayer(PlayerParamModel player, bool firstInput)
    {
        _player = player;
        NameInputField.text = player.Name;

        if (MenuParent.IsOnlineMode)
        {
            RemoveButton.gameObject.SetActive(player.IsAi);
        }
        else
        {
            RemoveButton.gameObject.SetActive(!firstInput);
        }

        Refresh();
    }

    private void Update()
    {
        Refresh();
    }

    private void Refresh()
    {
        if (_player.IsAi) InfoText.text = "AI";
        else
        {
            if (_player.Name != null && _player.Name.ToLower() == "giedrius") InfoText.text = "Lizard";
            else InfoText.text = "Human";
        }

        if (MenuParent.IsOnlineMode)
        {
            if (Singles.PlayerController == null) return;

            if (_player.PlayerControllerId == Singles.PlayerController.Id)
            {
                NameInputField.interactable = true;
            }
            else
            {
                NameInputField.interactable = NetworkServer.active && _player.IsAi;
                NameInputField.text = _player.Name;
            }
        }
        else
        {
            NameInputField.interactable = true;
        }
    }
}
