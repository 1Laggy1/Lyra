using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class Enemy : Entity
{
    void Start()
    {
        EntityDied += Died;
    }

    [ClientRpc]
    void Died(object sender, EventArgs eventArgs)
    {
        Destroy(gameObject);
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
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Collision with player " + gameObject.GetComponent<Entity>().isLocalPlayer);

        }
    }
}
