using System.Collections;
using System.Collections.Generic;
using Mirror;
using Org.BouncyCastle.Asn1.X509.Qualified;
using UnityEngine;

public class LyraAbility : PlayerAbility
{
    bool abilityStarted;
    [SerializeField]
    GameObject lyraCursorGO;
    GameObject currentLyraCursorGO;
    LyraCursor lyraCursor;

    public void Start()
    {
        if (!isLocalPlayer)
        {
            return;
        }
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
    }

    private void StopAbility()
    {
        if (currentLyraCursorGO.activeInHierarchy)
        {
            abilityStarted = false;
            lyraCursor.OnStopAbility();
            currentLyraCursorGO.SetActive(false);
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
