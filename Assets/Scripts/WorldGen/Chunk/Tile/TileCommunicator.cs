using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class TileCommunicator : MonoBehaviour
{
    [SerializeField] Tilemap tilemap;
    [SerializeField] WorldChunk chunk;
    public Tilemap GetMap()
    {
        return tilemap;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        TileChange(other.transform.position, other.GetComponent<SpriteRenderer>().bounds.size);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        TileChange(other.transform.position, other.gameObject.GetComponent<SpriteRenderer>().bounds.size);
    }

    void TileChange(Vector3 position, Vector3 size)
    {
        List<TileBase> tiles = new List<TileBase>();
        for (int y = -1; y <= 1; y++)
        {

        }

    }
}
