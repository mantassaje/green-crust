using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public static class PlayerService
{
    public static IEnumerable<Biome> GetPlayerBiomes(Player player)
    {
        return Singles.Grid.GetAllBiomes().Where(v => v.IsOwner(player));
    }

    public static int GetTotalManaGain(Player player)
    {
        var biomes = Singles.Grid.GetAllBiomes()
            .Where(v => v.IsOwner(player));
        var newMana = biomes.Sum(v => v.GetManaOutput());
        return newMana + 1;
    }

    /// <summary>
    /// Will give random pos
    /// </summary>
    /// <param name="player"></param>
    public static void SetRandomStartingPosition(Player player)
    {
        var biome = Singles.Grid.GetAllBiomes()
            .Where(b => b.Weather.HeatType == HeatTypes.Warm 
                && !Singles.Grid.GetAllNearby(b, 5)
                .Any(near => BadForStartingPosition(near)))
            .ToList()
            .PickRandom(1)
            .FirstOrDefault();
        if (biome == null)
        {
            Debug.Log("Player not added. No space found");
            return;
        }
        BalanceStartPos(biome);
        SetOwnerInRadius(biome, player, 1);
        player.LastCamPos = biome.transform.position;
    }

    public static void SetStartingPosition(Player player, Biome biome)
    {
        BalanceStartPos(biome);
        SetOwnerInRadius(biome, player, 1);
        player.LastCamPos = biome.transform.position;
    }

    /// <summary>
    /// Set owner without any capture logic. Used for player setup
    /// </summary>
    public static void SetOwnerInRadius(Biome biome, Player player, int radius)
    {
        var nears = Singles.Grid.GetAllNearby(biome, radius);
        foreach (var near in nears)
        {
            near.SetOwner(player);
        }
    }

    private static bool BadForStartingPosition(Biome biome)
    {
        return biome.Owner != null;
    }

    private static void BalanceStartPos(Biome biome)
    {
        var nearBiomes = Singles.Grid.GetAllNearby(biome, 2);
        foreach(var near in nearBiomes)
        {
            if (near.Crust.Height >= BiomeService.MinMountainHeight)
            {
                near.Crust.Height = BiomeService.MinMountainHeight - 1;
            }
            if (near.IsAbyss)
            {
                near.SetAbyss(false);
            }
        }
    }

    public static void AddEndTurnMana(Player player)
    {
        var addMana = PlayerService.GetTotalManaGain(player);
        player.TotalManaCollected += addMana;
        addMana = addMana.GetMin(0);
        player.AddMana(addMana);
    }

    public static void ResetPlayerForEndTurn(Player player)
    {
        player.LastCamPos = Singles.CameraControlls.transform.position;
        Singles.PlayerController.SetSelectedBiome(null);
        player.IsDisasterCasted = false;
    }

    public static Player GetVacant()
    {
        var usedPlayerIds = GameObject.FindObjectsOfType<PlayerController>()
            .Where(controller => controller.Player != null)
            .Select(controller => controller.Player.Id).ToList();
        return Singles.Cache.GetPlayers()
            .OrderBy(player => player.Id)
            .First(player => !player.IsAi && !usedPlayerIds.Contains(player.Id));
    }
}
