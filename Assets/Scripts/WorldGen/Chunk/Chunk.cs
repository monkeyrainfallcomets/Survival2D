using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;
public class Chunk
{
    Dictionary<Vector2Int, bool> structurePositions = new Dictionary<Vector2Int, bool>();
    public Vector2Int position { get { return position; } private set { position = value; } }
    public WorldTemplate world { get { return world; } private set { world = value; } }
    Tilemap tilemap;
    public bool active { get { return active; } set { tilemap.gameObject.SetActive(value); active = value; } }

    public Chunk(WorldTemplate world, Tilemap tilemap, Vector2Int position)
    {
        this.world = world;
        this.position = position;
        this.tilemap = tilemap;
        active = true;
    }
    public void PlaceTile(TileBase tile, Vector3Int position)
    {
        tilemap.SetTile(position, tile);
    }

    public void SetStructurePosition(Vector2Int position)
    {
        if (!structurePositions.ContainsKey(position))
        {
            structurePositions.Add(position, true);
        }
    }

    public void RemoveStructurePosition(Vector2Int position)
    {
        if (structurePositions.ContainsKey(position))
        {
            structurePositions.Remove(position);
        }
    }
}

public class ChunkEvent : EventArgs
{
    public Chunk chunk;
    public ChunkEvent(Chunk chunk)
    {
        this.chunk = chunk;
    }
}

public class Vector2IntEvent : EventArgs
{
    public Vector2Int vector2;
    public Vector2IntEvent(Vector2Int vector2)
    {
        this.vector2 = vector2;
    }
}