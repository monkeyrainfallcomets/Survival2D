using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RandomNoiseGroup<Type>
{
    [Range(0f, 1f)]
    [SerializeField] double chance = 1;
    [SerializeField] NoiseGroup<Type>[] placements;
    public bool TrySelectPlacement(double random, out Type placement, NoiseValue noiseValues)
    {
        if (chance >= random)
        {
            for (int i = 0; i < placements.Length; i++)
            {
                if (placements[i].WithinRange(noiseValues))
                {
                    placement = placements[i].GetPlacement(noiseValues);
                    return true;
                }
            }
        }
        placement = default(Type);
        return false;
    }
}
