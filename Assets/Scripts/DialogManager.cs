using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DialogManager : NetworkBehaviour
{
    [SerializeField]
    GameObject dialogGO;


    [SerializeField]
    Image characterImage;
    [SerializeField]
    TMP_Text characterName;
    [SerializeField]
    TMP_Text characterText;
    [SerializeField]
    [SyncVar]
    int playersSkipped;
    List<DialogSO> dialogsQueue = new List<DialogSO>();
    [SyncVar]
    public bool dialogStarted;
    [SerializeField]
    bool skippedByPlayer;
    [SerializeField]
    List<CharactersImages> charactersImages = new List<CharactersImages>();
    public event Action<DialogSO> DialogEndedEvent;
    [Command(requiresAuthority = false)]
    public void StartDialogQueueCommand(DialogSO dialog)
    {
        StartDialogQueueServerOnly(dialog);
    }
    public void StartDialogQueueServerOnly(DialogSO dialog)
    {
        if (!isServer) return;
        if (!dialogsQueue.Contains(dialog))
            dialogsQueue.Add(dialog);
        if (dialogsQueue[0] == dialog && !dialogStarted)
        {
            dialogStarted = true;
            StartCoroutine(StartDialogAsync());
        }
    }

    IEnumerator StartDialogAsync()
    {
        while (dialogsQueue.Count != 0)
        {
            foreach (DialogInfo dialogInfo in dialogsQueue[0].dialogs)
            {
                playersSkipped = 0;
                skippedByPlayer = false;
                ShowDialog(dialogInfo);
                yield return new WaitUntil(() => playersSkipped == 2);
                HideDialog();
            }
            if (DialogEndedEvent != null)
                DialogEndedEvent.Invoke(dialogsQueue[0]);
            dialogsQueue.Remove(dialogsQueue[0]);
        }
        dialogStarted = false;
    }
    [Command(requiresAuthority = false)]
    void Skip()
    {
        playersSkipped++;
    }
    [ClientRpc]
    public void ShowDialog(DialogInfo dialogInfo)
    {
        characterImage.sprite = charactersImages.FirstOrDefault(image => image.Name == dialogInfo.Character.CharacterImageName).Image;
        characterName.text = dialogInfo.Character.Name;
        characterText.text = dialogInfo.Text;
        dialogGO.SetActive(true);
    }

    [ClientRpc]
    void HideDialog()
    {
        skippedByPlayer = false;
        dialogGO.SetActive(false);
    }
    public void CutSceneDialog(DialogInfo dialogInfo)
    {
        if (isServer)
            ShowDialog(dialogInfo);
    }
    public void HideCutSceneDialog()
    {
        if (isServer)
            HideDialog();
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonUp("SkipDialog"))
        {
            if (!skippedByPlayer && dialogStarted)
            {
                skippedByPlayer = true;
                if (isServer)
                {
                    playersSkipped++;
                }
                else
                {
                    Skip();
                }

            }
        }
    }
}
