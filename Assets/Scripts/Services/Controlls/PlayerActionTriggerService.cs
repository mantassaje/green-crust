using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public static class PlayerActionTriggerService
{
    public static void CheckPresses(PlayerController inputs)
    {
        if (Singles.World.PlayersTurn.IsAi) return;

        if (inputs.SelectedBiome.IsNull()) inputs.AncientMigrateChoose = false;

        //Select biome or migrate
        if (Input.GetKeyDown(KeyCode.Mouse0)
            && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
        {
            if (inputs.AncientMigrateChoose
                && Singles.PlayerAction.CanMigrateAncients(Singles.PlayerController.Player, inputs.SelectedBiome, inputs.HoverBiome))
            {
                inputs.AncientMigrateToBiome = inputs.HoverBiome;
            }
            else
            {
                inputs.AncientMigrateChoose = false;
                inputs.SetSelectedBiome(inputs.HoverBiome);
            }
        }

        if (Input.GetKeyUp(KeyCode.W)) inputs.Rise = true;
        if (Input.GetKeyUp(KeyCode.S)) inputs.Lower = true;
        if (Input.GetKeyUp(KeyCode.E)) inputs.EndTurn = true;
        if (Input.GetKeyUp(KeyCode.A)) inputs.AncientMigrateChoose = true;
        if (Input.GetKeyUp(KeyCode.Space)) inputs.PlantGrass = true;
        if (Input.GetKeyUp(KeyCode.F1)) inputs.IceMeteor = true;
        if (Input.GetKeyUp(KeyCode.F2)) inputs.RockMeteor = true;
        if (Input.GetKeyUp(KeyCode.D)) inputs.NewGrounds = true;
        if (Input.GetKeyUp(KeyCode.T)) inputs.TestTrigger = true;

        if(Input.GetKeyUp(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Mouse1))
        {
            if (Singles.UiController.Menu.gameObject.activeSelf) Singles.UiController.Menu.gameObject.SetActive(false);
            else if (inputs.AncientMigrateChoose) inputs.AncientMigrateChoose = false;
            else if (inputs.SelectedBiome.IsNotNull()) inputs.SetSelectedBiome(null);
            else Singles.UiController.Menu.gameObject.SetActive(true);
        }
    }

    public static void ActTriggeredActions(PlayerController playerController)
    {
        if (Singles.World.PlayersTurn.IsAi) return;

        if (playerController.EndTurn) Singles.PlayerAction.CmdEndTurn(playerController.Player.Id);
        if (playerController.TestTrigger)
        {
            var debug = UnityEngine.Object.FindObjectOfType<BiomeDebugCard>();
            debug.ShowStats = !debug.ShowStats;
        }

        if (playerController.SelectedBiome.IsNull()) return;

        if (playerController.PlantGrass) Singles.PlayerAction.CmdPlantGrass(playerController.SelectedBiome.Key, playerController.Player.Id);
        if (playerController.Lower) Singles.PlayerAction.CmdLower(playerController.SelectedBiome.Key, playerController.Player.Id);
        if (playerController.Rise) Singles.PlayerAction.CmdRise(playerController.SelectedBiome.Key, playerController.Player.Id);
        
        if (playerController.IceMeteor) Singles.PlayerAction.CmdDropIceMeteor(playerController.SelectedBiome.Key, playerController.Player.Id);
        if (playerController.RockMeteor) Singles.PlayerAction.CmdDropRockMeteor(playerController.SelectedBiome.Key, playerController.Player.Id);
        if (playerController.NewGrounds) Singles.PlayerAction.CmdCreateNewGround(playerController.SelectedBiome.Key, playerController.Player.Id);
        if (playerController.AncientMigrateToBiome.IsNotNull()) Singles.PlayerAction.CmdMigrateAncients(
            playerController.Player.Id, 
            playerController.SelectedBiome.Key, 
            playerController.AncientMigrateToBiome.Key
        );
    }

    public static void Reset(PlayerController inputs)
    {
        inputs.PlantGrass = false;
        inputs.Lower = false;
        inputs.Rise = false;
        inputs.EndTurn = false;
        inputs.IceMeteor = false;
        inputs.RockMeteor = false;
        inputs.NewGrounds = false;
        inputs.TestTrigger = false;
        inputs.AncientMigrateToBiome = null;
    }

    public static void EndTurnReset(PlayerController inputs)
    {
        Reset(inputs);
        inputs.AncientMigrateChoose = false;
    }
}

