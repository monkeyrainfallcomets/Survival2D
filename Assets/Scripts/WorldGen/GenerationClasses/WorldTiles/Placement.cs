using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
[System.Serializable]
public class Placement
{
    [SerializeField] RangeF heatValue;
    [SerializeField] RangeF moistureValue;
    [SerializeField] RangeF heightValue;
    public bool CanPlace(float heat, float moisture, float height)
    {
        return heatValue.WithinRange(heat) && moistureValue.WithinRange(moisture) && heightValue.WithinRange(height);
    }
    public bool TryPlacing(float heat, float moisture, float height, WorldInstance chunk, Vector2Int position)
    {
        if (CanPlace(heat, moisture, height))
        {
            Place(chunk, position);
            return true;
        }
        return false;
    }
    public virtual void Place(WorldInstance chunk, Vector2Int position)
    {
    }
}

[System.Serializable]
public class TileGroup : PlacementGroup<TilePlacement>
{

}
[System.Serializable]
public class RandomPlacementGroup<PlacementType> where PlacementType : Placement
{
    [Range(0f, 1f)]
    [SerializeField] double chance;
    [SerializeField] PlacementGroup<PlacementType>[] placements;
    public bool TrySelectPlacement(System.Random random, out PlacementType placement, float heat, float moisture, float height)
    {
        if (random.NextDouble() < chance)
        {
            for (int i = 0; i < placements.Length; i++)
            {
                if (placements[i].WithinRange(heat, moisture))
                {
                    placement = placements[i].GetPlacement(heat, moisture, height);
                    return true;
                }
            }
            placement = GetLastPlacement();
        }
        placement = null;
        return false;
    }
    public PlacementType GetLastPlacement()
    {
        return placements[placements.Length - 1].GetLastPlacement();
    }
}

public class PlacementGroup<PlacementType> where PlacementType : Placement
{
    [SerializeField] RangeF heatValue;
    [SerializeField] RangeF moistureValue;
    [SerializeField] PlacementType[] placements;
    public virtual bool WithinRange(float heat, float moisture)
    {
        return heatValue.WithinRange(heat) && moistureValue.WithinRange(moisture);
    }

    public PlacementType GetPlacement(float heat, float moisture, float height)
    {
        for (int i = 0; i < placements.Length; i++)
        {
            if (placements[i].CanPlace(heat, moisture, height))
            {
                return placements[i];
            }
        }
        return GetLastPlacement();
    }
    public PlacementType GetLastPlacement()
    {
        return placements[placements.Length - 1];
    }
}

public class PlacementInstance
{
    public PlacementInstance()
    {

    }
    public void Destroy()
    {

    }

}
