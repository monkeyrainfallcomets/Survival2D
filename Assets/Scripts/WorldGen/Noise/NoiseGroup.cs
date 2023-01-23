using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NoiseGroup<Type>
{
    [SerializeField] RangeF heatValue = new RangeF(0, 1);
    [SerializeField] RangeF moistureValue = new RangeF(0, 1);
    [SerializeField] NoiseObject<Type>[] values;
    public virtual bool WithinRange(NoiseValue noiseValues)
    {
        return heatValue.WithinRange(noiseValues.heatValue) && moistureValue.WithinRange(noiseValues.moistureValue);
    }

    public Type GetPlacement(NoiseValue noiseValues)
    {
        for (int i = 0; i < values.Length; i++)
        {
            if (values[i].WithinRange(noiseValues))
            {
                return values[i].GetObject();
            }
        }
        return GetLastPlacement();
    }

    public Type GetLastPlacement()
    {
        return values[values.Length - 1].GetObject();
    }
}
