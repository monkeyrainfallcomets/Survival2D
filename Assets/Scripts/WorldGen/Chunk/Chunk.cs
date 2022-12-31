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
    Transform chunk;
    bool active;
    public Chunk(WorldTemplate world, WorldChunk chunkPrefab, Transform grid, Vector2Int position)
    {
        this.world = world;
        this.position = position;
        Vector3 worldPosition = new Vector3(position.x, position.y, 0);
        this.chunk = new GameObject("chunk").transform;
        this.chunk.position = worldPosition;
        baseTilemap = MonoBehaviour.Instantiate(chunkPrefab.GetBaseTilemap(), Vector3.zero, Quaternion.identity);
        baseTilemap.transform.SetParent(chunk);
        detailmap = MonoBehaviour.Instantiate(chunkPrefab.GetDetailTilemap(), Vector3.zero, Quaternion.identity);
        detailmap.transform.SetParent(chunk);
        collisionDetailmap = MonoBehaviour.Instantiate(chunkPrefab.GetCollisionDetailmap(), Vector3.zero, Quaternion.identity);
        collisionDetailmap.transform.SetParent(chunk);
        chunk.SetParent(grid);
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
        chunk.gameObject.SetActive(active);
    }
    public Vector2Int GetPosition()
    {
        return position;
    }

    public WorldTemplate CurrentWorld()
    {
        return world;
    }

    public void Destroy()
    {
        MonoBehaviour.Destroy(chunk.gameObject);
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