using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mirror;
using UnityEngine;

public class CutSceneManager : MonoBehaviour
{
    GameObject Kayden;
    GameObject Lyra;
    Animation KaydenAnim;


    Animation LyraAnim;

    DialogSO currentDialog;
    int dialogNumber = 0;
    public Action OnCutSceneEndedEvent;
    DialogManager dialogManager;

    public void Start()
    {
        StartCoroutine(FindPlayers());
    }

    IEnumerator FindPlayers()
    {
        yield return new WaitUntil(() => GameObject.FindGameObjectWithTag("DialogManager") != null);
        dialogManager = GameObject.FindGameObjectWithTag("DialogManager").GetComponent<DialogManager>();
        yield return new WaitUntil(() => GameObject.FindGameObjectsWithTag("Player").Count() == 2);
        Kayden = GameObject.Find("Kayden(Clone)");
        Lyra = GameObject.Find("Lyra(Clone)");
        KaydenAnim = Kayden.GetComponent<Animation>();
        LyraAnim = Lyra.GetComponent<Animation>();


    }
    IEnumerator OnCutSceneEnded(Animation animationToTrack)
    {
        yield return new WaitUntil(() => animationToTrack.isPlaying == false);
        OnCutSceneEndedEvent.Invoke();
        currentDialog = null;
        dialogNumber = 0;


    }
    public void StartAnim(List<CutSceneObject> objectsToAnim, DialogSO dialog)
    {
        if (dialog != null)
        {
            currentDialog = dialog;
        }
        foreach (CutSceneObject cutSceneObject in objectsToAnim)
        {
            cutSceneObject.AnimClip.legacy = true;
            switch (cutSceneObject.Type)
            {
                case CutSceneObjectType.GO:
                    cutSceneObject.AnimationGO.AddClip(cutSceneObject.AnimClip, cutSceneObject.Name);
                    break;
                case CutSceneObjectType.Kayden:
                    KaydenAnim.AddClip(cutSceneObject.AnimClip, cutSceneObject.Name);
                    break;
                case CutSceneObjectType.Lyra:
                    LyraAnim.AddClip(cutSceneObject.AnimClip, cutSceneObject.Name);
                    break;
            }
        }
        foreach (CutSceneObject cutSceneObject in objectsToAnim)
        {
            switch (cutSceneObject.Type)
            {
                case CutSceneObjectType.GO:
                    cutSceneObject.AnimationGO.Play(cutSceneObject.Name);
                    break;
                case CutSceneObjectType.Kayden:
                    KaydenAnim.Play(cutSceneObject.Name);
                    break;
                case CutSceneObjectType.Lyra:
                    LyraAnim.Play(cutSceneObject.Name);
                    break;
            }
        }
        CutSceneObject longestAnim = objectsToAnim[0];
        foreach (CutSceneObject cutSceneObject in objectsToAnim)
        {
            if (cutSceneObject.AnimClip.length > longestAnim.AnimClip.length)
            {
                longestAnim = cutSceneObject;
            }
        }
        StartCoroutine(OnCutSceneEnded(longestAnim.AnimationGO));
    }
    public void ShowCurrentDialogOfCurrentAnim()
    {
        dialogManager.CutSceneDialog(currentDialog.dialogs[dialogNumber]);
        dialogNumber++;
    }
}
