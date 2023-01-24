using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
[CreateAssetMenu(fileName = "NewTile", menuName = "WorldGen/Placements/Tile")]
public class DetailTile : Placement
{
    [SerializeField] DamageTypes tileType;
    [SerializeField] TileBase tile;
    [SerializeField] WorldInstance.Map map;
    [Header("Entity's effectiveness must be less than this for it to be traversable by that entity")]
    [SerializeField] protected Effectiveness traversability;
    public override bool IsTraversable(Entity entity)
    {
        if (map == WorldInstance.Map.Collision)
        {
            return false;
        }
        if (entity.GetTypeMatchups().GetEffectiveness(tileType) >= traversability)
        {
            return false;
        }
        return true;
    }

    public override PlacementInstance CreateInstance(WorldInstance world, Vector2Int position)
    {
        Tilemap tilemap = world.GetMap(map);
        Vector3Int tilePosition = (Vector3Int)position;
        return new DetailTilePlacementInstance(tilemap, tilePosition, tile);
    }

}
