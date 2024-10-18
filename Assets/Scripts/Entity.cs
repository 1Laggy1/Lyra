using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class Entity : NetworkBehaviour
{
    [SyncVar]
    public float Health = 100;
    public float Knockback = 3;
    public float DamageF = 1;

    public Rigidbody2D Rb;
    [SyncVar]
    public bool IsAlive = true;

    public float AttackSpeed;
    public float TimeSinceAttack;

    public event EventHandler EntityDied;

    public event EventHandler EntityDamaged;

    public virtual void Update()
    {
        TimeSinceAttack += Time.deltaTime;
    }

    public virtual void Attack(int fasing)
    {
    }

    [Command(requiresAuthority = false)]
    public virtual void Damage(float damage, int fasing)
    {
        DamageRPC(damage, fasing);
    }
    [ClientRpc]
    public void DamageRPC(float damage, int fasing)
    {
        Health -= damage;
        Rb.AddForce(new Vector2(Knockback * fasing * 10000, Knockback * 10000), ForceMode2D.Force);
        if (Health <= 0)
        {
            EntityDied?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            EntityDamaged?.Invoke(this, EventArgs.Empty);
        }
    }
}
