using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[System.Serializable]
public class CharactersImages
{
    public string Name;
    public Sprite Image;
}
[System.Serializable]
public class DialogInfo
{


    public DialogCharacter Character;
    public string Text;
    public float Speed = 0.05f;
    public float WaitWhenEnded = 3f;
}
[CreateAssetMenu(fileName = "Dialog", menuName = "Custom/Dialog", order = 2)]
public class DialogSO : ScriptableObject
{
    public string Name;
    public List<DialogInfo> dialogs = new List<DialogInfo>();
}