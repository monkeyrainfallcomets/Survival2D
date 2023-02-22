using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetTemplate : CelestialObjectTemplate
{
    [SerializeField] List<PlanetParams> planets;
    [SerializeField] Planet planetPrefab;
    public override CelestialObject CreateCelestialObject()
    {
        Planet planet = Instantiate(planetPrefab);
        int index = Random.Range(0, planets.Count);
        planet.SetPlanet(planets[index]);
        planets.RemoveAt(index);
        return planet;
    }
}
