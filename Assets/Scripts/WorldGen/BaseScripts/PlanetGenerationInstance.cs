using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class PlanetGenerationInstance : MonoBehaviour
{

    [SerializeField] SerializableDictionary<Map, Tilemap> tilemaps = new SerializableDictionary<Map, Tilemap>();
    Unit mainUnit;
    WorldGenSettings genData;
    int seed;
    Planet world;
    Dictionary<Vector2Int, WorldTile> worldTiles = new Dictionary<Vector2Int, WorldTile>();
    WorldGenSettings genSettings;

    public void End()
    {
        Destroy(gameObject);
    }

    public void GenerateWorld(int seed, Planet world, UnitLineUp units, WorldGenSettings genSettings)
    {
        this.genSettings = genSettings;
        this.seed = seed;
        this.world = world;
        world.GenerateSeeds();
        //creating spawn locations
        List<Vector2Int> spawnLocations = new List<Vector2Int>();
        Dictionary<Vector2Int, WorldTile> tiles = new Dictionary<Vector2Int, WorldTile>();
        void GenerateTile(Vector2Int position)
        {
            WorldTile tile = GenerateWorldTile(position);
            if (tile.Traversable(mainUnit))
            {
                spawnLocations.Add(position);
            }
            tiles[position] = tile;
        }
        int index = 1;
        GenerateTile(new Vector2Int(0, 0));
        while (spawnLocations.Count < genSettings.requiredSpawnPoints)
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
        Vector2Range viewBox = units.GetFieldOfVision(genSettings.viewDistance);
        for (int y = spawnLocation.y - genSettings.viewDistance.y - 1; y <= spawnLocation.y + genSettings.viewDistance.y + 1; y++)
        {
            for (int x = spawnLocation.x - genSettings.viewDistance.x - 1; x <= spawnLocation.x + genSettings.viewDistance.x + 1; x++)
            {
                position = new Vector2Int(x, y);
                if (!tiles.TryGetValue(position, out tile))
                {
                    tile = GenerateWorldTile(position);
                }
                worldTiles.Add(tile);
            }
        }
        DisplaySpawn(worldTiles, spawnLocation);
    }
    void DisplaySpawn(List<WorldTile> tiles, Vector2Int spawnLocation)
    {
        for (int i = 0; i < tiles.Count; i++)
        {
            worldTiles.Add(tiles[i].GetPosition(), tiles[i]);
        }
        Vector2Int startLocation = new Vector2Int(spawnLocation.x - genData.viewDistance.x, spawnLocation.y - genData.viewDistance.y);
        Vector2Int endLocation = new Vector2Int(spawnLocation.x + genData.viewDistance.x, spawnLocation.y + genData.viewDistance.y);
        Placement(startLocation, endLocation, new Vector2Int[] { Vector2Int.up, Vector2Int.right });
    }

    public void PlaceTile(Map map, TileBase tile, Vector3Int position)
    {
        tilemaps[map].SetTile(position, tile);
    }

    public WorldTile GenerateWorldTile(Vector2Int position)
    {
        NoiseValue noiseValues = world.GetNoiseValues(position);
        TilePlacement tile = world.SelectTile(position, noiseValues);
        return new WorldTile(position, noiseValues, tile, this);
    }

    public Tilemap GetMap(Map map)
    {
        return tilemaps[map];
    }

    void Placement(Vector2Int startLocation, Vector2Int endLocation, Vector2Int[] transitionDirections)
    {
        Vector2Int position;
        WorldTile tile;
        for (int y = startLocation.y; y <= endLocation.y; y++)
        {
            for (int x = startLocation.x; x <= endLocation.x; x++)
            {
                position = new Vector2Int(x, y);
                if (worldTiles.TryGetValue(position, out tile))
                {
                    tile.Place(this, transitionDirections);
                }
            }
        }
    }
    public PlanetType GetPlanet()
    {
        return world.GetPlanet();
    }

    public bool GetTile(Vector2Int position, out TilePlacementInstance tile)
    {
        WorldTile worldTile;
        if (worldTiles.TryGetValue(position, out worldTile))
        {
            tile = worldTile.GetTile();
            return true;
        }
        tile = null;
        return false;
    }

    public enum Map
    {
        Detail,
        Base,
        Collision,
        Transition,
        Extension
    }
}

public enum PlanetType
{
    Earth
}

public class WorldGenSettings
{
    public int requiredSpawnPoints;
    public Vector2Int viewDistance;
}