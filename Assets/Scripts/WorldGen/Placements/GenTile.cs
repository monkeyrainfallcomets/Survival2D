using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
[CreateAssetMenu(fileName = "NewGenTile", menuName = "WorldGen/GenTile")]
public class GenTile : ScriptableObject
{
    public int priority;
    public TileBase baseTile;
    public TileBase transitionPiece;
}