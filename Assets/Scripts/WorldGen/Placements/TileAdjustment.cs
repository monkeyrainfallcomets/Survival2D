using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class TileAdjustment
{
    [Header("length of 4 indexes 0:left,1:down,2:right,3:up")]
    [SerializeField] List<TileComparison> surroundingTiles = new List<TileComparison>(4);
    [SerializeField] GenTile resultingTile;
    public bool TryAdjustment(TilePlacementInstance[] tiles, out GenTile tile)
    {
        for (int i = 0; i < 4; i++)
        {
            if (!surroundingTiles[i].Contains(tiles[i]))
            {
                tile = null;
                return false;
            }
        }
        tile = resultingTile;
        return false;
    }
}


public class TileComparison
{
    [SerializeField] bool contains;
    [SerializeField] SerializableHashSet<GenTile> tiles;

    public bool Contains(TilePlacementInstance tileInstance)
    {
        bool result = tiles.Contains(tileInstance.GetTile());
        if (!contains)
        {
            result = !result;
        }
        return result;
    }
}
public class TileAdjustmentGroup
{
    [SerializeField] TileCondition[] conditions;
    [SerializeField] TileAdjustment[] tileAdjustments;
    public bool GetTile(out GenTile genTile, TilePlacementInstance[] tiles)
    {
        for (int i = 0; i < conditions.Length; i++)
        {
            if (!conditions[i].Pass(tiles))
            {
                genTile = null;
                return false;
            }
        }
        for (int i = 0; i < tileAdjustments.Length; i++)
        {
            if (tileAdjustments[i].TryAdjustment(tiles, out genTile))
            {
                return true;
            }
        }
        genTile = null;
        return false;
    }
}

[SerializeField]
public class TileCondition
{
    [SerializeField] SerializableHashSet<GenTile> allowedTiles;
    [SerializeField] int count;
    public bool Pass(TilePlacementInstance[] tiles)
    {
        int numCounted = 0;
        for (int i = 0; i < 4; i++)
        {
            if (allowedTiles.Contains(tiles[i].GetTile()))
            {
                numCounted++;
                if (numCounted >= count)
                {
                    return true;
                }
            }
        }
        return false;
    }
}