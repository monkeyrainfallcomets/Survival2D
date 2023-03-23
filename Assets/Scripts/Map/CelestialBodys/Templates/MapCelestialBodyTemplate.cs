using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCelestialBodyTemplate : ScriptableObject
{
    // Start is called before the first frame update
    public virtual MapCelestialBody CreateCelestialBody(CelestialBody ship)
    {
        return new MapCelestialBody();
    }
}
