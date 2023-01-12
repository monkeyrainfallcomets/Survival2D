using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Range
{
    public int min;
    public int max;

    public Range(int min, int max)
    {
        this.min = min;
        this.max = max;
    }

    public int GenerateNumber()
    {
        return Random.Range(min, max);
    }
    public bool WithinRange(int num)
    {
        return num > min && num < max;
    }
}
