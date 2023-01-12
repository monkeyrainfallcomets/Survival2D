using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
[System.Serializable]
public class WorldTile
{
    [SerializeField] Vector2Int position;
    [SerializeField] NoiseValue noiseValues;
    [SerializeField] Placement detail;
    [SerializeField] TilePlacement tile;
    [SerializeField] Structure structure;
    bool placed;
    PlacementInstance detailInstance;
    PlacementInstance tileInstance;
    public WorldTile(Vector2Int position, NoiseValue noiseValues, TilePlacement tile, Placement detail, Structure structure)
    {
        this.position = position;
        this.noiseValues = noiseValues;
        this.detail = detail;
        this.tile = tile;
        this.structure = structure;
    }

    public WorldTile(Vector2Int position, NoiseValue noiseValues, TilePlacement tile)
    {
        this.position = position;
        this.noiseValues = noiseValues;
        this.detail = null;
        this.tile = tile;
        this.structure = null;
    }

    public WorldTile(Vector2Int position, NoiseValue noiseValues, TilePlacement tile, Placement detail)
    {
        this.position = position;
        this.noiseValues = noiseValues;
        this.detail = detail;
        this.tile = tile;
        this.structure = null;
    }

    public bool Traversable(TileBase[] nonTraversable)
    {
        if (tile != null && !tile.IsTraversable(nonTraversable))
        {
            return false;
        }
        if (detail != null && !detail.IsTraversable(nonTraversable))
        {
            return false;
        }
        return true;
    }
    public void Place(WorldInstance world)
    {
        if (!placed)
        {
            placed = true;
        }
        if (tile)
        {
            tileInstance = tile.Place(world, position);
        }
        if (detail)
        {
            detailInstance = detail.Place(world, position);
        }
    }

    public void Destroy()
    {
        if (placed)
        {
            if (tileInstance != null)
            {
                tileInstance.Destroy();
            }

            if (detailInstance != null)
            {
                detailInstance.Destroy();
            }
        }
    }
}
