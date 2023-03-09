using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "NewTile", menuName = "WorldGen/WorldTiles/Tile")]
public class TilePlacement : ScriptableObject
{
    [SerializeField] GenTile baseTile;
    [SerializeField] RandomNoiseGroup<GenTile>[] tiles;
    [SerializeField] TileAdjustmentGroup[] tileAdjustments;
    [SerializeField] DamageTypes damageType;
    [SerializeField] int zIndex;

    [Header("Entity's effectiveness must be less than this for it to be traversable by that entity")]
    [SerializeField] protected Effectiveness traversability;

    public TilePlacementInstance CreateInstance(PlanetGenerationInstance world, Vector2Int position, NoiseValue noiseValues)
    {
        Tilemap tilemap = world.GetMap(PlanetGenerationInstance.Map.Base);
        GenTile tile = GetTile(position, noiseValues);
        return new TilePlacementInstance(tilemap, new Vector3Int(position.x, position.y, zIndex), tile, tile.tile.GetPriority(world.GetPlanet()));
    }

    public TilePlacementInstance CreateInstance(GenTile tile, PlanetGenerationInstance world, Vector2Int position)
    {
        Tilemap tilemap = world.GetMap(PlanetGenerationInstance.Map.Base);
        return new TilePlacementInstance(tilemap, (Vector3Int)position, tile, tile.tile.GetPriority(world.GetPlanet()));
    }

    public GenTile GetTile(Vector2Int position, NoiseValue noiseValues)
    {
        int valueAverage = (int)(((noiseValues.heightValue + noiseValues.heatValue + noiseValues.moistureValue) * 100) / 3);
        int randomSeed = (valueAverage * (position.x + position.y));
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

    public bool PostGenerationAdjustments(PlanetGenerationInstance world, Vector3Int position, out TilePlacementInstance tileInstance)
    {
        if (tileAdjustments != null)
        {
            TilePlacementInstance[] tiles = new TilePlacementInstance[4];
            world.GetTile(new Vector2Int(position.x - 1, position.y), out tiles[0]);
            world.GetTile(new Vector2Int(position.x, position.y - 1), out tiles[1]);
            world.GetTile(new Vector2Int(position.x + 1, position.y), out tiles[2]);
            world.GetTile(new Vector2Int(position.x, position.y + 1), out tiles[3]);
            GenTile genTile;
            for (int i = 0; i < tileAdjustments.Length; i++)
            {
                if (tileAdjustments[i].GetTile(out genTile, tiles))
                {
                    tileInstance = new TilePlacementInstance(world.GetMap(PlanetGenerationInstance.Map.Base), new Vector3Int(position.x, position.y, zIndex), genTile, genTile.tile.GetPriority(world.GetPlanet()));
                    return true;
                }
            }
        }
        tileInstance = null;
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
    public GenTile GetBaseTile()
    {
        return baseTile;
    }
}

