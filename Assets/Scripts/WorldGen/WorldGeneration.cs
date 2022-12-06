using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGeneration : MonoBehaviour
{
    [SerializeField] WorldTemplate[] worlds;
    WorldTemplate world;
    void Start()
    {
        world = worlds[Random.Range(0, worlds.Length)];
        world.worldMap.GenerateRandomSeed();
        world.biomeMap.GenerateRandomSeed();
        world.detailMap.GenerateRandomSeed();
    }


}
