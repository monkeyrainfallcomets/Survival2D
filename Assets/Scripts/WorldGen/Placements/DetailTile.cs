using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
[CreateAssetMenu(fileName = "NewTile", menuName = "WorldGen/Placements/Tile")]
public class DetailTile : Placement
{
    [SerializeField] DamageTypes tileType;
    [SerializeField] TileBase tile;
    [SerializeField] PlanetGenerationInstance.Map map;
    [Header("Entity's effectiveness must be less than this for it to be traversable by that entity")]
    [SerializeField] protected Effectiveness traversability;
    [SerializeField] int zIndex;
    public override bool IsTraversable(Entity entity)
    {
        if (map == PlanetGenerationInstance.Map.Collision)
        {
            return false;
        }
        if (entity.GetTypeMatchups().GetEffectiveness(tileType) >= traversability)
        {
            return false;
        }
        return true;
    }

    public override PlacementInstance CreateInstance(PlanetGenerationInstance world, Vector2Int position)
    {
        Tilemap tilemap = world.GetMap(map);
        Vector3Int tilePosition = new Vector3Int(position.x, position.y, zIndex);
        return new DetailTilePlacementInstance(tilemap, tilePosition, tile);
    }

}
