using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class Door : IUseable
{
    [SerializeField]
    BoxCollider2D doorCollider;

    [Command(requiresAuthority = false)]
    public override void Use()
    {
        if (isServer)
        {
            UseRPC();
        }
    }

    [ClientRpc]
    public override void UseRPC()
    {
        doorCollider.isTrigger = doorCollider.isTrigger ? false : true;
        Debug.Log($"Opened {doorCollider.isTrigger}");
    }
}
