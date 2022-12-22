using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;
public class WorldGeneration : MonoBehaviour
{
    public static event EventHandler<ChunkEvent> ChunkGenerating;
    public static event EventHandler<ChunkEvent> ChunkGenerated;
    [SerializeField] SerializableDictionary<int, int> dictionary;

    [SerializeField] WorldChunk chunkPrefab;
    [SerializeField] WorldTemplate[] worlds;
    [SerializeField] GameObject[] playerPrefabs;
    [SerializeField] Transform grid;
    [SerializeField] Vector2Int chunkSize;
    [SerializeField] int maxActiveChunks;
    WorldTemplate world;
    void Start()
    {
        GenerateWorld(worlds[UnityEngine.Random.Range(0, worlds.Length)]);
    }

    void GenerateWorld(WorldTemplate world)
    {
        this.world = world;
        world.GenerateSeeds();
        GenerateChunk(new Vector2Int(0, 0));
    }

    void GenerateChunk(Vector2Int chunkPosition)
    {
        Vector2 worldPosition = new Vector2Int(chunkPosition.x * chunkSize.x, chunkPosition.y * chunkSize.y);

        Chunk chunk = new Chunk(world, chunkPrefab, grid, chunkPosition);
        FillChunkTiles(chunk);
    }

    void FillChunkTiles(Chunk chunk)
    {
        Vector2Int tilePosition = Vector2Int.zero;
        for (int y = 0; y < chunkSize.y; y++)
        {
            for (int x = 0; x < chunkSize.x; x++)
            {
                tilePosition = new Vector2Int(x, y) + chunk.GetPosition();
                chunk.CurrentWorld().PlaceTile(chunk, tilePosition);
            }
        }
    }
}


