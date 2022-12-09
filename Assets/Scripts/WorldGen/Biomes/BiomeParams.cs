using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Biome", menuName = "WorldGen/Biome")]
public class BiomeParams : ScriptableObject
{
    [SerializeField] float noiseValue;
    [SerializeField] Placement[] placements;
    public bool PlaceTile(Chunk chunk, Vector2Int position, float biomeNoiseValue, float tileNoiseValue)
    {
        if (noiseValue >= tileNoiseValue)
        {
            PlaceTile(chunk, position, tileNoiseValue);
            return true;
        }
        return false;
    }
    public void PlaceTile(Chunk chunk, Vector2Int position, float tileNoiseValue)
    {
        for (int i = 0; i < placements.Length; i++)
        {
            if (i == placements.Length - 1)
            {
                placements[i].PlaceTile(chunk, position);
            }
            if (placements[i].PlaceTile(chunk, position, tileNoiseValue))
            {
                return;
            }
        }
    }
}

[System.Serializable]
public class Placement
{
    [SerializeField] float noiseValue;
    [SerializeField] WorldTile tile;
    public bool PlaceTile(Chunk chunk, Vector2Int position, float tileNoiseValue)
    {
        if (noiseValue >= tileNoiseValue)
        {
            PlaceTile(chunk, position);
            return true;
        }
        return false;
    }
    public void PlaceTile(Chunk chunk, Vector2Int position)
    {
        tile.Place(chunk, position);
    }
}

