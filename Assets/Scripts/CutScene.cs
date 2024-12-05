using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
public enum CutSceneObjectType
{
    GO,
    Kayden,
    Lyra
}
[System.Serializable]
public class CutSceneObject
{
    public string Name;

    public CutSceneObjectType Type;

    public Animation AnimationGO;
    public AnimationClip AnimClip;
}
public class CutScene : NetworkBehaviour
{
    CutSceneManager cutSceneManager;
    [SerializeField]
    public List<CutSceneObject> ObjectsToAnim = new List<CutSceneObject>();
    [SerializeField]
    public DialogSO CutSceneDialog;
    public void Start()
    {
        if (cutSceneManager == null)
        {
            cutSceneManager = GameObject.FindGameObjectWithTag("CutSceneManager").GetComponent<CutSceneManager>();
        }
    }
    public virtual void StartAnim()
    {
        cutSceneManager.StartAnim(ObjectsToAnim, CutSceneDialog);
    }

}
