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

    void AddTransitions(Vector2Int startLocation, Vector2Int endLocation)
    {
        Vector2Int topPosition;
        WorldTile top;
        Vector2Int rightPosition;
        WorldTile right;
        Vector2Int tilePosition;
        WorldTile tile;

        for (int y = startLocation.y; y <= endLocation.y; y++)
        {
            for (int x = startLocation.x; x <= endLocation.x; x++)
            {
                topPosition = new Vector2Int(x, y + 1);
                rightPosition = new Vector2Int(x + 1, y);
                tilePosition = new Vector2Int(x, y);
                if (worldTiles.TryGetValue(tilePosition, out tile))
                {
                    if (worldTiles.TryGetValue(rightPosition, out right))
                    {
                        tile.AddTransition((Vector3Int)rightPosition, right.GetTile(), this);
                    }
                    if (worldTiles.TryGetValue(topPosition, out top))
                    {
                        tile.AddTransition((Vector3Int)topPosition, top.GetTile(), this);
                    }
                }
            }
        }
    }

    public enum Map
    {
        Detail,
        Base,
        Collision,
        Transition
    }
}
