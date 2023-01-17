using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class DetailTilePlacementInstance : PlacementInstance
{
    Vector3Int position;
    Tilemap tilemap;
    public DetailTilePlacementInstance(Tilemap tilemap, Vector3Int position)
    {
        this.position = position;
        this.tilemap = tilemap;
    }

    public override void Destroy(WorldInstance world)
    {
        tilemap.SetTile(position, null);
    }
}
