using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseMap : MonoBehaviour
{
    [SerializeField] Vector2 scale;
    [SerializeField] float lacunarity;
    [SerializeField] float persistancy;
    [SerializeField] int octaves;
    [SerializeField] int seed;
    public float GenerateRandomSeed()
    {
        seed = Random.Range(0, 999999999);
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
            float sampleY = (coordinates.y + seed) / scale.x * frequency;
            float octaveNoiseValue = Mathf.PerlinNoise(sampleX, sampleY);
            noiseValue += octaveNoiseValue * amplitude;
            amplitude *= persistancy;
            frequency *= lacunarity;
        }
        return noiseValue;
    }
}
