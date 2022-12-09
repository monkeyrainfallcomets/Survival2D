using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
[System.Serializable]
public class WorldTile
{
    [SerializeField] TileBase tile;
    public virtual void Place(Chunk chunk, Vector2Int position)
    {
        chunk.PlaceTile(tile, new Vector3Int(position.x, position.y, 0));
    }
}
