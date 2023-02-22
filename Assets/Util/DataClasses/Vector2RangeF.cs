using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Vector2FRange : MonoBehaviour
{
    public RangeF x;
    public RangeF y;
    public bool WithinRange(Vector2 vector)
    {
        return x.WithinRange(vector.x) && y.WithinRange(vector.x);
    }
}
