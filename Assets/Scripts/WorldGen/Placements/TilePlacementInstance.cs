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
    bool[] extensions = new bool[4];
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
    public override void Place(WorldInstance world)
    {
        base.Place(world);
    }

    public void AddTransition(Vector3Int direction)
    {
        transitions[GetIndex(direction)] = true;
    }
    public void AddExtension(Vector3Int direction)
    {
        transitions[GetIndex(direction)] = true;
    }

    void MakeTransitions(WorldInstance world)
    {
        Tilemap tilemap = world.GetMap(WorldInstance.Map.Transition);
        int tileIndex = GenTile.GetTransitionTileIndex(transitions);
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


    public GenTile GetTile()
    {
        return genTile;
    }
}
