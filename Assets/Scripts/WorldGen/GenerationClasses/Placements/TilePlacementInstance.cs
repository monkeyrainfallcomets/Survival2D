using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Tilemaps;
[System.Serializable]
public class TilePlacementInstance : PlacementInstance
{
    Vector3Int position;
    Tilemap tilemap;
    public TilePlacementInstance(Tilemap tilemap, Vector3Int position)
    {
        this.position = position;
        this.tilemap = tilemap;
    }

    public override void Destroy()
    {
        tilemap.SetTile(position, null);
    }
}
