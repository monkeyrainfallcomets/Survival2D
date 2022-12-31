using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "NewTile", menuName = "WorldGen/Tile")]
public class WorldTile : ScriptableObject
{
    [SerializeField] TileBase tile;
    public TileBase Place(Chunk chunk, Vector2Int position)
    {
        chunk.PlaceTile(tile, new Vector3Int(position.x, position.y, 0));
        return tile;
    }



}