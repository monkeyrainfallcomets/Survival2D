using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetTemplate : MapCelestialBodyTemplate
{
    [SerializeField] List<PlanetParams> planets;
    [SerializeField] Planet planetPrefab;
    public override MapCelestialBody CreateCelestialBody(CelestialBody ship)
    {
        Planet planet = Instantiate(planetPrefab);
        int index = Random.Range(0, planets.Count);
        planet.SetPlanet(planets[index]);
        planets.RemoveAt(index);
        return planet;
    }
}
