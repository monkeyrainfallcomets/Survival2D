using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Vector2Range : MonoBehaviour
{
    public Range x;
    public Range y;
    public Vector2Range(Range x, Range y)
    {
        this.x = x;
        this.y = y;
    }

    public bool WithinRange(Vector2Int vector)
    {
        return x.WithinRange(vector.x) && y.WithinRange(vector.x);
    }
}
