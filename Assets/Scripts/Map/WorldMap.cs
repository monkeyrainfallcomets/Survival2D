using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class WorldMap : MonoBehaviour
{
    public readonly float tileSize;
    public readonly Vector2 mapSize;
    public static EventHandler UpdateCycleEvent;
    [SerializeField] Dictionary<Vector2, CelestialBody> mapPositions = new Dictionary<Vector2, CelestialBody>();
    [SerializeField] GravityHandler gravityHandler;
    [SerializeField] CelestialBodyRing[] rings;
    [SerializeField] RandomSpawnRing[] spawnRings;
    public bool Place(CelestialBody celestialBody, Vector2 position)
    {
        Vector2 startTile = position / tileSize;
        Vector2 size = celestialBody.Size();
        if (TryFillPositions(new Vector2(position.x - size.x, position.y - size.y), new Vector2(position.x + size.x, position.y + size.y), celestialBody))
        {
            gravityHandler.Add(Instantiate(celestialBody, position, Quaternion.identity));
            return true;
        }
        return false;
    }

    void Start()
    {

    }
    public bool TryFillPositions(Vector2 startPosition, Vector2 endPosition, CelestialBody body)
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

        for (int y = yStartTile; y < yEndTile; y++)
        {
            for (int x = xStartTile; x < xEndTile; x++)
            {
                if (mapPositions.ContainsKey(new Vector2(x, y)))
                {
                    return false;
                }
            }
        }

        for (int y = yStartTile; y < yEndTile; y++)
        {
            for (int x = xStartTile; x < xEndTile; x++)
            {
                mapPositions[new Vector2(x, y)] = body;
            }
        }
        return true;
    }
    public void UpdateCycle()
    {

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


