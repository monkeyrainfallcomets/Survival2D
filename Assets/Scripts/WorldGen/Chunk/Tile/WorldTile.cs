using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WorldTile
{
    Vector2Int position;
    NoiseValue noiseValues;
    TilePlacement tile;
    Structure structure;
    Placement detail;
    bool placed;
    PlacementInstance detailInstance;
    TilePlacementInstance tileInstance;

    public WorldTile(Vector2Int position, NoiseValue noiseValues, TilePlacement tile, WorldInstance world)
    {
        this.position = position;
        this.noiseValues = noiseValues;
        this.tile = tile;
        tile.CreateInstance(world, position, noiseValues);

        tile.SelectDetail(position, out detail, noiseValues);
        if (detail)
        {
            detail.CreateInstance(world, position);
        }
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
    public void Place(WorldInstance world, Vector2Int[] transitionDirections)
    {
        if (!placed)
        {
            placed = true;
            TilePlacementInstance outputTile;
            bool tileAdjusted = tile.PostGenerationAdjustments(world, (Vector3Int)position, out outputTile);
            if (tileAdjusted)
            {
                tileInstance = outputTile;
            }
            bool transitionsAdded = AddTransitions(transitionDirections, world);

            tileInstance.Place(world);
            if (detail != null)
            {
                detailInstance.Place(world);
            }
        }
    }

    public void Destroy(WorldInstance world)
    {
        if (placed)
        {
            if (tileInstance != null)
            {
                tileInstance.Destroy(world);
            }

            if (detailInstance != null)
            {
                detailInstance.Destroy(world);
            }
        }
    }

    public void Interact(Entity entity)
    {
        if (placed)
        {
            if (tileInstance != null)
            {
                tileInstance.Interact(entity);
            }

            if (detailInstance != null)
            {
                detailInstance.Interact(entity);
            }
        }
    }

    bool AddTransitions(Vector2Int[] directions, WorldInstance world)
    {
        TilePlacementInstance tile;

        if (tileInstance != null)
        {
            GenTile genTile = tileInstance.GetTile();
            GenTile adjacentTile;
            for (int i = 0; i < directions.Length; i++)
            {
                if (world.GetTile((Vector2Int)(directions[i] + position), out tile))
                {
                    adjacentTile = tile.GetTile();
                    if (genTile.GetPriority(world.GetPlanet()) > adjacentTile.GetPriority(world.GetPlanet()))
                    {
                        tileInstance.AddTransition(directions[i]);
                        tile.AddExtension(-directions[i]);
                    }
                    else if (genTile.GetPriority(world.GetPlanet()) < adjacentTile.GetPriority(world.GetPlanet()))
                    {
                        tile.AddTransition(-directions[i]);
                        tileInstance.AddExtension(directions[i]);
                    }
                }
            }
            return true;
        }
        return false;
    }

    public Vector2Int GetPosition()
    {
        return position;
    }

    public TilePlacementInstance GetTile()
    {
        return tileInstance;
    }
}
