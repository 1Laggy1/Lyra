using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class Enemy : Entity
{
    public void Start()
    {
    }

    public void AttackPlayer(Entity entity)
    {
        if (TimeSinceAttack > AttackSpeed)
        {
            TimeSinceAttack = 0;
            entity.Damage(DamageF, transform.position.x > entity.transform.position.x ? -1 : 1);
        }
    }

    public void OnCollisionStay2D(Collision2D other)
    {

    }
}
