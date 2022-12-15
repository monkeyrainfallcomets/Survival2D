using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;
public class TileHandler : MonoBehaviour
{
    public static event EventHandler<TileChangeEvent> TileChange;
    private void OnTriggerEnter2D(Collider2D other)
    {
        Entity entity = other.GetComponent<Entity>();
        if (TileChange != null)
        {
            TileChange(this, new TileChangeEvent());
        }
    }
}

public struct TileChangeEvent
{
    public GameObject gameObject;
    public TileBase[] newTiles;
    public Vector3Int position;
    public TileChangeEvent(GameObject gameObject, TileBase[] newTiles, Vector3Int position)
    {
        this.gameObject = gameObject;
        this.newTiles = newTiles;
        this.position = position;
    }
}