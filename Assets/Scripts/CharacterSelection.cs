using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelection : MonoBehaviour
{
    string characterName;

    // Start is called before the first frame update
    void Start() { }

    // Update is called once per frame
    void Update() { }

    public void OnTextChanged(string character)
    {
        ChangeCharacter(character);
    }

    public string ChangeCharacter(string character)
    {
        PlayerPrefs.SetString("Character", character);
        characterName = character;
        return characterName;
    }
}



