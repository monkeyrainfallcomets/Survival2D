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
    public TileBase[] extensionTiles;
    public int GetPriority(Planet planet)
    {
        return priority[planet];
    }
    public static int GetTransitionTileIndex(bool[] transitions)
    {
        if (transitions[0])
        {
            if (transitions[1])
            {
                if (transitions[2])
                {
                    if (transitions[3])
                    {
                        return 14;
                    }
                    return 10;
                }
                if (transitions[3])
                {
                    return 13;
                }
                return 4;
            }
            if (transitions[2])
            {
                if (transitions[3])
                {
                    return 12;
                }
                return 5;
            }
            if (transitions[3])
            {
                return 6;
            }
            return 0;
        }
        else if (transitions[1])
        {
            if (transitions[2])
            {
                if (transitions[3])
                {
                    return 11;
                }
                return 7;
            }
            if (transitions[3])
            {
                return 8;
            }
            return 1;
        }
        else if (transitions[2])
        {
            if (transitions[3])
            {
                return 9;
            }
            return 2;
        }
        else if (transitions[3])
        {
            return 3;
        }
        return -1;
    }
}