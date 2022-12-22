using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;
public class Chunk
{
    Vector2Int position;
    WorldTemplate world;
    Tilemap detailmap;
    Tilemap baseTilemap;
    Tilemap collisionDetailmap;
    bool active;
    public Chunk(WorldTemplate world, WorldChunk chunkPrefab, Transform grid, Vector2Int position)
    {
        this.world = world;
        this.position = position;
        GameObject chunk = new GameObject("chunk");
        Vector3 worldPosition = new Vector3(position.x, position.y, 0);
        baseTilemap = MonoBehaviour.Instantiate(chunkPrefab.GetBaseTilemap(), worldPosition, Quaternion.identity);
        baseTilemap.transform.SetParent(grid);
        detailmap = MonoBehaviour.Instantiate(chunkPrefab.GetDetailTilemap(), worldPosition, Quaternion.identity);
        detailmap.transform.SetParent(grid);
        collisionDetailmap = MonoBehaviour.Instantiate(chunkPrefab.GetCollisionDetailmap(), worldPosition, Quaternion.identity);
        collisionDetailmap.transform.SetParent(grid);
        active = true;
    }
    public void PlaceTile(TileBase tile, Vector3Int position)
    {
        baseTilemap.SetTile(position, tile);
    }
    public bool IsActive()
    {
        return active;
    }
    public void SetActive(bool active)
    {
        this.active = active;
    }
    public Vector2Int GetPosition()
    {
        return position;
    }

    public WorldTemplate CurrentWorld()
    {
        return world;
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