using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Tilemaps;
[System.Serializable]
public class TilePlacementInstance : PlacementInstance
{
    Vector3Int position;
    Tilemap tilemap;
    GenTile genTile;
    bool[] transitions = new bool[4];
    public TilePlacementInstance(Tilemap tilemap, Vector3Int position, GenTile genTile)
    {
        this.position = position;
        this.tilemap = tilemap;
        this.genTile = genTile;
    }

    public override void Destroy(WorldInstance world)
    {
        tilemap.SetTile(position, null);
    }

    public void AddTransition(Vector3Int adjacentPosition)
    {
        Vector3Int direction = adjacentPosition - position;
        Vector3Int rotation = new Vector3Int(0, 0, 0);
        if (direction == Vector3Int.down)
        {
            rotation = new Vector3Int(0, 0, 90);
        }
        else if (direction == Vector3Int.right)
        {
            rotation = new Vector3Int(0, 0, 180);
        }
        else if (direction == Vector3Int.up)
        {
            rotation = new Vector3Int(0, 0, 270);
        }
        transitions[GetIndex(direction)] = true;
    }

    public void MakeTransition(WorldInstance world)
    {
        Tilemap tilemap = world.GetMap(WorldInstance.Map.Transition);
        int tileIndex = GetTransitionTile();
        if (tileIndex != -1)
        {
            tilemap.SetTile(position, genTile.transitionTiles[tileIndex]);
        }

    }

    private int GetIndex(Vector3Int direction)
    {
        if (direction == Vector3Int.left)
        {
            return 0;
        }
        else if (direction == Vector3Int.down)
        {
            return 1;
        }
        else if (direction == Vector3Int.right)
        {
            return 2;
        }
        else if (direction == Vector3Int.up)
        {
            return 3;
        }
        return -1;
    }
    private int GetTransitionTile()
    {
        if (transitions[0])
        {
            if (transitions[1])
            {
                if (transitions[2])
                {
                    if (transitions[3])
                    {
                        return 14;
                    }
                    return 10;
                }
                if (transitions[3])
                {
                    return 13;
                }
                return 4;
            }
            if (transitions[2])
            {
                if (transitions[3])
                {
                    return 12;
                }
                return 5;
            }
            if (transitions[3])
            {
                return 6;
            }
            return 0;
        }
        else if (transitions[1])
        {
            if (transitions[2])
            {
                if (transitions[3])
                {
                    return 11;
                }
                return 7;
            }
            if (transitions[3])
            {
                return 8;
            }
            return 1;
        }
        else if (transitions[2])
        {
            if (transitions[3])
            {
                return 9;
            }
            return 2;
        }
        else if (transitions[3])
        {
            return 3;
        }
        return -1;
    }

    public GenTile GetTile()
    {
        return genTile;
    }
}
