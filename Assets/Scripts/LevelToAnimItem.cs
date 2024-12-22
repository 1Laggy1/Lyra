using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Mirror;
using UnityEngine;

public class LevelToAnimItem : IUseable
{
    [SerializeField]
    Animation animationObject;
    [SerializeField]
    Animation thisAnimation;

    [SerializeField]
    AnimationClip openObject;

    [SerializeField]
    AnimationClip closeObject;
    [SerializeField]
    AnimationClip onOpen;

    [SerializeField]
    AnimationClip onClose;
    [SyncVar]
    bool opened;

    [SyncVar]
    bool canBeOpened;
    void Start()
    {
        if (thisAnimation != null && onOpen != null)
        {
            thisAnimation.AddClip(onOpen, "open");
            thisAnimation.AddClip(onClose, "close");
        }
        animationObject.AddClip(openObject, "open");
        animationObject.AddClip(closeObject, "close");
    }
    [Command(requiresAuthority = false)]
    public override void Use()
    {
        if (animationObject.isPlaying)
        {
            return;
        }
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
        animationObject.Play("open");
        if (thisAnimation != null && onOpen != null)
        {
            thisAnimation.Play("open");
        }
        opened = true;
    }

    void Close()
    {
        animationObject.Play("close");
        if (thisAnimation != null && onClose != null)
        {
            thisAnimation.Play("close");
        }
        opened = false;
    }
}
