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
    [SerializeField] SerializableDictionary<TileBase, DetailGroup[]> detailGroups;

    public TilePlacement SelectTile(Vector2Int position)
    {
        float moistureValue = moistureMap.GetValue(position);
        float heatValue = heatMap.GetValue(position);
        float heightValue = heightMap.GetValue(position);
        for (int i = 0; i < tileGroups.Length; i++)
        {
            if (tileGroups[i].WithinRange(heatValue, moistureValue))
            {
                return tileGroups[i].GetTile(heatValue, moistureValue, heightValue);
            }
        }
        return tileGroups[tileGroups.Length - 1].GetLastTile();
    }

    public Placement SelectDetail(Vector2Int position)
    {
        float moistureValue = moistureMap.GetValue(position);
        float heatValue = heatMap.GetValue(position);
        float heightValue = heightMap.GetValue(position);

    }
    public void GenerateSeeds()
    {
        moistureMap.GenerateRandomSeed();
        heightMap.GenerateRandomSeed();
        heatMap.GenerateRandomSeed();
    }

    [System.Serializable]
    public class TileGroup : PlacementGroup<TilePlacement>
    {

    }

    public class DetailGroup : PlacementGroup<Placement>
    {
        [SerializeField]
    }

    public class PlacementGroup<PlacementType> where PlacementType : Placement
    {
        [SerializeField] RangeF heatValue;
        [SerializeField] RangeF moistureValue;
        [SerializeField] PlacementType[] placements;
        public bool WithinRange(float heat, float moisture)
        {
            return heatValue.WithinRange(heat) && moistureValue.WithinRange(moisture);
        }

        public PlacementType GetTile(float heat, float moisture, float height)
        {
            for (int i = 0; i < placements.Length; i++)
            {
                if (placements[i].CanPlace(heat, moisture, height))
                {
                    return placements[i];
                }
            }
            return GetLastTile();
        }
        public PlacementType GetLastTile()
        {
            return placements[placements.Length - 1];
        }
    }
}


