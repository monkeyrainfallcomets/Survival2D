using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GoPlacementInstance : PlacementInstance
{
    GameObject gameObject;
    public GoPlacementInstance(GameObject gameObject)
    {
        this.gameObject = gameObject;
    }
    public override void Destroy()
    {
        MonoBehaviour.Destroy(gameObject);
    }
}
