using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "NewTile", menuName = "WorldGen/Tile")]
public class TilePlacement : ScriptableObject
{
    [SerializeField] RandomNoiseGroup<Placement>[] details;
    [SerializeField] RandomNoiseGroup<GenTile>[] tiles;
    [SerializeField] DamageTypes damageType;
    [Header("Entity's effectiveness must be less than this for it to be traversable by that entity")]
    [SerializeField] protected Effectiveness traversability;

    public TilePlacementInstance Place(WorldInstance world, Vector2Int position, NoiseValue noiseValues)
    {
        Tilemap tilemap = world.GetMap(WorldInstance.Map.Base);
        GenTile tile = GetTile(position, noiseValues);
        if (tile == null)
        {
            Debug.Log(noiseValues.heatValue + " " + noiseValues.moistureValue + " " + noiseValues.heightValue);
        }
        tilemap.SetTile((Vector3Int)position, tile.baseTile);
        return new TilePlacementInstance(tilemap, (Vector3Int)position, tile);
    }

    public TilePlacementInstance Place(GenTile tile, WorldInstance world, Vector2Int position)
    {
        Tilemap tilemap = world.GetMap(WorldInstance.Map.Base);
        tilemap.SetTile((Vector3Int)position, tile.baseTile);
        return new TilePlacementInstance(tilemap, (Vector3Int)position, tile);
    }

    public GenTile GetTile(Vector2Int position, NoiseValue noiseValues)
    {
        int valueAverage = (int)(((noiseValues.heightValue + noiseValues.heatValue + noiseValues.moistureValue) * 100) / 3);
        int randomSeed = (valueAverage * (position.x * position.y));
        System.Random random = new System.Random(randomSeed);
        double randomNum = random.NextDouble();
        Debug.Log(randomNum);
        GenTile tile;
        for (int i = 0; i < tiles.Length; i++)
        {
            if (tiles[i].TrySelectPlacement(randomNum, out tile, noiseValues))
            {
                return tile;
            }
        }
        return null;
    }

    public bool SelectDetail(Vector2Int position, out Placement placement, NoiseValue noiseValues)
    {
        if (details != null)
        {
            int valueAverage = (int)(((noiseValues.heightValue + noiseValues.heatValue + noiseValues.moistureValue) * 100) / 3);
            int randomSeed = valueAverage * (position.x * position.y);
            System.Random random = new System.Random(randomSeed);
            double randomNum = random.NextDouble();

            for (int i = 0; i < details.Length; i++)
            {
                if (details[i].TrySelectPlacement(randomNum, out placement, noiseValues))
                {
                    return true;
                }
            }
        }
        placement = null;
        return false;
    }
    public bool IsTraversable(Entity entity)
    {
        if (entity.GetTypeMatchups().GetEffectiveness(damageType) >= traversability)
        {
            return false;
        }
        return true;
    }


}
