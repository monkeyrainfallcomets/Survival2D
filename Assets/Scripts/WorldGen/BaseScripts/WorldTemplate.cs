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
    [SerializeField] SerializableDictionary<TileBase, RandomPlacementGroup<Placement>[]> detailGroups;

    public TilePlacement SelectTile(Vector2Int position)
    {
        float moistureValue = moistureMap.GetValue(position);
        float heatValue = heatMap.GetValue(position);
        float heightValue = heightMap.GetValue(position);
        for (int i = 0; i < tileGroups.Length; i++)
        {
            if (tileGroups[i].WithinRange(heatValue, moistureValue))
            {
                return tileGroups[i].GetPlacement(heatValue, moistureValue, heightValue);
            }
        }
        return tileGroups[tileGroups.Length - 1].GetLastPlacement();
    }

    public bool SelectDetail(Vector2Int position, TileBase tile, out Placement placement)
    {
        float moistureValue = moistureMap.GetValue(position);
        float heatValue = heatMap.GetValue(position);
        float heightValue = heightMap.GetValue(position);
        int valueAverage = (int)(((heightValue + heatValue + moistureValue) * 100) / 3);
        int randomSeed = valueAverage * (position.x * position.y);
        System.Random random = new System.Random(randomSeed);
        RandomPlacementGroup<Placement>[] details;

        if (detailGroups.TryGetValue(tile, out details))
        {
            for (int i = 0; i < details.Length; i++)
            {
                if (details[i].TrySelectPlacement(random, out placement, moistureValue, heatValue, heightValue))
                {
                    return true;
                }
            }
            placement = details[details.Length - 1].GetLastPlacement();
            return true;
        }
        placement = null;
        return false;
    }
    public void GenerateSeeds()
    {
        moistureMap.GenerateRandomSeed();
        heightMap.GenerateRandomSeed();
        heatMap.GenerateRandomSeed();
    }


}


