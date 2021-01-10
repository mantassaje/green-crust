using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class BiomeButtonsCard : ManualUpdateBehaviour
{

    public Button GrowButton;
    public Button MigrateButton;
    public Button LowerGroundButton;
    public Button RiseGroundButton;
    public Button RockMeteorButton;
    public Button IceMeteorButton;
    public Button NewGroundsButton;

    public Text CheapButtonsText;
    public Text ExpensiveButtonsText;
    public Text AbyssButtonsText;

    public Text BiomeActionDoneText;
    public Text DissasterCastedText;

    void Start () {
        GrowButton.onClick.AddListener(() => Singles.PlayerAction.CmdPlantGrass(Singles.PlayerController.SelectedBiome.Key, Singles.PlayerController.Player.Id));
        MigrateButton.onClick.AddListener(() => Singles.PlayerAction.StartChooseMigrateAncients(Singles.PlayerController.Player, Singles.PlayerController.SelectedBiome));
        RiseGroundButton.onClick.AddListener(() => Singles.PlayerAction.CmdRise(Singles.PlayerController.SelectedBiome.Key, Singles.PlayerController.Player.Id));
        LowerGroundButton.onClick.AddListener(() => Singles.PlayerAction.CmdLower(Singles.PlayerController.SelectedBiome.Key, Singles.PlayerController.Player.Id));
        IceMeteorButton.onClick.AddListener(() => Singles.PlayerAction.CmdDropIceMeteor(Singles.PlayerController.SelectedBiome.Key, Singles.PlayerController.Player.Id));
        RockMeteorButton.onClick.AddListener(() => Singles.PlayerAction.CmdDropRockMeteor(Singles.PlayerController.SelectedBiome.Key, Singles.PlayerController.Player.Id));
        NewGroundsButton.onClick.AddListener(() => Singles.PlayerAction.CmdCreateNewGround(Singles.PlayerController.SelectedBiome.Key, Singles.PlayerController.Player.Id));
    }

    public override void ManualUpdate()
    {
        if (Singles.PlayerController.Player.IsNotNull())
        {
            CheapButtonsText.text = Singles.PlayerController.Player.GetManaCost().ToString();
            ExpensiveButtonsText.text = Singles.PlayerController.Player.GetDisasterManaCost().ToString();
            AbyssButtonsText.text = Singles.PlayerController.Player.GetMidManaCost().ToString();
        }

        if(Singles.PlayerController.SelectedBiome.IsNotNull() && Singles.PlayerController.Player.IsNotNull())
        {
            MigrateButton.image.sprite = Singles.PlayerController.SelectedBiome.Spec.Ancient;
            MigrateButton.gameObject.SetActive(Singles.PlayerController.SelectedBiome.Spec.Ancient != null);

            GrowButton.interactable = Singles.PlayerAction.CanPlantGrass(Singles.PlayerController.SelectedBiome, Singles.PlayerController.Player);
            MigrateButton.interactable = Singles.PlayerAction.CanMigrateAncients(Singles.PlayerController.Player, Singles.PlayerController.SelectedBiome);
            RiseGroundButton.interactable = Singles.PlayerAction.CanRise(Singles.PlayerController.SelectedBiome, Singles.PlayerController.Player);
            LowerGroundButton.interactable = Singles.PlayerAction.CanLower(Singles.PlayerController.SelectedBiome, Singles.PlayerController.Player);
            IceMeteorButton.interactable = Singles.PlayerAction.CanPerformDisaster(Singles.PlayerController.SelectedBiome, Singles.PlayerController.Player);
            RockMeteorButton.interactable = Singles.PlayerAction.CanPerformDisaster(Singles.PlayerController.SelectedBiome, Singles.PlayerController.Player);
            NewGroundsButton.interactable = Singles.PlayerAction.CanCreateNewGround(Singles.PlayerController.SelectedBiome, Singles.PlayerController.Player);

            DissasterCastedText.gameObject.SetActive(Singles.PlayerController.Player.IsDisasterCasted);
            BiomeActionDoneText.gameObject.SetActive(Singles.PlayerController.SelectedBiome.IsActionDone);
        }
        else
        {
            GrowButton.interactable = false;
            MigrateButton.interactable = false;
            RiseGroundButton.interactable = false;
            LowerGroundButton.interactable = false;
            IceMeteorButton.interactable = false;
            RockMeteorButton.interactable = false;
            NewGroundsButton.interactable = false;
        }
    }
}
