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

    public bool Traversable(Entity entity)
    {
        if (tile != null && !tile.IsTraversable(entity))
        {
            return false;
        }
        if (detail != null && !detail.IsTraversable(entity))
        {
            return false;
        }
        return true;
    }
    public void Place(WorldInstance world, NoiseValue noiseValues)
    {
        if (!placed)
        {
            placed = true;
        }
        if (tile)
        {
            tileInstance = tile.Place(world, position, noiseValues);
        }
        if (detail)
        {
            detailInstance = detail.Place(world, position, noiseValues);
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
