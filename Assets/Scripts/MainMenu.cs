using System.Collections;
using System.Collections.Generic;
using Mono.CecilX.Cil;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject Main;
    public GameObject ServerType;
    public GameObject ChooseTypeOfClient;
    public GameObject ChooseCharacter;
    string serverType = "ServerType";
    string clientType = "ClientType";
    string characterType = "CharacterType";

    public void Play()
    {
        Main.SetActive(false);
        ChooseTypeOfClient.SetActive(true);
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void UseIp()
    {
        PlayerPrefs.SetString(serverType, "IP");
        ServerType.SetActive(false);
        ChooseCharacter.SetActive(true);

    }

    public void UseSteam()
    {
        PlayerPrefs.SetString(serverType, "Steam");
        ServerType.SetActive(false);
        ChooseCharacter.SetActive(true);
    }

    public void Host()
    {
        PlayerPrefs.SetString(clientType, "Host");
        ChooseTypeOfClient.SetActive(false);
        ServerType.SetActive(true);

    }

    public void Client()
    {
        PlayerPrefs.SetString(clientType, "Client");
        SceneManager.LoadScene(1);
    }

    public void BackToMain()
    {
        Main.SetActive(true);
        ChooseTypeOfClient.SetActive(false);
    }

    public void BackToServerType()
    {
        ServerType.SetActive(false);
        ChooseTypeOfClient.SetActive(true);
    }

    public void BackToChooseServerType()
    {
        ChooseCharacter.SetActive(false);
        ServerType.SetActive(true);
    }

    public void ChooseLyra()
    {
        PlayerPrefs.SetString(characterType, "Lyra");
        SceneManager.LoadScene(1);
    }

    public void ChooseKayden()
    {
        PlayerPrefs.SetString(characterType, "Kayden");
        SceneManager.LoadScene(1);
    }

}
