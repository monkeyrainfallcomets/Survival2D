using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;
using System.Threading;
public class WorldGeneration : MonoBehaviour
{
    [SerializeField] WorldInstance chunkPrefab;
    [SerializeField] WorldTemplate[] worlds;
    [SerializeField] int requiredSpawnPoints;
    [SerializeField] WorldGenData worldGenData;
    [SerializeField] Transform grid;
    [SerializeField] TileBase tile;
    Vector3 playerPosition;
    WorldTemplate world;
    WorldInstance worldInstance;

    public void Start()
    {
        GenerateWorld(917595710, worlds[UnityEngine.Random.Range(0, worlds.Length)], worldGenData.player);

    }

    public void GenerateWorld(int seed, WorldTemplate world, Entity player)
    {
        //creating worldInstance
        worldInstance = Instantiate(chunkPrefab);
        worldInstance.transform.SetParent(grid);
        worldGenData.worldTemplate = world;
        worldGenData.seed = seed;
        worldInstance.Initialize(worldGenData);
        //handling world template
        this.world = world;
        world.GenerateSeeds();
        //creating spawn locations
        List<Vector2Int> spawnLocations = new List<Vector2Int>();
        Dictionary<Vector2Int, WorldTile> tiles = new Dictionary<Vector2Int, WorldTile>();
        void GenerateTile(Vector2Int position)
        {
            WorldTile tile = worldInstance.GenerateWorldTile(position);
            if (tile.Traversable(player))
            {
                spawnLocations.Add(position);
            }
            tiles[position] = tile;
        }
        int index = 1;
        GenerateTile(new Vector2Int(0, 0));
        while (spawnLocations.Count < requiredSpawnPoints)
        {
            for (int i = 0; i <= index; i++)
            {
                GenerateTile(new Vector2Int(index, i));
                GenerateTile(new Vector2Int(i, index));
            }
            index++;
        }
        List<WorldTile> worldTiles = new List<WorldTile>();
        Vector2Int spawnLocation = spawnLocations[UnityEngine.Random.Range(0, spawnLocations.Count)];
        WorldTile tile;
        Vector2Int position;
        for (int y = spawnLocation.y - worldGenData.viewDistance.y; y <= spawnLocation.y + worldGenData.viewDistance.y; y++)
        {
            for (int x = spawnLocation.x - worldGenData.viewDistance.x; x <= spawnLocation.x + worldGenData.viewDistance.x; x++)
            {
                position = new Vector2Int(x, y);
                if (!tiles.TryGetValue(position, out tile))
                {
                    tile = worldInstance.GenerateWorldTile(position);
                }
                worldTiles.Add(tile);
            }
        }
        worldInstance.GenerateWorld(worldTiles, spawnLocation);
    }

    public void EndGeneration()
    {
        worldInstance.End();
    }

    public Planet CurrentPlanet()
    {
        return worldGenData.worldTemplate.GetPlanet();
    }

}

[System.Serializable]
public struct WorldGenData
{
    public Entity player;
    public Vector2Int viewDistance;
    [HideInInspector] public int seed;
    [HideInInspector] public WorldTemplate worldTemplate;

    public WorldGenData(Entity player, Vector2Int viewDistance, WorldTemplate worldTemplate, int seed)
    {
        this.player = player;
        this.viewDistance = viewDistance;
        this.worldTemplate = worldTemplate;
        this.seed = seed;
    }
}

