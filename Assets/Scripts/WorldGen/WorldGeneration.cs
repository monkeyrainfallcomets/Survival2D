using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;
public class WorldGeneration : MonoBehaviour
{
    public static event EventHandler<ChunkEvent> ChunkGenerating;
    public static event EventHandler<ChunkEvent> ChunkGenerated;

    [SerializeField] GameObject chunkPrefab;
    [SerializeField] WorldTemplate[] worlds;
    [SerializeField] GameObject[] playerPrefabs;
    [SerializeField] Vector2Int chunkSize;
    WorldTemplate world;
    void Start()
    {
        world = worlds[UnityEngine.Random.Range(0, worlds.Length)];
        world.worldMap.GenerateRandomSeed();
        world.biomeMap.GenerateRandomSeed();
        world.detailMap.GenerateRandomSeed();
        GenerateChunk(new Vector2Int(0, 0));
    }

    void GenerateChunk(Vector2Int position)
    {
        Vector2Int worldPosition = new Vector2Int(position.x * chunkSize.x, position.y * chunkSize.y);
        GameObject chunkGo = Instantiate(chunkPrefab);
    }

    void FillChunkTiles()
    {

    }

    void GetChunk()
    {

    }

}

public class Chunk
{
    Vector3 position;
    WorldTemplate world;
    Tilemap tilemap;
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