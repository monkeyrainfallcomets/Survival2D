using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;
using System.Threading;
public class WorldGeneration : MonoBehaviour
{
    public static event EventHandler<ChunkEvent> ChunkGenerating;
    public static event EventHandler<ChunkEvent> ChunkGenerated;

    [SerializeField] WorldChunk chunkPrefab;
    [SerializeField] WorldTemplate[] worlds;
    [SerializeField] GameObject[] playerPrefabs;
    [SerializeField] Transform grid;
    [SerializeField] Vector2Int chunkSize;
    [SerializeField] int maxActiveChunks;
    [SerializeField] Transform player;
    [SerializeField] TileBase[] nonTraversable;
    [SerializeField] int requiredSpawnPoints;
    [SerializeField] int viewDistance = 1;
    [SerializeField] int despawnDistance = 5;
    Dictionary<Vector2Int, Chunk> chunks = new Dictionary<Vector2Int, Chunk>();
    List<Vector2Int> visibleChunks = new List<Vector2Int>();
    Vector3 playerPosition;
    Vector2Int playerChunk;
    WorldTemplate world;
    List<ChunkData> completedChunks = new List<ChunkData>();
    bool updateCycle = false;
    bool chunkGenerating = false;
    void Start()
    {
        GenerateWorld(worlds[UnityEngine.Random.Range(0, worlds.Length)], player, nonTraversable, UnityEngine.Random.Range(0, 999999999));
    }
    void Update()
    {
        if (updateCycle && playerPosition != player.position)
        {
            if (!chunkGenerating)
            {
                StartCoroutine("ChunkUpdate");
            }
        }
        if (completedChunks.Count != 0)
        {
            for (int i = 0; i < completedChunks[0].positions.Count; i++)
            {
                Debug.Log(completedChunks[0].positions[i]);
            }
        }
    }

    IEnumerator ChunkUpdate()
    {
        chunkGenerating = true;
        Vector2Int chunk = WorldPositionToChunk(player.position, true);
        if (chunk != playerChunk)
        {
            while (visibleChunks.Count != 0)
            {
                int distanceX = Math.Abs(chunk.x - visibleChunks[0].x);
                int distanceY = Math.Abs(chunk.y - visibleChunks[0].y);
                if (distanceX > viewDistance || distanceY > viewDistance)
                {
                    chunks[visibleChunks[0]].SetActive(false);
                }
                visibleChunks.RemoveAt(0);
            }
            for (int y = chunk.y - viewDistance; y <= chunk.y + viewDistance; y++)
            {
                for (int x = chunk.x - viewDistance; x <= chunk.x + viewDistance; x++)
                {
                    Vector2Int position = new Vector2Int(x, y);
                    if (!chunks.ContainsKey(position))
                    {
                        GenerateChunk(position, nonTraversable);
                    }
                    else
                    {
                        if (!chunks[position].IsActive())
                        {
                            chunks[position].SetActive(true);
                        }
                    }
                    visibleChunks.Add(position);
                    yield return null;
                }
            }
        }
        playerPosition = player.position;
        playerChunk = chunk;
        chunkGenerating = false;
    }
    public void GenerateWorld(WorldTemplate world, Transform player, TileBase[] nonTraversable, int seed)
    {
        Thread thread = new Thread(new ParameterizedThreadStart(delegate { GenerateChunk(new Vector2Int(0, 0)); }));
        thread.Start();
        this.world = world;
        UnityEngine.Random.InitState(seed);
        world.GenerateSeeds();
        List<Vector2Int> spawnPoints = new List<Vector2Int>();
        Vector2Int position;
        int iterations = 1;
        int x = 0;
        int y = 0;
        /*while (spawnPoints.Count <= requiredSpawnPoints)
        {
            position = new Vector2Int(x, y);
            spawnPoints.AddRange();
            visibleChunks.Add(position);
            x++;
            if (x >= iterations)
            {
                y++;
                if (y == iterations - 1)
                {
                    x = 0;
                }
                else if (y >= iterations)
                {
                    x = iterations;
                }
                else
                {
                    x = iterations - 1;
                }
                if (y >= iterations)
                {
                    y = 0;
                    iterations++;
                }
            }
        }
        Vector2Int chosenPosition = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Count)];
        playerPosition = new Vector3(chosenPosition.x, chosenPosition.y, 0);
        player.position = playerPosition;
        StartCoroutine("ChunkUpdate");
        updateCycle = true;*/
    }

    //this overloaded version will add possible player spawn locations
    void GenerateChunk(Vector2Int chunkPosition, TileBase[] nonTraversable)
    {
        System.Random random = new System.Random();
    }

    void GenerateChunk(Vector2Int chunkPosition)
    {
        Vector2Int worldPosition = new Vector2Int(chunkPosition.x * chunkSize.x, chunkPosition.y * chunkSize.y);
        Chunk chunk = new Chunk(world, worldPosition);
        Dictionary<TileBase, List<Vector2Int>> tiles = FillChunkTiles(chunk);
        List<TilePosition> positions = new List<TilePosition>();
        lock (completedChunks)
        {
            Debug.Log("i'm like a locksmith");
            foreach (KeyValuePair<TileBase, List<Vector2Int>> item in tiles)
            {
                Debug.Log("check out this sick list");
                for (int i = 0; i < item.Value.Count; i++)
                {
                    positions.Add(new TilePosition(item.Value[i], item.Key));
                }
            }
            completedChunks.Add(new ChunkData(positions));
        }
    }

    Dictionary<TileBase, List<Vector2Int>> FillChunkTiles(Chunk chunk)
    {
        Dictionary<TileBase, List<Vector2Int>> returnData = new Dictionary<TileBase, List<Vector2Int>>();
        Vector2Int tilePosition = Vector2Int.zero;
        TilePlacement tilePlacement;
        TileBase tile;
        List<Vector2Int> currentList;
        for (int y = 0; y < chunkSize.y; y++)
        {
            for (int x = 0; x < chunkSize.x; x++)
            {
                tilePosition = new Vector2Int(x, y);
                tilePlacement = chunk.CurrentWorld().SelectTile(chunk, tilePosition);
                tile = tilePlacement.GetTile();
                if (returnData.TryGetValue(tile, out currentList))
                {
                    currentList.Add(tilePosition);
                }
                else
                {
                    returnData.Add(tile, new List<Vector2Int>() { tilePosition });
                }
            }
        }

        return returnData;
    }

    public Vector2Int WorldPositionToChunk(Vector3 position, bool round)
    {
        if (round)
        {
            return new Vector2Int(Mathf.RoundToInt(position.x / chunkSize.x), Mathf.RoundToInt(position.y / chunkSize.y));
        }
        return new Vector2Int((int)(position.x / chunkSize.x), (int)(position.y / chunkSize.y));
    }
}

public struct ChunkData
{
    public List<TilePosition> positions;

    public ChunkData(List<TilePosition> positions)
    {
        this.positions = positions;
    }
}

public struct TilePosition
{
    public Vector2Int position;
    public TileBase tile;
    public TilePosition(Vector2Int position, TileBase tile)
    {
        this.tile = tile;
        this.position = position;
    }
}