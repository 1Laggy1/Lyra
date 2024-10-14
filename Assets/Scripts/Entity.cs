using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public float Health = 100;
    public float Knockback = 3;
    public float DamageF = 1;
    public Rigidbody2D Rb;
    public bool IsAlive = true;

    public event EventHandler EntityDied;

    public virtual void Attack(int fasing)
    {
    }

    public virtual void Damage(float damage, int fasing)
    {
        Health -= damage;
        Rb.AddForce(new Vector2(Knockback * fasing, 0));
        if (Health <= 0)
        {
            EntityDied?.Invoke(this, EventArgs.Empty);
        }
    }
}
