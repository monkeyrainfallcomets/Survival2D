using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class NoiseTest : MonoBehaviour
{
    [SerializeField] Vector2Int size;
    [SerializeField] Vector2 scale;
    [SerializeField] int seed;

    void Start()
    {
        Texture2D texture = new Texture2D(size.x, size.y);
        Color[] pixels = new Color[size.x * size.y];
        for (int y = 0; y < size.x; y++)
        {
            for (int x = 0; x < size.y; x++)
            {
                Color color = Color.Lerp(Color.white, Color.black, GenerateNoise(x, y));
                Debug.Log(GenerateNoise(x, y));
                pixels[y * size.y + x] = color;
            }
        }
        texture.SetPixels(pixels);
        texture.Apply();
        GetComponent<RawImage>().texture = texture;
    }

    float GenerateNoise(int x, int y)
    {
        return Mathf.PerlinNoise((x + seed) / scale.x, (y + seed) / scale.y);
    }
}
