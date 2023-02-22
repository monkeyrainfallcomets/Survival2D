using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
[CreateAssetMenu(menuName = "WorldGen/WorldTiles/BaseTile", fileName = "newBaseTile")]
public class BaseTile : ScriptableObject
{
    [SerializeField] SerializableDictionary<PlanetType, int> priority;
    public TileBase baseTile;
    public Texture2D baseTexture;
    public Texture2D[] transitionTiles;
    public Texture2D[] cornerTransitions;

    public int GetPriority(PlanetType planet)
    {
        return priority[planet];
    }
}
