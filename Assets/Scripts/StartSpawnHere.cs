using System;
using System.Collections;
using System.Collections.Generic;
using Codice.Client.BaseCommands.TubeClient;
using Mirror;
using UnityEngine;

public class StartSpawnHere : NetworkBehaviour
{
    bool isSpawning;
    bool firstSpawn = true;
    bool loop;
    
    [SerializeField]
    SpawnInTime spawnInTime;
    SpawnManager sm;

    void Start()
    {
        if (!isServer)
        {
            return;
        }
        sm = GameObject.FindGameObjectWithTag("SpawnManager").GetComponent<SpawnManager>();
    }


    public void OnTriggerEnter2D(Collider2D other)
    {
        if (isServer && other.gameObject.tag == "Player" && !isSpawning && (loop || firstSpawn))
        {
            firstSpawn = false;
            isSpawning = true;
            sm.StartSpawning(spawnInTime);
            sm.SpawnEndedEvent += SpawnEnded;

        }
    }
    public void SpawnEnded(SpawnInTime spawn)
    {
        if (spawn == spawnInTime)
        {
            isSpawning = false;
        }
    }

}
