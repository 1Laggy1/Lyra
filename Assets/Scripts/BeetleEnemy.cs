using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class BeetleEnemy : WalkableEnemy
{
    [SyncVar]
    private bool _isFirstDeath = true;
    [SyncVar]
    private bool isReviving = false;

    [ClientRpc]
    public override void DamageRPC(float damage, int fasing)
    {
        if (isReviving) return;
        Health -= damage;
        Rb.AddForce(new Vector2(Knockback * fasing * 10000, Knockback * 3000), ForceMode2D.Force);
        if (Health <= 0)
        {
            if (_isFirstDeath)
            {
                if (isServer)
                    StartCoroutine(FirstDeath());
            }
            else
            {
                Died();
            }

        }
        else
        {
            OnDamaged();
        }
    }
    IEnumerator FirstDeath()
    {
        _isFirstDeath = false;
        isReviving = true;
        Health = 2;
        npcAI.walkingSpeed = 0f;
        yield return new WaitForSeconds(2f);
        isReviving = false;
        npcAI.walkingSpeed = 3f;
    }
}
