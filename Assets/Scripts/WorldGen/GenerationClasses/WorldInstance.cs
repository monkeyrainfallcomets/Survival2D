using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class WorldInstance : MonoBehaviour
{
    [SerializeField] SerializableDictionary<Map, Tilemap> tilemaps = new SerializableDictionary<Map, Tilemap>();
    WorldGenData genData;


    public void Initialize(WorldGenData genData)
    {
        this.genData = genData;
    }

    public void End()
    {

    }

    public void PlaceTile(Map map, TileBase tile, Vector3Int position)
    {
        tilemaps[map].SetTile(position, tile);
    }

    public WorldTile GenerateWorldTile(Vector2Int position)
    {
        NoiseValue noiseValues = genData.worldTemplate.GetNoiseValues(position);
        TilePlacement tile = genData.worldTemplate.SelectTile(position, noiseValues);
        tile.Place(this, position);
        Placement detail;
        if (tile.SelectDetail(position, out detail, noiseValues))
        {
            return new WorldTile(position, noiseValues, tile, detail);
        }
        return new WorldTile(position, noiseValues, tile);
    }

    public Tilemap GetMap(Map map)
    {
        return tilemaps[map];
    }

    public enum Map
    {
        Detail,
        Base,
        Collision
    }
}
