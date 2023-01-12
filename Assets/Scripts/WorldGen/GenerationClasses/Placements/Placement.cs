using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Placement : ScriptableObject
{
    public virtual PlacementInstance Place(WorldInstance world, Vector2Int position)
    {
        return new PlacementInstance();
    }
    public virtual bool IsTraversable(TileBase[] nonTraversable)
    {
        return true;
    }
}







