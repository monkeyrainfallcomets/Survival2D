using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
[CreateAssetMenu(fileName = "NewGenTile", menuName = "WorldGen/GenTile")]
public class GenTile : ScriptableObject
{
    [SerializeField] SerializableDictionary<Planet, int> priority;
    public TileBase baseTile;
    public TileBase[] transitionTiles;
    public int GetPriority(Planet planet)
    {
        return priority[planet];
    }
}