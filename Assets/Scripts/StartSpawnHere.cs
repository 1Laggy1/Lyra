using System;
using System.Collections;
using System.Collections.Generic;
using Codice.Client.BaseCommands.TubeClient;
using Mirror;
using UnityEngine;

public class StartSpawnHere : NetworkBehaviour
{
    [SerializeField]
    SpawnInTime spawnInTime;
    SpawnManager sm;
    bool isSpawning;
    [SerializeField]
    bool loop;
    bool firstSpawn = true;

    int spawnedGos = 0;
    void Start()
    {
        if (!isServer)
        {
            this.enabled = false;
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
            StartCoroutine(Spawning());

        }
    }
    IEnumerator Spawning()
    {
        foreach (SpawnOneAttack soa in spawnInTime.WhatToSpawn)
        {
            yield return new WaitForSeconds(soa.Time);
            foreach (AttackInfo attackInfo in soa.attackInfos)
            {
                for (int i = 1; i <= attackInfo.Amount; i++)
                {
                    GameObject go = sm.Spawn(attackInfo.Go, attackInfo.Spawnpoint);
                    spawnedGos++;
                    go.GetComponent<Entity>().EntityDied += EnemyDies;
                }
            }

            if (!spawnInTime.Haos)
                yield return new WaitUntil(() => spawnedGos == 0);
        }
        isSpawning = false;
        yield return null;
    }

    public void EnemyDies(object sender, EventArgs eventArgs)
    {
        spawnedGos--;
    }
}
