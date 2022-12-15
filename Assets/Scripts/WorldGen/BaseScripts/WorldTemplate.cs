using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
[CreateAssetMenu(fileName = "New World", menuName = "WorldGen/World")]
public class WorldTemplate : ScriptableObject
{
    [SerializeField] NoiseMap moistureMap;
    [SerializeField] NoiseMap heatMap;
    [SerializeField] NoiseMap heightMap;
    [SerializeField] TileGroup[] tileGroups;
    public void PlaceTile(Chunk chunk, Vector2Int position)
    {
        SelectTile(chunk, position).Place(chunk, position);
    }
    Placement SelectTile(Chunk chunk, Vector2Int position)
    {
        float moistureValue = chunk.CurrentWorld().moistureMap.GetValue(position);
        float heatValue = chunk.CurrentWorld().heatMap.GetValue(position);
        float heightValue = chunk.CurrentWorld().heightMap.GetValue(position);
        for (int i = 0; i < tileGroups.Length; i++)
        {
            if (tileGroups[i].WithinRange(heatValue, moistureValue))
            {
                return tileGroups[i].GetTile(heatValue, moistureValue, heightValue);
            }
        }
        return tileGroups[tileGroups.Length - 1].GetLastTile();
    }
    public void GenerateSeeds()
    {
        moistureMap.GenerateRandomSeed();
        heightMap.GenerateRandomSeed();
        heatMap.GenerateRandomSeed();
    }

    [System.Serializable]
    public struct TileGroup
    {
        [SerializeField] RangeF heatValue;
        [SerializeField] RangeF moistureValue;
        [SerializeField] Placement[] tiles;

        public bool WithinRange(float heat, float moisture)
        {
            return heatValue.WithinRange(heat) && moistureValue.WithinRange(moisture);
        }

        public Placement GetTile(float heat, float moisture, float height)
        {
            for (int i = 0; i < tiles.Length; i++)
            {
                if (tiles[i].CanPlace(heat, moisture, height))
                {
                    return tiles[i];
                }
            }
            return GetLastTile();
        }
        public Placement GetLastTile()
        {
            return tiles[tiles.Length - 1];
        }
    }
}


