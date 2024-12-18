using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class Notepad : ILyraAbilityItem
{
    [SerializeField]
    List<DialogSO> dialogSOs = new List<DialogSO>();

    int currentDialog = 0;


    [SerializeField]
    DialogManager dm;
    public override void UseAbility()
    {
        if (dm.dialogStarted)
        {
            return;
        }
        base.UseAbility();
        if (isServer)
        {
            dm.StartDialogQueueServerOnly(dialogSOs[currentDialog]);
        }
        else
        {
            dm.StartDialogQueueCommand(dialogSOs[currentDialog]);
        }
        currentDialog++;
    }
    void Start()
    {
        dm = GameObject.FindGameObjectWithTag("DialogManager").GetComponent<DialogManager>();
    }
}
