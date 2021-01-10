using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerAction : NetworkBehaviour
{
    public void Start()
    {
        DontDestroyOnLoad(this);
        var playerController = GetComponent<PlayerController>();
        if (playerController.hasAuthority)
        {
            Singles.PlayerAction = this;
        }
    }

    [Command]
    public void CmdEndTurn(int playerId)
    {
        var player = Singles.Cache.GetPlayer(playerId);

        if (CanEndTurn(player))
            EndTurnService.EndTurn();
    }

    public bool CanEndTurn(Player player)
    {
        return CanPerformAnyBaseAction(player);
    }

    private bool CanPerformAnyBaseAction(Player player)
    {
        if (Singles.World.PlayersTurn != player) return false;
        if (Singles.World.ActionInProgress) return false;
        return true;
    }

    private bool CanPerformAnyBaseAction(Biome biome, Player player)
    {
        if (!CanPerformAnyBaseAction(player)) return false;
        if (biome.IsNull()) return false;
        if (biome.IsAbyss) return false;
        if (!biome.IsOwner(player) && !Singles.World.IsSandbox) return false;
        return true;
    }

    [Command]
    public void CmdRise(XYZKey biomeKey, int playerId)
    {
        var player = Singles.Cache.GetPlayer(playerId);
        var biome = Singles.Grid.Get(biomeKey);

        var cost = player.GetManaCost();
        if (!CanRise(biome, player)) return;
        if (player.UseMana(cost))
        {
            biome.Rise();
            biome.IsActionDone = true && !Singles.World.IsSandbox;
            if (!player.IsAi) RpcRise(biomeKey, playerId);
        }
    }

    [ClientRpc]
    private void RpcRise(XYZKey biomeKey, int playerId)
    {
        var player = Singles.Cache.GetPlayer(playerId);
        var biome = Singles.Grid.Get(biomeKey);

        biome.PlayAudioClip(Singles.Audio.Earthquake);
    }

    public bool CanRise(Biome biome, Player player)
    {
        if (!player.IsEnoughMana(player.GetManaCost())) return false;
        if (!CanPerformAnyBaseAction(biome, player)) return false;
        if (biome.Crust.Height >= BiomeService.MaxHeight) return false;
        if (biome.CityState.IsNotable) return false;
        if (biome.IsActionDone) return false;
        return true;
    }

    public bool CanLower(Biome biome, Player player)
    {
        if (!player.IsEnoughMana(player.GetManaCost())) return false;
        if (biome.IsWater) return false;
        if (!CanPerformAnyBaseAction(biome, player)) return false;
        if (biome.CityState.IsNotable) return false;
        if (biome.IsActionDone) return false;
        return true;
    }

    [Command]
    public void CmdLower(XYZKey biomeKey, int playerId)
    {
        var player = Singles.Cache.GetPlayer(playerId);
        var biome = Singles.Grid.Get(biomeKey);

        var cost = player.GetManaCost();
        if (biome.IsNull()) return;
        if (!CanLower(biome, player)) return;
        if (player.UseMana(cost))
        {
            biome.Lower();
            biome.IsActionDone = true && !Singles.World.IsSandbox;
            if(biome.IsWater) BiomeService.TryCaptureNearby(biome, player, false);
            if(!player.IsAi) RpcLower(biomeKey, playerId);
        }
    }

    [ClientRpc]
    private void RpcLower(XYZKey biomeKey, int playerId)
    {
        var player = Singles.Cache.GetPlayer(playerId);
        var biome = Singles.Grid.Get(biomeKey);

        biome.PlayAudioClip(Singles.Audio.Earthquake);
    }

    public bool CanPlantGrass(Biome biome, Player player)
    {
        if (!player.IsEnoughMana(player.GetManaCost())) return false;
        if (!CanPerformAnyBaseAction(biome, player)) return false;
        if (biome.Spec.IsDead) return false;
        if (biome.IsWater) return false;
        if (biome.IsActionDone) return false;
        return true;
    }

    [Command]
    public void CmdPlantGrass(XYZKey biomeKey, int playerId)
    {
        var player = Singles.Cache.GetPlayer(playerId);
        var biome = Singles.Grid.Get(biomeKey);

        if (!CanPlantGrass(biome, player)) return;
        var cost = player.GetManaCost();
        if (player.UseMana(cost))
        {
            biome.PlantGrass(player, 1f);
            biome.Nature.Maturity += 1f;
            biome.IsActionDone = true && !Singles.World.IsSandbox;
            BiomeService.TryCaptureNearby(biome, player, false);
            if (!player.IsAi) RpcPlantGrass(biomeKey, playerId);
        }
    }

    [ClientRpc]
    private void RpcPlantGrass(XYZKey biomeKey, int playerId)
    {
        var player = Singles.Cache.GetPlayer(playerId);
        var biome = Singles.Grid.Get(biomeKey);

        biome.PlayAudioClip(Singles.Audio.GrowthCracking);
    }

    public bool CanPerformDisaster(Biome biome, Player player)
    {
        if (!player.IsEnoughMana(player.GetDisasterManaCost())) return false;
        if (!CanPerformAnyBaseAction(biome, player)) return false;
        if (player.IsDisasterCasted) return false;
        return true;
    }

    [Command]
    public void CmdDropIceMeteor(XYZKey biomeKey, int playerId)
    {
        var player = Singles.Cache.GetPlayer(playerId);
        var biome = Singles.Grid.Get(biomeKey);

        if (!CanPerformDisaster(biome, player)) return;
        var cost = player.GetDisasterManaCost();
        if (player.UseMana(cost))
        {
            RpcDropIceMeteor(biomeKey, playerId);
            ServerDropIceMeteor(biome, player);
        }
    }

    /// <summary>
    /// Prevents some syncing problems. ClientRpc might be called a bit later even if it is host client.
    /// </summary>
    private void ServerDropIceMeteor(Biome biome, Player player)
    {
        Singles.World.ActionInProgress = true;

        Action meteorImpact = () => DoIceMeteorEffect(biome, player);
        biome.PlayMeteorAnimation(meteorImpact);
    }

    private void DoIceMeteorEffect(Biome biome, Player player)
    {
        if (NetworkService.IsServer())
        {
            Singles.World.ActionInProgress = false;
            var biomes = biome.GetNearbyBiomesCache();
            biome.IsWater = true;
            BiomeService.TryCaptureNearby(biome, player, false);
            foreach (var near in biomes)
            {
                near.Lower(UnityEngine.Random.Range(1, 2));
                if (near.IsWater) BiomeService.TryCaptureNearby(near, player, false);
                near.KillPopulation();
            }
            player.IsDisasterCasted = true && !Singles.World.IsSandbox;
        }

        biome.PlayAudioClip(Singles.Audio.MeteorExplosion);
    }

    [ClientRpc]
    private void RpcDropIceMeteor(XYZKey biomeKey, int playerId)
    {
        if (NetworkServer.active) return;
        Singles.World.ActionInProgress = true;

        var player = Singles.Cache.GetPlayer(playerId);
        var biome = Singles.Grid.Get(biomeKey);

        Action meteorImpact = () => biome.PlayAudioClip(Singles.Audio.MeteorExplosion);
        biome.PlayMeteorAnimation(meteorImpact);
    }

    [Command]
    public void CmdDropRockMeteor(XYZKey biomeKey, int playerId)
    {
        var player = Singles.Cache.GetPlayer(playerId);
        var biome = Singles.Grid.Get(biomeKey);

        if (!CanPerformDisaster(biome, player)) return;
        var cost = player.GetDisasterManaCost();
        if (player.UseMana(cost))
        {
            RpcDropRockMeteor(biomeKey, playerId);
            ServerDropRockMeteor(biome, player);
        }
    }

    /// <summary>
    /// Prevents some syncing problems. ClientRpc might be called a bit later even if it is host client.
    /// </summary>
    private void ServerDropRockMeteor(Biome biome, Player player)
    {
        Singles.World.ActionInProgress = true;

        Action meteorImpact = () => DoRockMeteorEffect(biome, player);
        biome.PlayMeteorAnimation(meteorImpact);
    }

    private void DoRockMeteorEffect(Biome biome, Player player)
    {
        if (NetworkService.IsServer())
        {
            Singles.World.ActionInProgress = false;
            var biomes = biome.GetNearbyBiomesCache().ToList();
            biomes.Remove(biome);
            biomes = biomes.PickRandom(3).ToList();
            biomes.Add(biome);
            biome.SetAbyss(true);
            foreach (var near in biomes)
            {
                near.Lower(UnityEngine.Random.Range(1, 2));
                if (near.IsWater) near.SetAbyss(true);
                near.KillPopulation();
            }
            player.IsDisasterCasted = true && !Singles.World.IsSandbox;
        }

        biome.PlayAudioClip(Singles.Audio.MeteorExplosion);
    }

    [ClientRpc]
    private void RpcDropRockMeteor(XYZKey biomeKey, int playerId)
    {
        if (NetworkServer.active) return;
        Singles.World.ActionInProgress = true;

        var player = Singles.Cache.GetPlayer(playerId);
        var biome = Singles.Grid.Get(biomeKey);

        Action meteorImpact = () => biome.PlayAudioClip(Singles.Audio.MeteorExplosion);
        biome.PlayMeteorAnimation(meteorImpact);
    }

    public bool CanCreateNewGround(Biome biome, Player player)
    {
        if (!CanPerformAnyBaseAction(player)) return false;
        if (!player.IsEnoughMana(player.GetMidManaCost())) return false;
        if (biome.IsNull()) return false;
        if (!biome.IsAbyss) return false;
        if (!biome.GetNearbyBiomesCache().Any(v => v.Owner == player) && !Singles.World.IsSandbox) return false;
        return true;
    }

    [Command]
    public void CmdCreateNewGround(XYZKey biomeKey, int playerId)
    {
        var player = Singles.Cache.GetPlayer(playerId);
        var biome = Singles.Grid.Get(biomeKey);

        if (!CanCreateNewGround(biome, player)) return;
        var cost = player.GetMidManaCost();
        if (player.UseMana(cost))
        {
            biome.SetAbyss(false);
            biome.SetOwner(player);
            CityStateService.CreateNew(biome);
            RpcCreateNewGround(biomeKey, playerId);
        }
    }

    [ClientRpc]
    private void RpcCreateNewGround(XYZKey biomeKey, int playerId)
    {
        var player = Singles.Cache.GetPlayer(playerId);
        var biome = Singles.Grid.Get(biomeKey);

        biome.PlayAudioClip(Singles.Audio.Earthquake);
    }

    public bool CanMigrateAncients(Player player, Biome biomeFrom)
    {
        if (!player.IsEnoughMana(player.GetManaCost())) return false;
        if (biomeFrom.IsActionDone) return false;
        return CanPerformAnyBaseAction(biomeFrom, player)
            && biomeFrom.Nature.IsAncient;
    }

    public bool CanMigrateAncients(Player player, Biome biomeFrom, Biome biomeTo)
    {
        if (!biomeFrom.GetNearbyBiomesCache().Any(near => near == biomeTo)) return false;
        if (biomeFrom.Spec != biomeTo.Spec) return false;
        if (biomeTo.Nature.IsAncient) return false;
        //TODO Mass call of this method can be slow because of mana cost check
        if (!CanMigrateAncients(player, biomeFrom)) return false;
        return true;
    }

    [Command]
    public void CmdMigrateAncients(int playerId, XYZKey biomeFromKey, XYZKey biomeToKey)
    {
        var player = Singles.Cache.GetPlayer(playerId);
        var biomeFrom = Singles.Grid.Get(biomeFromKey);
        var biomeTo = Singles.Grid.Get(biomeToKey);

        if (!CanMigrateAncients(player, biomeFrom, biomeTo)) return;
        var cost = player.GetManaCost();
        if (player.UseMana(cost))
        {
            biomeTo.Nature.Maturity += BiomeConstant.AncientMax;
            biomeTo.SetOwner(player);
            biomeTo.IsActionDone = true && !Singles.World.IsSandbox;
            biomeFrom.IsActionDone = true && !Singles.World.IsSandbox;

            RpcMigrateAncients(playerId, biomeFromKey, biomeToKey);
        }
    }

    [ClientRpc]
    private void RpcMigrateAncients(int playerId, XYZKey biomeFromKey, XYZKey biomeToKey)
    {
        var player = Singles.Cache.GetPlayer(playerId);
        var biomeFrom = Singles.Grid.Get(biomeFromKey);
        var biomeTo = Singles.Grid.Get(biomeToKey);

        Singles.PlayerController.AncientMigrateChoose = false;
        Singles.PlayerController.AncientMigrateToBiome = null;
        biomeTo.PlayAudioClip(Singles.Audio.GrowthCracking);
    }

    public void StartChooseMigrateAncients(Player player, Biome biomeFrom)
    {
        if (!CanMigrateAncients(player, biomeFrom)) return;
        Singles.PlayerController.AncientMigrateChoose = true;
    }

    [Command]
    public void CmdSetName(int playerControllerId, string name)
    {
        var playerParam = FindObjectsOfType<PlayerParamModel>()
            .FirstOrDefault(model => model.PlayerControllerId == playerControllerId);

        if (playerParam == null) return;

        playerParam.Name = name;
    }
}

