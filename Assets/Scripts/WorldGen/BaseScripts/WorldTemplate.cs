using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New World", menuName = "WorldGen/World")]
public class WorldTemplate : ScriptableObject
{
    public NoiseMap biomeMap { get { return biomeMap; } set { biomeMap = value; } }
    public NoiseMap tileNoiseMap { get { return tileNoiseMap; } set { tileNoiseMap = value; } }
    BiomeParams[] biomeParams;
    public void PlaceTile(Chunk chunk, Vector2Int position)
    {
        float biomeNoiseValue = chunk.world.biomeMap.GetValue(position);
        float tileNoiseValue = chunk.world.tileNoiseMap.GetValue(position);
        for (int i = 0; i < biomeParams.Length; i++)
        {
            if (i == biomeParams.Length - 1)
            {
                biomeParams[i].PlaceTile(chunk, position, tileNoiseValue);
            }
            if (biomeParams[i].PlaceTile(chunk, position, biomeNoiseValue, tileNoiseValue))
            {
                return;
            }
        }
    }
}
