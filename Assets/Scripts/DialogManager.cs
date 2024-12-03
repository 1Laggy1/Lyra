using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mirror;
using TMPro;
using UnityEngine;
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
    bool dialogStarted;
    [SerializeField]
    bool skippedByPlayer;
    [SerializeField]
    List<CharactersImages> charactersImages = new List<CharactersImages>();
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
    void ShowDialog(DialogInfo dialogInfo)
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
