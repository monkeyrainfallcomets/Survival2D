using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New World", menuName = "WorldGen/World")]
public class WorldTemplate : ScriptableObject
{
    public NoiseMap worldMap;
    public NoiseMap biomeMap;
    public NoiseMap detailMap;
}
