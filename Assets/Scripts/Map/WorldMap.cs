using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class WorldMap : MonoBehaviour
{
    [SerializeField] float tileSize;
    [SerializeField] Vector2 mapSize;
    [SerializeField] Canvas canvas;
    public static EventHandler UpdateCycleEvent;
    [SerializeField] Dictionary<Vector2Int, CelestialBody> mapPositions = new Dictionary<Vector2Int, CelestialBody>();
    [SerializeField] GravityHandler gravityHandler;
    [SerializeField] CelestialBodyRing[] rings;
    [SerializeField] RandomSpawnRing[] spawnRings;
    [SerializeField] CelestialBody sunPrefab;
    [SerializeField] CelestialBody shipPrefab;
    CelestialBody ship;

    void Start()
    {
        Place(sunPrefab, Vector2.zero);


    }
    public bool Place(CelestialBody celestialBody, Vector2 position)
    {
        Vector2 startTile = position / tileSize;
        Vector2 size = celestialBody.Size();
        Debug.Log(size);
        if (TryFillPositions(new Vector2(position.x - size.x, position.y - size.y), new Vector2(position.x + size.x, position.y + size.y), celestialBody, false, true))
        {
            CelestialBody instantiatedBody = Instantiate(celestialBody, position, Quaternion.identity);
            gravityHandler.Add(instantiatedBody);
            instantiatedBody.transform.SetParent(transform);
            return true;
        }
        return false;
    }
    public bool Place(MapCelestialBodyTemplate bodyTemplate, Vector2 position)
    {
        CelestialBody celestialBody = bodyTemplate.CreateCelestialBody(shipPrefab);
        Vector2 startTile = position / tileSize;
        Vector2 size = celestialBody.Size();
        if (TryFillPositions(new Vector2(position.x - (size.x / 2), position.y - (size.y / 2)), new Vector2(position.x + size.x, position.y + size.y), celestialBody, false, true))
        {
            gravityHandler.Add(celestialBody);
            celestialBody.transform.SetParent(transform);
            return true;
        }
        return false;
    }

    public bool OutOfBounds(CelestialBody celestialBody)
    {
        Vector2 positionInTiles = celestialBody.transform.position / tileSize;
        return positionInTiles.x > mapSize.x || positionInTiles.y > mapSize.y;
    }

    public void UpdatePosition(CelestialBody body, Vector3 newPosition)
    {
        Vector2 currentPosition = body.transform.position;
        Vector2 size = body.Size();
        Vector2 collisionPosition;
        CelestialBody collisionBody;
        if (!CheckForCollision(new Vector2(newPosition.x - size.x, currentPosition.y - size.y), new Vector2(currentPosition.x + size.x, currentPosition.y + size.y), body, out collisionPosition, out collisionBody))
        {
            bool collisionResultBody = body.OnCollision(collisionBody);
            bool collisionResultOther = collisionBody.OnCollision(body);
            if (!collisionResultBody && collisionResultOther)
            {
                Destroy(body);
                return;
            }
            else if (collisionResultBody && !collisionResultOther)
            {
                Destroy(collisionBody);
            }
        }
        TryFillPositions(new Vector2(currentPosition.x - size.x, currentPosition.y - size.y), new Vector2(currentPosition.x + size.x, currentPosition.y + size.y), body, true, false);
        TryFillPositions(new Vector2(newPosition.x - size.x, currentPosition.y - size.y), new Vector2(currentPosition.x + size.x, currentPosition.y + size.y), body, false, false);
    }
    bool CheckForCollision(Vector2 startPosition, Vector2 endPosition, CelestialBody body, out Vector2 collisionPosition, out CelestialBody collisionBody)
    {
        Vector2Int iterationStart;
        Vector2Int iterationEnd;
        ConvertStartEnd(startPosition, endPosition, out iterationStart, out iterationEnd);

        for (int y = iterationStart.y; y <= iterationEnd.y; y++)
        {
            for (int x = iterationStart.x; x <= iterationEnd.x; x++)
            {
                Vector2Int position = new Vector2Int(x, y);
                Debug.Log(position);
                if (mapPositions.ContainsKey(position) && mapPositions[position] != body)
                {
                    collisionPosition = position;
                    collisionBody = mapPositions[position];
                    return true;
                }
            }
        }
        collisionPosition = Vector2.zero;
        collisionBody = null;
        return false;
    }

    bool TryFillPositions(Vector2 startPosition, Vector2 endPosition, CelestialBody body, bool clear, bool checkForCollision)
    {
        Vector2Int iterationStart;
        Vector2Int iterationEnd;
        ConvertStartEnd(startPosition, endPosition, out iterationStart, out iterationEnd);
        Debug.Log(iterationStart + " " + iterationEnd);
        if (!checkForCollision && !clear)
        {
            for (int y = iterationStart.y; y <= iterationEnd.y; y++)
            {
                for (int x = iterationStart.x; x <= iterationEnd.x; x++)
                {
                    if (mapPositions.ContainsKey(new Vector2Int(x, y)))
                    {
                        if (!clear)
                        {
                            return false;
                        }
                        else
                        {
                            mapPositions.Remove(new Vector2Int(x, y));
                        }
                    }
                }
            }
        }

        if (clear)
        {
            return true;
        }
        for (int y = iterationStart.y; y <= iterationEnd.y; y++)
        {
            for (int x = iterationStart.x; x <= iterationEnd.x; x++)
            {
                Debug.Log(new Vector2Int(x, y));
                mapPositions[new Vector2Int(x, y)] = body;
            }
        }
        return true;
    }

    void ConvertStartEnd(Vector2 startPosition, Vector2 endPosition, out Vector2Int returnStartPosition, out Vector2Int returnEndPosition)
    {
        int xStartTile = 0;
        int xEndTile = 0;
        int yStartTile = 0;
        int yEndTile = 0;
        if (startPosition.x <= endPosition.x)
        {
            xStartTile = (int)(startPosition.x / tileSize);
            xEndTile = (int)(endPosition.x / tileSize);
        }
        else
        {
            xStartTile = (int)(endPosition.x / tileSize);
            xEndTile = (int)(startPosition.x / tileSize);
        }
        if (startPosition.y <= endPosition.y)
        {
            yStartTile = (int)(startPosition.y / tileSize);
            yEndTile = (int)(endPosition.y / tileSize);
        }
        else
        {
            xStartTile = (int)(endPosition.y / tileSize);
            xEndTile = (int)(startPosition.y / tileSize);
        }
        returnStartPosition = new Vector2Int(xStartTile, yStartTile);
        returnEndPosition = new Vector2Int(xEndTile, yEndTile);
    }

    void UpdateCycle()
    {

    }
    public float GetTileSize()
    {
        return tileSize;
    }
}

[System.Serializable]
public class OrbitalRing
{
    [SerializeField] RangeF radiusRange;
    public Vector2 GetRandomPosition()
    {
        float angle = UnityEngine.Random.Range(0, 2.0f) * Mathf.PI * 2;
        float radius = radiusRange.GenerateNumber();
        float x = Mathf.Cos(angle) * radius;
        float y = Mathf.Sign(angle) * radius;
        return new Vector2(x, y);
    }
}

public class CelestialBodyRing : OrbitalRing
{
    [SerializeField] CelestialBody body;
    public void PlaceBody(WorldMap map)
    {
        Vector2 position = GetRandomPosition();
        map.Place(body, position);
    }
}

public class RandomSpawnRing : OrbitalRing
{
    [SerializeField] RandomGroup<CelestialBody> celestialBodies;

    public bool TrySpawn(WorldMap map)
    {
        CelestialBody value;
        if (celestialBodies.TrySelect(UnityEngine.Random.Range(0, 1f), out value))
        {
            return map.Place(value, GetRandomPosition());
        }
        return false;
    }
}


