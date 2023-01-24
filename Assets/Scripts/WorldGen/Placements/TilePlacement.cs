using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "NewTile", menuName = "WorldGen/Tile")]
public class TilePlacement : ScriptableObject
{
    [SerializeField] GenTile baseTile;
    [SerializeField] RandomNoiseGroup<GenTile>[] tiles;
    [SerializeField] RandomNoiseGroup<Placement>[] details;
    [SerializeField] TileAdjustment tileAdjustments;
    [SerializeField] DamageTypes damageType;

    [Header("Entity's effectiveness must be less than this for it to be traversable by that entity")]
    [SerializeField] protected Effectiveness traversability;

    public TilePlacementInstance CreateInstance(WorldInstance world, Vector2Int position, NoiseValue noiseValues)
    {
        Tilemap tilemap = world.GetMap(WorldInstance.Map.Base);
        GenTile tile = GetTile(position, noiseValues);
        return new TilePlacementInstance(tilemap, (Vector3Int)position, tile);
    }

    public TilePlacementInstance CreateInstance(GenTile tile, WorldInstance world, Vector2Int position)
    {
        Tilemap tilemap = world.GetMap(WorldInstance.Map.Base);
        return new TilePlacementInstance(tilemap, (Vector3Int)position, tile);
    }

    public GenTile GetTile(Vector2Int position, NoiseValue noiseValues)
    {
        int valueAverage = (int)(((noiseValues.heightValue + noiseValues.heatValue + noiseValues.moistureValue) * 100) / 3);
        int randomSeed = (valueAverage * (position.x * position.y));
        System.Random random = new System.Random(randomSeed);
        double randomNum = random.NextDouble();
        GenTile tile;
        for (int i = 0; i < tiles.Length; i++)
        {
            if (tiles[i].TrySelectPlacement(randomNum, out tile, noiseValues))
            {
                return tile;
            }
        }
        return baseTile;
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

    public bool PostGenerationAdjustments(WorldInstance world, out TilePlacementInstance tileInstance, Vector3Int position)
    {
        TilePlacementInstance[] tiles = new TilePlacementInstance[4];
        world.GetTile(new Vector2Int(position.x - 1, position.y));
    }
    public bool IsTraversable(Entity entity)
    {
        if (entity.GetTypeMatchups().GetEffectiveness(damageType) >= traversability)
        {
            return false;
        }
        return true;
    }
    public GenTile GetBaseTile()
    {
        return baseTile;
    }
}

