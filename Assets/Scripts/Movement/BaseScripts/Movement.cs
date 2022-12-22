using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Movement : ScriptableObject
{
    [SerializeField] Rigidbody2D rigidbody2D;
    [SerializeField] protected float speedMultiplier;
    public virtual void Move(Vector2 direction, float speed)
    {
        rigidbody2D.AddForce(direction * speed * speedMultiplier);
    }
}
