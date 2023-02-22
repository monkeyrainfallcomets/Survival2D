using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CelestialObjectTemplate : ScriptableObject
{
    // Start is called before the first frame update
    public virtual CelestialObject CreateCelestialObject()
    {
        return new CelestialObject();
    }
}
