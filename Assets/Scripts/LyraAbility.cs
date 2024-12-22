using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using Org.BouncyCastle.Asn1.X509.Qualified;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LyraAbility : PlayerAbility
{
    bool abilityStarted;
    [SerializeField]
    GameObject lyraCursorGO;
    GameObject currentLyraCursorGO;
    LyraCursor lyraCursor;
    public Action<bool> AbilityActivatedBool;
    public void Start()
    {
        AbilityActivatedBool = null;
        SceneManager.activeSceneChanged += SceneChanged;
        if (!isLocalPlayer)
        {
            return;
        }
    }
    public void SceneChanged(Scene old, Scene New)
    {
        AbilityActivatedBool = null;
    }
    public override void UseAbility()
    {
        if (!isLocalPlayer)
        {
            return;
        }
        if (currentLyraCursorGO == null)
        {
            SpawnCursorOnServer();
        }
        else if (!abilityStarted && lyraCursor != null)
        {
            StartAbility();
        }
        else if (lyraCursor != null)
        {
            StopAbility();
        }
    }

    [Command(requiresAuthority = false)]
    public void SpawnCursorOnServer()
    {
        if (isServer)
        {
            currentLyraCursorGO = Instantiate(lyraCursorGO);
            NetworkServer.Spawn(currentLyraCursorGO, this.gameObject);
            SpawnCursorOnLocal(currentLyraCursorGO);
        }
    }

    [ClientRpc]
    public void SpawnCursorOnLocal(GameObject spawnedCursor)
    {
        if (!isLocalPlayer)
        {
            return;
        }

        currentLyraCursorGO = spawnedCursor;
        lyraCursor = currentLyraCursorGO.GetComponent<LyraCursor>();
        StartAbility();
    }

    private void StartAbility()
    {
        abilityStarted = true;
        currentLyraCursorGO.SetActive(true);
        lyraCursor.OnStartAbility();
        lyraCursor.transform.position = transform.position;

        AbilityActivatedBool?.Invoke(true);

    }

    private void StopAbility()
    {
        if (currentLyraCursorGO.activeInHierarchy)
        {
            abilityStarted = false;
            lyraCursor.OnStopAbility();
            currentLyraCursorGO.SetActive(false);
            AbilityActivatedBool?.Invoke(false);


        }
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1") && abilityStarted && lyraCursor.LyraAbilityItem != null)
        {
            lyraCursor.LyraAbilityItem.UseAbility();
        }
    }
}
