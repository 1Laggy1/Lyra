using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class SpawnManager : NetworkBehaviour
{
    void Start()
    {
        if (!isServer)
        {
            enabled = false;

            return;
        }

    }
    public GameObject Spawn(GameObject toSpawn, Vector2 where)
    {
        GameObject go = Instantiate(toSpawn, where, Quaternion.identity);
        NetworkServer.Spawn(go);
        return go;
    }
}
