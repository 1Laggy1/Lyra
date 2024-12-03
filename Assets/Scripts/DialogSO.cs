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
}
[CreateAssetMenu(fileName = "Dialog", menuName = "Custom/Dialog", order = 2)]
public class DialogSO : ScriptableObject
{

    public List<DialogInfo> dialogs = new List<DialogInfo>();
}