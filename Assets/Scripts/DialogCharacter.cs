using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "DialogCharacter", menuName = "Custom/DialogCharacter", order = 3)]
public class DialogCharacter : ScriptableObject
{
    public string CharacterImageName;
    public string Name;
}