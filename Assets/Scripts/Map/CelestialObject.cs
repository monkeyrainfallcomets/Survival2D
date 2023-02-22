using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CelestialObject : MonoBehaviour
{
    [SerializeField] GameObject interactionDisplay;
    public virtual void Interact()
    {
        interactionDisplay.SetActive(true);
    }
    public virtual void Destroy()
    {
        Destroy(gameObject);
    }
}
[System.Serializable]
public class Action
{
    public int name;
    public void Use()
    {

    }
}
