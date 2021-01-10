using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class Loader : MonoBehaviour
{
    public GameObject NetworkSinglesPrefab;

    public void Start()
    {
        if (NetworkService.IsServer())
        {
            var networkSingles = Instantiate(NetworkSinglesPrefab);
            NetworkServer.Spawn(networkSingles);
            Singles.World.IsOnlneMode = Singles.WorldStartParamModel.IsOnlneMode;
            Singles.World.IsSandbox = Singles.WorldStartParamModel.IsSandbox;

            //Players
            Singles.World.Players = new List<Player>();

            for (int i = 0; i < Singles.WorldStartParamModel.Players.Count; i++)
            {
                var playerParam = Singles.WorldStartParamModel.Players[i];

                var player = Instantiate(Singles.Templates.Player);
                player.Id = i + 1;
                NetworkServer.Spawn(player.gameObject);
                player.gameObject.SetActive(true);

                player.Name = string.IsNullOrEmpty(playerParam.Name) ? "Player " + (i + 1) : playerParam.Name;
                player.IsAi = playerParam.IsAi;
                player.Color = Singles.Colors.PlayerColors[i];

                Singles.World.Players.Add(player);

                AssignPlayerToController(player, playerParam);
            }

            //World
            int size;
            if (Singles.World.IsSandbox)
            {
                size = 8;
                Singles.Grid.Size = 45;
                Singles.Grid.Load();
                CircleWorldGenerator.Generate();

                //HACK This depends on order at witch biomes are created.
                var allBiomes = Singles.Grid.GetAllBiomes().ToList();
                var centerBiome = allBiomes[allBiomes.Count / 2];
                Singles.World.Players.ForEach(player => player.LastCamPos = centerBiome.transform.position);
            }
            else
            {
                Singles.Grid.Size = 25 + Singles.World.Players.Count * 5;
                size = 4;
                Singles.Grid.Load();
                BalancedWorldGenerator.Generate(size);
            }
            
            
            EndTurnService.SetPlayerTurn(Singles.World.Players.First());

            Singles.World.IsLoaded = true;
            ClientRpcAction.Singleton.RpcWorldLoaded();
        }
    }

    private void AssignPlayerToController(Player player, PlayerParamModel playerParam)
    {
        var playerController = FindObjectsOfType<PlayerController>().FirstOrDefault(controller => controller.Id == playerParam.PlayerControllerId);
        if (playerController != null)
        {
            playerController.Player = player;
        }
    }
}
