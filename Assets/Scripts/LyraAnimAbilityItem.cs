using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.Video;

public class LyraAnimAbilityItem : ILyraAbilityItem
{
    [SerializeField]
    Animation animationObject;

    [SerializeField]
    AnimationClip openObject;

    [SerializeField]
    AnimationClip closeObject;
    [SyncVar]
    bool opened;

    [SyncVar]
    bool canBeOpened;
    bool loop;
    void Start()
    {
        animationObject.AddClip(openObject, "open");
        animationObject.AddClip(closeObject, "close");
    }

    [ClientRpc]
    public void UseRPC()
    {
        if (!opened)
        {
            Open();
        }
        else
        {
            Close();
        }

        Debug.Log(name + $" opened {opened}");
    }

    void Open()
    {
        animationObject.Play("open");
        opened = true;
    }

    void Close()
    {
        if (!loop)
        {
            return;
        }
        animationObject.Play("close");
        opened = false;
    }
    [Command(requiresAuthority = false)]
    public void UseAbilityCommand()
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
    public override void UseAbility()
    {
        base.UseAbility();
        UseAbilityCommand();
    }
}
