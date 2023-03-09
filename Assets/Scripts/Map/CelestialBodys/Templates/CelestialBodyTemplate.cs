using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CelestialBodyTemplate : ScriptableObject
{
    // Start is called before the first frame update
    public virtual CelestialBody CreateCelestialBody()
    {
        return new CelestialBody();
    }
}
