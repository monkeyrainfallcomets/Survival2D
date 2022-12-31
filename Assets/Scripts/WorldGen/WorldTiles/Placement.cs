using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
[System.Serializable]
public class Placement
{
    [SerializeField] WorldTile tile;
    [SerializeField] RangeF heatValue;
    [SerializeField] RangeF moistureValue;
    [SerializeField] RangeF heightValue;
    public bool CanPlace(float heat, float moisture, float height)
    {
        return heatValue.WithinRange(heat) && moistureValue.WithinRange(moisture) && heightValue.WithinRange(height);
    }
    public bool TryPlacing(float heat, float moisture, float height, Chunk chunk, Vector2Int position)
    {
        if (CanPlace(heat, moisture, height))
        {
            Place(chunk, position);
            return true;
        }
        return false;
    }
    public TileBase Place(Chunk chunk, Vector2Int position)
    {
        return tile.Place(chunk, position);
    }
}
