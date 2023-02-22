using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
[CreateAssetMenu(fileName = "NewPlanet", menuName = "WorldGen/Planet")]
public class PlanetParams : ScriptableObject
{
    public PlanetType planet;
    public NoiseMap moistureMap;
    public NoiseMap heatMap;
    public NoiseMap heightMap;
    public NoiseGroup<TilePlacement>[] tileGroups;
}


public struct NoiseValue
{
    public float moistureValue;
    public float heatValue;
    public float heightValue;

    public NoiseValue(float moistureValue, float heatValue, float heightValue)
    {
        this.moistureValue = moistureValue;
        this.heatValue = heatValue;
        this.heightValue = heightValue;
    }
}