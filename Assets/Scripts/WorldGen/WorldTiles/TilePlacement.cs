using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public class TilePlacement : Placement
{
    [SerializeField] TileBase tile;
    public override void Place(Chunk chunk, Vector2Int position)
    {
        base.Place(chunk, position);
    }

    public TileBase GetTile()
    {
        return tile;
    }
}
