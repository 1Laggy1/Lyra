using System.Collections;
using System.Collections.Generic;
using Codice.Client.BaseCommands.TubeClient;
using Mirror;
using UnityEngine;

public class RockThrowing : Rock
{
    [SerializeField]
    Vector2 whereToThrow;
    public virtual void Begin()
    {

        base.Begin();
        isFalling = true;
        rb.simulated = true;

        rb.AddForce(whereToThrow, ForceMode2D.Force);
    }
}
