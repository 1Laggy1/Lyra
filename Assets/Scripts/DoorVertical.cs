using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Mirror;
using UnityEngine;

public class DoorVertical : IUseable
{
    [SerializeField]
    BoxCollider2D doorCollider;
    [SyncVar]
    bool opened;

    [SyncVar]
    bool canBeOpened;

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
        if (!opened)
        {
            Open();
        }
        else
        {
            Close();
        }

        Debug.Log($"Opened {opened}");
    }

    void Open()
    {
        doorCollider.isTrigger = true;
        opened = true;
        this.transform.DOMove(transform.position + new Vector3(0, 2, 0), 0.3f, false);
    }

    void Close()
    {
        opened = false;
        this.transform.DOMove(transform.position + new Vector3(0, -2, 0), 0.3f, false);
        doorCollider.isTrigger = false;
    }
}
