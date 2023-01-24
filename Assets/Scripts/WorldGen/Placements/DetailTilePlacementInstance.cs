using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class DetailTilePlacementInstance : PlacementInstance
{
    Vector3Int position;
    Tilemap tilemap;
    TileBase tileBase;
    public DetailTilePlacementInstance(Tilemap tilemap, Vector3Int position, TileBase tileBase)
    {
        this.position = position;
        this.tilemap = tilemap;
    }

    public override void Destroy(WorldInstance world)
    {
        tilemap.SetTile(position, null);
    }
    public override void Place(WorldInstance world)
    {
        tilemap.SetTile(position, tileBase);
    }
}
