using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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
    private CancellationTokenSource currentTextAnimToken;
    bool ended;
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
                ended = false;
                ShowDialog(dialogInfo);
                yield return new WaitUntil(() => ended);//() => playersSkipped == 2 || );
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
        // Зупиняємо попередню анімацію
        currentTextAnimToken?.Cancel();
        currentTextAnimToken = new CancellationTokenSource();

        characterImage.sprite = charactersImages.FirstOrDefault(image => image.Name == dialogInfo.Character.CharacterImageName)?.Image;
        characterName.text = dialogInfo.Character.Name;
        characterText.text = "";
        dialogGO.SetActive(true);

        // Запускаємо нову анімацію
        StartCoroutine(TextAnim(dialogInfo, currentTextAnimToken.Token));
    }
    public IEnumerator TextAnim(DialogInfo dialogInfo, CancellationToken token)
    {
        char[] characters = dialogInfo.Text.ToCharArray();
        string currentText = "";
        int symbol = -1;

        while (!token.IsCancellationRequested)
        {
            yield return new WaitForSeconds(dialogInfo.Speed);
            symbol++;
            if (symbol < characters.Length)
            {
                if (characters[symbol] == '{')
                {
                    string seconds = "";
                    int secondsNumber = 0;
                    while (true)
                    {
                        secondsNumber++;
                        if (int.TryParse(characters[symbol + secondsNumber].ToString(), out int number))
                        {
                            seconds += characters[symbol + secondsNumber];
                        }
                        else
                        {
                            symbol += secondsNumber + 1;
                            int.TryParse(seconds, out int delay);
                            float delayinseconds = (float)delay / 1000f;
                            yield return new WaitForSeconds(delayinseconds);
                            break;
                        }
                    }
                }
                currentText += characters[symbol];
                characterText.text = currentText;
            }
            else
            {
                yield return new WaitForSeconds(dialogInfo.WaitWhenEnded);
                break;
            }
        }
        ended = true;
    }
    [ClientRpc]
    void HideDialog()
    {
        // Скасування анімації при закритті діалогу
        currentTextAnimToken?.Cancel();
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
