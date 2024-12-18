using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class SpawnManager : NetworkBehaviour
{



    int spawnedGos = 0;

    [SerializeField]
    AudioSource audioSource;
    [SerializeField]
    AudioClip enemiesSpawnAudio;
    public event Action<SpawnInTime> SpawnEndedEvent;
    void Start()
    {
        if (!isServer)
        {

            return;
        }

    }
    public GameObject Spawn(GameObject toSpawn, Vector2 where)
    {
        GameObject go = Instantiate(toSpawn, where, Quaternion.identity);
        NetworkServer.Spawn(go);
        return go;
    }
    public void StartSpawning(SpawnInTime spawnInTime)
    {
        if (!isServer)
        {

            return;
        }
        StartCoroutine(Spawning(spawnInTime));
    }
    IEnumerator Spawning(SpawnInTime spawnInTime)
    {
        foreach (SpawnOneAttack soa in spawnInTime.WhatToSpawn)
        {
            yield return new WaitForSeconds(soa.Time);
            SpawnAudio();
            foreach (AttackInfo attackInfo in soa.attackInfos)
            {
                for (int i = 1; i <= attackInfo.Amount; i++)
                {
                    GameObject go = Spawn(attackInfo.Go, attackInfo.Spawnpoint);
                    spawnedGos++;
                    go.GetComponent<Entity>().EntityDied += EnemyDies;
                }
            }

            if (!spawnInTime.Haos)
                yield return new WaitUntil(() => spawnedGos == 0);
        }
        SpawnEndedEventInvoke(spawnInTime);
        yield return null;
    }

    [Command(requiresAuthority = false)]
    public void SpawnAudio()
    {
        SpawnAudioClientRPC();
    }
    [ClientRpc]
    public void SpawnAudioClientRPC()
    {
        Debug.Log("SpawnAudio");
        if (PlayerPrefs.GetFloat("Volume_effects") != 0)
            audioSource.volume = PlayerPrefs.GetFloat("Volume_effects");
        audioSource.PlayOneShot(enemiesSpawnAudio);
    }
    [ClientRpc]
    public void SpawnEndedEventInvoke(SpawnInTime spawnInTime)
    {
        if (SpawnEndedEvent != null)
            SpawnEndedEvent.Invoke(spawnInTime);
    }
    public void EnemyDies(object sender, EventArgs eventArgs)
    {
        spawnedGos--;
    }
}
