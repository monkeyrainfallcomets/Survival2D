using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
[System.Serializable]
public struct WorldTile
{
    public Placement detail;
    public TileBase tile;
    public Structure structure;
}
