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
    List<Vector3Int> transitions = new List<Vector3Int>();
    public TilePlacementInstance(Tilemap tilemap, Vector3Int position, GenTile genTile)
    {
        this.position = position;
        this.tilemap = tilemap;
        this.genTile = genTile;
    }

    public override void Destroy(WorldInstance world)
    {
        tilemap.SetTile(position, null);
        Tilemap transitionMap = world.GetMap(WorldInstance.Map.Transition);
        for (int i = 0; i < transitions.Count; i++)
        {
            transitionMap.SetTile(transitions[0], null);
        }
    }

    public void AddTransition(Vector3Int adjacentPosition, WorldInstance world)
    {
        Vector3Int direction = adjacentPosition - position;
        Vector3Int rotation = new Vector3Int(0, 0, 0);
        if (direction == Vector3Int.down)
        {
            rotation = new Vector3Int(0, 0, 0);
        }
        else if (direction == Vector3Int.right)
        {
            rotation = new Vector3Int(0, 0, 180);
        }
        else if (direction == Vector3Int.up)
        {
            rotation = new Vector3Int(0, 0, 270);
        }
        Tilemap tilemap = world.GetMap(WorldInstance.Map.Transition);
        tilemap.SetTransformMatrix(adjacentPosition, Matrix4x4.Rotate(Quaternion.Euler(rotation)));
    }

    public GenTile GetTile()
    {
        return genTile;
    }
}
