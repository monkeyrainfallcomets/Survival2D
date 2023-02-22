using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : Entity
{
    Suit suit;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }
    public Vector2 GetPosition()
    {
        return transform.position;
    }
}

public class UnitLineUp
{
    public Unit[] units;
    int initialUnitSpacing;
    public Vector2Range GetFieldOfVision(Vector2Int viewDistance)
    {
        if (units.Length == 1)
        {
            return new Vector2Range(new Range(-viewDistance.x, viewDistance.x), new Range(-viewDistance.y, viewDistance.y));
        }
        else if (units.Length == 2)
        {
            return new Vector2Range(new Range(-viewDistance.x - initialUnitSpacing, viewDistance.x), new Range(-viewDistance.y, viewDistance.y));
        }
        else if (units.Length == 3)
        {
            return new Vector2Range(new Range(-viewDistance.x - initialUnitSpacing, viewDistance.x + initialUnitSpacing), new Range(-viewDistance.y, viewDistance.y));
        }
        else if (units.Length == 4)
        {
            return new Vector2Range(new Range(-viewDistance.x - initialUnitSpacing, viewDistance.x + initialUnitSpacing), new Range(-viewDistance.y - initialUnitSpacing, viewDistance.y));
        }
        else if (units.Length == 5)
        {
            return new Vector2Range(new Range(-viewDistance.x - initialUnitSpacing, viewDistance.x + initialUnitSpacing), new Range(-viewDistance.y - initialUnitSpacing, viewDistance.y + initialUnitSpacing));
        }
        else
        {
            return null;
        }
    }
}