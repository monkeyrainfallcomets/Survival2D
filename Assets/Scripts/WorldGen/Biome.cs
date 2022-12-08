using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Biome", menuName = "WorldGen/Biome")]
public class BiomeParams : ScriptableObject
{
    public Range noiseValue;
    public Placement[] placements;
}

public class Placement
{
    Range noiseValue;

}

