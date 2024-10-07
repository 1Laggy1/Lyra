using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class IUseable : NetworkBehaviour
{
    [Command(requiresAuthority = false)]
    public virtual void Use()
    {
        UseRPC();
    }

    [ClientRpc]
    public virtual void UseRPC()
    {
    }
}
