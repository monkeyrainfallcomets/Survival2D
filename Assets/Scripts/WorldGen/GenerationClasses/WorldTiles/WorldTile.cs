using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
[System.Serializable]
public struct WorldTile
{
    public float moistureValue;
    public float heatValue;
    public float heightValue;
    public PlacementInstance detail;
    public TileBase tile;
    public Structure structure;
}
