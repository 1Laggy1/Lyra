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

    PlayerMovement KaydenMovement;
    PlayerMovement LyraMovement;
    Animation LyraAnim;

    DialogSO currentDialog;
    int dialogNumber = 0;
    public Action OnCutSceneEndedEvent;
    DialogManager dialogManager;
    CameraMode previousCameraMode;
    CameraMovement cameraMovement;

    public void Start()
    {
        StartCoroutine(FindPlayers());
        cameraMovement = Camera.main.GetComponent<CameraMovement>();
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
        LyraMovement = Lyra.GetComponent<PlayerMovement>();
        KaydenMovement = Kayden.GetComponent<PlayerMovement>();


    }
    IEnumerator OnCutSceneEnded(Animation animationToTrack)
    {
        yield return new WaitUntil(() => animationToTrack.isPlaying == false);
        if (OnCutSceneEndedEvent != null)
            OnCutSceneEndedEvent.Invoke();
        currentDialog = null;
        dialogNumber = 0;
        LyraMovement.SetInputProvider(new InputProvider());
        KaydenMovement.SetInputProvider(new InputProvider());
        LyraMovement.isAnim = false;
        KaydenMovement.isAnim = false;

        if (Camera.main.GetComponent<CameraMovement>().ChangedMode)
        {
            Camera.main.GetComponent<CameraMovement>().ChangedMode = false;
        }
        else
        {
            switch (previousCameraMode)
            {
                case CameraMode.Dynamic:
                    cameraMovement.SetDynamicMode();
                    break;
                case CameraMode.Static:
                    cameraMovement.SetStaticMode();
                    break;
                case CameraMode.None:
                    cameraMovement.SetNoneMode();
                    break;
            }
        }


    }
    public void StartAnim(List<CutSceneObject> objectsToAnim, DialogSO dialog)
    {
        if (dialog != null)
        {
            currentDialog = dialog;
        }
        if (LyraAnim == null)
        {
            Kayden = GameObject.Find("Kayden(Clone)");
            Lyra = GameObject.Find("Lyra(Clone)");
            KaydenAnim = Kayden.GetComponent<Animation>();
            LyraAnim = Lyra.GetComponent<Animation>();
            LyraMovement = Lyra.GetComponent<PlayerMovement>();
            KaydenMovement = Kayden.GetComponent<PlayerMovement>();
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
                    KaydenMovement.isAnim = true;
                    KaydenAnim.AddClip(cutSceneObject.AnimClip, cutSceneObject.Name);
                    KaydenMovement.SetInputProvider(new CutSceneInputProvider());
                    break;
                case CutSceneObjectType.Lyra:
                    LyraMovement.isAnim = true;
                    LyraAnim.AddClip(cutSceneObject.AnimClip, cutSceneObject.Name);
                    LyraMovement.SetInputProvider(new CutSceneInputProvider());
                    break;
                case CutSceneObjectType.Camera:
                    cutSceneObject.AnimationGO.AddClip(cutSceneObject.AnimClip, cutSceneObject.Name);
                    previousCameraMode = cutSceneObject.AnimationGO.gameObject.GetComponent<CameraMovement>().curretMode;
                    cameraMovement.SetNoneMode();
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
                case CutSceneObjectType.Camera:
                    cutSceneObject.AnimationGO.Play(cutSceneObject.Name);
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
        if (dialogNumber < currentDialog.dialogs.Count())
        {
            dialogManager.CutSceneDialog(currentDialog.dialogs[dialogNumber]);
        }
        else
        {
            dialogManager.HideCutSceneDialog();
        }
        dialogNumber++;
    }
}
