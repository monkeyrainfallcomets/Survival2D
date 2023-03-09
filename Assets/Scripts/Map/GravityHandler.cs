using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GravityHandler
{
    [SerializeField] float G;
    List<CelestialBody> forceAppliers = new List<CelestialBody>();
    List<CelestialBody> forceRecievers = new List<CelestialBody>();
    public void ApplyGravity()
    {
        for (int i = 0; i < forceRecievers.Count; i++)
        {
            for (int j = 0; j < forceAppliers.Count; j++)
            {
                forceRecievers[i].ReceiveGravity(forceAppliers[j], G);
            }
        }
    }
    public void Add(CelestialBody body)
    {
        if (body.CanExertForce())
        {
            forceAppliers.Add(body);
        }
        if (body.CanRecieveForce())
        {
            forceRecievers.Add(body);
        }
    }
}
