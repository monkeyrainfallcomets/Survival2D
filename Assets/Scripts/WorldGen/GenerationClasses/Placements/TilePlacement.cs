using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "NewTile", menuName = "WorldGen/Placements/Tile")]
public class TilePlacement : Placement
{
    [SerializeField] RandomNoiseGroup<Placement> details;
    [SerializeField] TileBase tile;
    [SerializeField] WorldInstance.Map map;
    public override PlacementInstance Place(WorldInstance world, Vector2Int position)
    {
        Tilemap tilemap = world.GetMap(map);
        tilemap.SetTile((Vector3Int)position, tile);
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
    public override bool IsTraversable(TileBase[] nonTraversable)
    {
        if (map == WorldInstance.Map.Collision)
        {
            return false;
        }
        else if (map == WorldInstance.Map.Base)
        {
            for (int i = 0; i < nonTraversable.Length; i++)
            {
                if (tile == nonTraversable[i])
                {
                    return false;
                }
            }
        }
        return true;
    }
}