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
    [SerializeField] NoiseGroup<TilePlacement>[] tileGroups;

    public TilePlacement SelectTile(Vector2Int position, NoiseValue noiseValues)
    {
        for (int i = 0; i < tileGroups.Length; i++)
        {
            if (tileGroups[i].WithinRange(noiseValues))
            {
                return tileGroups[i].GetPlacement(noiseValues);
            }
        }
        return tileGroups[tileGroups.Length - 1].GetLastPlacement();
    }

    public TilePlacement SelectTile(Vector2Int position)
    {
        float moistureValue = moistureMap.GetValue(position);
        float heatValue = heatMap.GetValue(position);
        float heightValue = heightMap.GetValue(position);
        NoiseValue noiseValues = new NoiseValue(moistureValue, heatValue, heightValue);
        return SelectTile(position, noiseValues);
    }
    public NoiseValue GetNoiseValues(Vector2Int position)
    {
        float moistureValue = moistureMap.GetValue(position);
        float heatValue = heatMap.GetValue(position);
        float heightValue = heightMap.GetValue(position);
        return new NoiseValue(moistureValue, heatValue, heightValue);
    }
    public void GenerateSeeds()
    {
        moistureMap.GenerateRandomSeed();
        heightMap.GenerateRandomSeed();
        heatMap.GenerateRandomSeed();
    }
}


public struct NoiseValue
{
    public float moistureValue;
    public float heatValue;
    public float heightValue;

    public NoiseValue(float moistureValue, float heatValue, float heightValue)
    {
        this.moistureValue = moistureValue;
        this.heatValue = heatValue;
        this.heightValue = heightValue;
    }
}