using System.Collections;
using System.Collections.Generic;
using Codice.Client.BaseCommands.TubeClient;
using Mirror;
using UnityEngine;

public class RockFalling : Rock
{
    public override void Begin()
    {
        base.Begin();


        if (!isFalling && canActivate)
        {
            isFalling = true;
            rb.isKinematic = false;
            canActivate = false;
            StartCoroutine(WaitForSecond());
        }

    }
    IEnumerator WaitForSecond()
    {
        GetComponent<Collider2D>().isTrigger = true;
        yield return new WaitForSeconds(0.3f);
        GetComponent<Collider2D>().isTrigger = false;
        timeToCheck = true;
        yield return new WaitForSeconds(cooldown);
        if (respawnable)
        {
            rb.isKinematic = true;
            isFalling = false;
            timeToCheck = false;
            transform.position = startPostion;
            entitiesAttacked = new List<GameObject>();
            canActivate = true;

        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
