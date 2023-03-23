using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCelestialBody : CelestialBody
{
    protected CelestialBody ship;
    public void SetShipReference(CelestialBody ship)
    {
        this.ship = ship;
    }
}
