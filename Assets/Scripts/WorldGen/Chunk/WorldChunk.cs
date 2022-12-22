using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;
public class WorldChunk : MonoBehaviour
{
    [SerializeField] Tilemap baseTilemap;
    [SerializeField] Tilemap detailTilemap;
    [SerializeField] Tilemap collisionDetailmap;
    public static event EventHandler<TileChangeEvent> TileChange;
    private void OnTriggerEnter2D(Collider2D other)
    {
        Entity entity = other.GetComponent<Entity>();
        if (TileChange != null)
        {
            TileChange(this, new TileChangeEvent());
        }
    }
    public Tilemap GetBaseTilemap()
    {
        return baseTilemap;
    }

    public Tilemap GetDetailTilemap()
    {
        return detailTilemap;
    }

    public Tilemap GetCollisionDetailmap()
    {
        return collisionDetailmap;
    }

    public void RunTileChange(TileBase[] tiles, GameObject gameObject, Vector3Int position)
    {

    }
}

public struct TileChangeEvent
{
    public GameObject gameObject;
    public TileBase newTile;
    public Vector3Int position;
    public TileChangeEvent(GameObject gameObject, TileBase newTile, Vector3Int position)
    {
        this.gameObject = gameObject;
        this.newTile = newTile;
        this.position = position;
    }
}