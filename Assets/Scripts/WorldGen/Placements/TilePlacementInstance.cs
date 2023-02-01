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
    int priority;
    bool[] transitions = new bool[4];
    bool[] extensions = new bool[4];

    public TilePlacementInstance(Tilemap tilemap, Vector3Int position, GenTile genTile, int priority)
    {
        this.position = position;
        this.tilemap = tilemap;
        this.genTile = genTile;
        this.priority = priority;
    }

    public override void Destroy(WorldInstance world)
    {
        tilemap.SetTile(position, null);
    }
    public override void Place(WorldInstance world)
    {
        Debug.Log(position + " was placed");
        int tileIndex = GenTile.GetTransitionTileIndex(transitions);
        int extensionIndex = GenTile.GetTransitionTileIndex(extensions);
        if (tileIndex != -1 && genTile.transitionTiles != null)
        {
            Tilemap transitionTilemap = world.GetMap(WorldInstance.Map.Transition);
            tilemap.SetTile(position, genTile.transitionTiles[tileIndex]);
        }
        else
        {
            tilemap.SetTile(position, genTile.baseTile);
        }
        if (extensionIndex != -1 && genTile.extensionTiles != null)
        {
            Tilemap extensionTilemap = world.GetMap(WorldInstance.Map.Extension);
            extensionTilemap.SetTile(new Vector3Int(position.x, position.y, priority), genTile.extensionTiles[extensionIndex]);
        }
    }

    public void AddTransition(Vector2Int direction)
    {
        Debug.Log("transition added at " + position + "it was applied to tile in the direction " + direction);
        transitions[GetIndex(direction)] = true;
    }
    public void AddExtension(Vector2Int direction)
    {
        Debug.Log("extension added at " + position + "it was applied to tile in the direction " + direction);
        extensions[GetIndex(direction)] = true;
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
