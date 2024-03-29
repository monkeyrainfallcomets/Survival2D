using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New NoiseMap", menuName = "WorldGen/NoiseMap")]
public class NoiseMap : ScriptableObject
{
    [SerializeField] Vector2 scale;
    [SerializeField] float lacunarity;
    [SerializeField] float persistancy;
    [SerializeField] int octaves;
    [SerializeField] int seed;
    public int GenerateRandomSeed()
    {
        seed = Random.Range(0, 9999999);
        return seed;
    }
    public float GetValue(Vector2Int coordinates)
    {
        float amplitude = 1;
        float frequency = 1;
        float noiseValue = 0;
        for (int i = 0; i < octaves; i++)
        {
            float sampleX = (coordinates.x + seed) / scale.x * frequency;
            float sampleY = (coordinates.y + seed) / scale.y * frequency;
            float octaveNoiseValue = Mathf.PerlinNoise(sampleX, sampleY);
            noiseValue += octaveNoiseValue * amplitude;
            amplitude *= persistancy;
            frequency *= lacunarity;
        }
        return Mathf.Clamp(noiseValue, 0f, 1f);
    }
}
