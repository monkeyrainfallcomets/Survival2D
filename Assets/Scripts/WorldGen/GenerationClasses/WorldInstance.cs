using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class WorldInstance : MonoBehaviour
{
    [SerializeField] Tilemap baseTilemap;
    [SerializeField] Tilemap collisionTilemap;
    WorldGenData genData;


    public void Initialize(WorldTile[] tiles)
    {
    }

    public void End()
    {

    }
}
