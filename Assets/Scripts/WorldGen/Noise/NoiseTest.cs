using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class NoiseTest : MonoBehaviour
{
    [SerializeField] Vector2Int size;
    [SerializeField] Vector2 scale;
    [SerializeField] int seed;
    [SerializeField] float lacunarity;
    [SerializeField] float persistancy;
    [SerializeField] int octaves;


    void Start()
    {
        Texture2D texture = new Texture2D(size.x, size.y);
        Color[] pixels = new Color[size.x * size.y];
        for (int y = 0; y < size.x; y++)
        {
            for (int x = 0; x < size.y; x++)
            {
                float amplitude = 1;
                float frequency = 1;
                float noiseValue = 0;
                for (int i = 0; i < octaves; i++)
                {
                    float sampleX = (x + seed) / scale.x * frequency;
                    float sampleY = (y + seed) / scale.x * frequency;
                    float octaveNoiseValue = Mathf.PerlinNoise(sampleX, sampleY);
                    noiseValue += octaveNoiseValue * amplitude;
                    amplitude *= persistancy;
                    frequency *= lacunarity;
                }
                Color color = Color.Lerp(Color.white, Color.black, noiseValue);
                pixels[y * size.y + x] = color;
            }
        }
        texture.SetPixels(pixels);
        texture.Apply();
        GetComponent<RawImage>().texture = texture;
    }
}
