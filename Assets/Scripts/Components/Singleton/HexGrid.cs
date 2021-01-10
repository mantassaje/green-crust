using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class HexGrid : NetworkBehaviour {

    [SyncVar]
    public int Size;
    
    private Dictionary<XYZKey, Biome> _biomes;

    public HexGrid()
    {
        Singles.Grid = this;
        _biomes = new Dictionary<XYZKey, Biome>();
    }

    private void Start()
    {
    }

    public void AddLoadedBiome(Biome biome)
    {
        _biomes.Add(biome.Key, biome);
    }

    public void Load()
    {
        CreateGrid();
    }

    private void CreateGrid()
    {
        if (!isServer) return;
        
        var hexPos = new XYZKey(0, 0, 0);
        var bounds = Singles.Templates.Biome.GetBounds();
        var sizeX = bounds.size.x - 0.03f;
        var sizeY = bounds.size.y - 0.03f;
        for (int x = 0; x < Size; x++)
        {
            var rowHexPos = hexPos;
            for (int y = 0; y < Size; y++)
            {
                //Init
                var biome = Instantiate(Singles.Templates.Biome);
                biome.gameObject.SetActive(true);

                //World real position
                float yOffset = x % 2 == 0 ? sizeY / 2f : 0;
                biome.transform.position = new Vector3(
                    x * sizeX / 4f * 3f,
                    (y * -sizeY) - yOffset,
                    Singles.Templates.Biome.transform.position.z);

                biome.Key = rowHexPos;
                _biomes.Add(rowHexPos, biome);
                NetworkServer.Spawn(biome.gameObject);

                //iterate row, go right
                rowHexPos = rowHexPos.GoBot();

            }

            //go right
            if (x % 2 == 0) hexPos = hexPos.GoTopRight();
            else hexPos = hexPos.GoBotRight();
        }
    }

    public Biome TryGet(XYZKey pos)
    {
        Biome biome;
        _biomes.TryGetValue(pos, out biome);
        return biome;
    }

    /// <summary>
    /// Returns all biomes around center including the center
    /// </summary>
    public IEnumerable<Biome> GetAllNearby(Biome center, int radius = 1)
    {
        for (int x = radius * -1; x <= radius; x++)
            for (int y = radius * -1; y <= radius; y++)
                for (int z = radius * -1; z <= radius; z++)
                {
                    var biome = TryGet(new XYZKey(x + center.Key.X, y + center.Key.Y, z + center.Key.Z));
                    if (biome != null) yield return biome;
                }
    }

    public Biome GetTop(Biome biome)
    {
        return TryGet(biome.Key.GoTop());
    }

    public Biome GetBot(Biome biome)
    {
        return TryGet(biome.Key.GoBot());
    }

    /// <summary>
    ///   /
    /// O 
    /// </summary>
    public Biome GetTopRight(Biome biome)
    {
        return TryGet(biome.Key.GoTopRight());
    }

    /// <summary>
    /// \
    ///   O 
    /// </summary>
    public Biome GetTopLeft(Biome biome)
    {
        return TryGet(biome.Key.GoTopLeft());
    }

    /// <summary>
    /// O 
    ///   \   
    /// </summary>
    public Biome GetBotRight(Biome biome)
    {
        return TryGet(biome.Key.GoBotRight());
    }

    /// <summary>
    ///  O 
    /// /    
    /// </summary>
    public Biome GetBotLeft(Biome biome)
    {
        return TryGet(biome.Key.GoBotLeft());
    }

    public IEnumerable<Biome> GetAllBiomes()
    {
        return _biomes.Select(v => v.Value);
    }

    public Biome Get(XYZKey biomeKey)
    {
        return GetAllBiomes().FirstOrDefault(biome => biome.Key == biomeKey);
    }

    public Biome GetCenterBiome()
    {
        int half = Singles.Grid.Size / 4;
        var center = new XYZKey(Singles.Grid.Size / 2, Singles.Grid.Size / -2 - half, half);
        return TryGet(center);
    }
}
