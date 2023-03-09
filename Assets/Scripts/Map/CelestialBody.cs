using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CelestialBody : MonoBehaviour
{
    [SerializeField] bool exertForce;
    [SerializeField] bool orbit;
    [SerializeField] bool moveable;
    [SerializeField] float mass;
    [SerializeField] GameObject interactionDisplay;
    protected SpriteRenderer spriteRenderer;
    public virtual void Interact()
    {
        interactionDisplay.SetActive(true);
    }
    public virtual void Destroy()
    {
        Destroy(gameObject);
    }
    public Vector2 Size()
    {
        return spriteRenderer.bounds.size;
    }
    public bool CanExertForce()
    {
        return exertForce;
    }
    public bool CanRecieveForce()
    {
        return exertForce;
    }
    public void ReceiveGravity(CelestialBody body, float G)
    {
        Vector2 direction = body.transform.position - transform.position;
        Vector2 directionNormalized = direction.normalized;
        float distance = direction.magnitude;
        //gravity
        Vector2 transformation = ((G * (mass * body.mass) / (distance * distance)) * directionNormalized) / mass;
        if (orbit)
        {
            float orbitalForce = Mathf.Sqrt(G * body.mass / distance);
            //applying transform
            transformation += orbitalForce * new Vector2(directionNormalized.y, -directionNormalized.x);
        }
        transform.position = new Vector3(transform.position.x + transformation.x, transform.position.y + transformation.y, 0);
    }
}
[System.Serializable]
public class Action
{
    public string name;
    public void Use()
    {

    }
}
