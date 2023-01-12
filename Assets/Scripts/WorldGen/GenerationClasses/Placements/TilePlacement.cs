using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "NewTile", menuName = "WorldGen/Placements/Tile")]
public class TilePlacement : Placement
{
    [SerializeField] RandomNoiseGroup<Placement> details;
    [SerializeField] RandomNoiseGroup<TileBase> tiles;
    [SerializeField] DamageTypes tileType;
    [SerializeField] WorldInstance.Map map;

    public override PlacementInstance Place(WorldInstance world, Vector2Int position, NoiseValue noiseValues)
    {
        Tilemap tilemap = world.GetMap(map);
        int valueAverage = (int)(((noiseValues.heightValue + noiseValues.heatValue + noiseValues.moistureValue) * 100) / 3);
        int randomSeed = (valueAverage * (position.x * position.y));
        System.Random random = new System.Random(randomSeed);
        double randomNum = random.NextDouble();
        TileBase tileBase;
        tiles.TrySelectPlacement(randomNum, out tileBase, noiseValues);
        tilemap.SetTile((Vector3Int)position, tileBase);
        return new TilePlacementInstance(tilemap, (Vector3Int)position);
    }

    public bool SelectDetail(Vector2Int position, out Placement placement, NoiseValue noiseValues)
    {
        int valueAverage = (int)(((noiseValues.heightValue + noiseValues.heatValue + noiseValues.moistureValue) * 100) / 3);
        int randomSeed = valueAverage * (position.x * position.y);
        System.Random random = new System.Random(randomSeed);
        double randomNum = random.NextDouble();

        if (details.TrySelectPlacement(randomNum, out placement, noiseValues))
        {
            return true;
        }

        placement = null;
        return false;
    }
    public override bool IsTraversable(Entity entity)
    {
        if (map == WorldInstance.Map.Collision)
        {
            return false;
        }
        else if (map == WorldInstance.Map.Base)
        {
            if (entity.GetTypeMatchups().GetEffectiveness(tileType) == Effectiveness.VeryEffective)
            {
                return false;
            }
        }
        return true;
    }
}