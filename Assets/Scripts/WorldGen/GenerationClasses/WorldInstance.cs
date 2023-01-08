using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class WorldInstance : MonoBehaviour
{
    [SerializeField] SerializableDictionary<Map, Tilemap> tilemaps = new SerializableDictionary<Map, Tilemap>();
    WorldGenData genData;


    public void Initialize(WorldTile[] tiles)
    {

    }

    public void End()
    {

    }

    public void PlaceTile(Map map, TileBase tile, Vector3Int position)
    {
        tilemaps[map].SetTile(position, tile);
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
