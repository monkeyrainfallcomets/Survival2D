using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class NoiseObject<Type>
{
    [SerializeField] RangeF heatValue;
    [SerializeField] RangeF moistureValue;
    [SerializeField] RangeF heightValue;
    [SerializeField] Type value;

    public bool WithinRange(NoiseValue noiseValues)
    {
        return heatValue.WithinRange(noiseValues.heatValue) && moistureValue.WithinRange(noiseValues.moistureValue) && heightValue.WithinRange(noiseValues.heightValue);
    }

    public Type GetObject()
    {
        return value;
    }
}
