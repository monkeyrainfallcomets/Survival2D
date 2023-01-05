using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;
using System.Threading;
public class WorldGeneration : MonoBehaviour
{
    [SerializeField] WorldInstance chunkPrefab;
    [SerializeField] WorldTemplate[] worlds;
    [SerializeField] TileBase[] nonTraversable;
    [SerializeField] int requiredSpawnPoints;
    [SerializeField] WorldGenData worldGenData;
    Vector3 playerPosition;
    WorldTemplate world;
    WorldInstance worldInstance;

    public void Start()
    {

    }

    public void GenerateWorld(int seed, WorldTemplate world)
    {

    }

    public void EndGeneration()
    {
        worldInstance.End();
    }
}

[System.Serializable]
public struct WorldGenData
{
    public Transform player;
    public Vector2Int viewDistance;
    [HideInInspector] public WorldTemplate worldTemplate;

    public WorldGenData(Transform player, Vector2Int viewDistance, WorldTemplate worldTemplate)
    {
        this.player = player;
        this.viewDistance = viewDistance;
        this.worldTemplate = worldTemplate;
    }
}

