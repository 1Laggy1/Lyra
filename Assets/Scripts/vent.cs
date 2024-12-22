using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Mirror;
using UnityEngine;
using UnityEngine.Video;

public class vent : ILyraAbilityItem
{

    [SerializeField]
    DialogSO dialogAfterFight;

    [SerializeField]
    SpawnInTime spawnInTime;
    [SerializeField]
    SpawnManager sm;

    [SerializeField]
    DialogManager dm;
    bool first = true;
    [SyncVar]
    public bool canBeUsed = false;
    [SerializeField]
    DialogSO enableAfterDialog;
    public override void Start()
    {
        base.Start();
        sm.SpawnEndedEvent += FightEnded;
        dm.DialogEndedEvent += CanBeUsed;
    }
    public override void UseAbility()
    {
        if (first && canBeUsed)
        {
            base.UseAbility();
            if (isServer)
            {
                StartSpawnRPC();
            }
            else
            {
                StartSpawnCommand();
            }

            VentAnimCommand();

            first = false;
        }


    }
    [Command(requiresAuthority = false)]
    public void StartSpawnCommand()
    {
        sm.StartSpawning(spawnInTime);
    }
    [ClientRpc]
    public void StartSpawnRPC()
    {
        sm.StartSpawning(spawnInTime);
    }
    [Command(requiresAuthority = false)]
    public void VentAnimCommand()
    {
        VentAnimRPC();
    }
    [ClientRpc]
    public void VentAnimRPC()
    {
        transform.DORotate(new Vector3(0, 0, 30), 2, RotateMode.Fast);
    }
    public void FightEnded(SpawnInTime spawn)
    {
        if (spawn.Name == spawnInTime.Name)
        {
            if (isServer)
            {
                dm.StartDialogQueueServerOnly(dialogAfterFight);
            }
        }
    }
    public void CanBeUsed(DialogSO dialogSO)
    {
        if (!isServer)
        {
            return;
        }
        if (dialogSO.Name == enableAfterDialog.Name)
        {
            canBeUsed = true;
        }
        if (dialogSO.Name == dialogAfterFight.Name)
        {
            PlayerPrefs.SetString("LastSceneName", "Level1");
            NetworkManager.singleton.ServerChangeScene("Level1");

        }
    }
    public void DialogEnded(DialogSO dialogSO)
    {

    }
}