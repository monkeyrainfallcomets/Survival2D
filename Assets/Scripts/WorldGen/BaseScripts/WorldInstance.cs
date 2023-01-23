using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class WorldInstance : MonoBehaviour
{
    [SerializeField] SerializableDictionary<Map, Tilemap> tilemaps = new SerializableDictionary<Map, Tilemap>();
    Entity player;
    WorldGenData genData;
    Dictionary<Vector2Int, WorldTile> worldTiles = new Dictionary<Vector2Int, WorldTile>();

    public void Initialize(WorldGenData genData)
    {
        this.genData = genData;
    }

    public void End()
    {
        Destroy(gameObject);
    }

    public void GenerateWorld(List<WorldTile> tiles, Vector2Int spawnLocation)
    {
        for (int i = 0; i < tiles.Count; i++)
        {
            worldTiles.Add(tiles[i].GetPosition(), tiles[i]);
            tiles[i].Place(this);
        }
        Vector2Int startLocation = new Vector2Int(spawnLocation.x - genData.viewDistance.x, spawnLocation.y - genData.viewDistance.y);
        Vector2Int endLocation = new Vector2Int(spawnLocation.x + genData.viewDistance.x, spawnLocation.y + genData.viewDistance.y);
        AddTransitions(startLocation, endLocation);
    }

    public void PlaceTile(Map map, TileBase tile, Vector3Int position)
    {
        tilemaps[map].SetTile(position, tile);
    }

    public WorldTile GenerateWorldTile(Vector2Int position)
    {
        NoiseValue noiseValues = genData.worldTemplate.GetNoiseValues(position);
        TilePlacement tile = genData.worldTemplate.SelectTile(position, noiseValues);
        return new WorldTile(position, noiseValues, tile);
    }

    public Tilemap GetMap(Map map)
    {
        return tilemaps[map];
    }

    void PostGeneration(Vector2Int startLocation, Vector2Int endLocation)
    {
        Vector2Int position;
        WorldTile tile;
        for (int y = startLocation.y; y <= endLocation.y; y++)
        {
            for (int x = startLocation.x; x <= endLocation.x; x++)
            {
                position = new Vector2Int(0, 0);
                if (worldTiles.TryGetValue(position, out tile))
                {
                    tile.AddTransition(new Vector3Int[] { new Vector3Int(position.x + 1, position.y, 0),
                    new Vector3Int(position.x, position.y + 1, 0) }, this);
                }
            }
        }
    }
    public Planet GetPlanet()
    {
        return genData.worldTemplate.GetPlanet();
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
        Transition
    }
}

public enum Planet
{
    Earth
}
