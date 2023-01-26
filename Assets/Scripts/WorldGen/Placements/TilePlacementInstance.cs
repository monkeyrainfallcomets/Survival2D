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

    public void AddTransition(Vector2Int direction)
    {
        transitions[GetIndex(direction)] = true;
    }
    public void AddExtension(Vector2Int direction)
    {
        transitions[GetIndex(direction)] = true;
    }

    void MakeTransitions(WorldInstance world)
    {
        Tilemap transitionTilemap = world.GetMap(WorldInstance.Map.Transition);
        Tilemap extensionTilemap = world.GetMap(WorldInstance.Map.Extension);
        int tileIndex = GenTile.GetTransitionTileIndex(transitions);
        int extensionIndex = GenTile.GetTransitionTileIndex(extensions);
        if (tileIndex != -1)
        {
            tilemap.SetTile(position, genTile.transitionTiles[tileIndex]);
        }
        else
        {
            Debug.Log("uh oh");
        }
        if (extensionIndex != -1)
        {
            tilemap.SetTile(position, genTile.extensionTiles[extensionIndex]);
        }
        else
        {
            Debug.Log("uh oh");
        }
    }



    private int GetIndex(Vector2Int direction)
    {
        if (direction == Vector2Int.left)
        {
            return 0;
        }
        else if (direction == Vector2Int.down)
        {
            return 1;
        }
        else if (direction == Vector2Int.right)
        {
            return 2;
        }
        else if (direction == Vector2Int.up)
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
