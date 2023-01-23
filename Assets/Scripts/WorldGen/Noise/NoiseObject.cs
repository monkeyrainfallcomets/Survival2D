using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class NoiseObject<Type>
{
    [SerializeField] RangeF heatValue = new RangeF(0, 1);
    [SerializeField] RangeF moistureValue = new RangeF(0, 1);
    [SerializeField] RangeF heightValue = new RangeF(0, 1);
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
