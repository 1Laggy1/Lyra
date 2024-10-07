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
    string key = "ServerType";

    public void Play()
    {
        Main.SetActive(false);
        ServerType.SetActive(true);
    }

    public void Exit()
    {
        Application.Quit();
    }

    void UseIp()
    {
        PlayerPrefs.SetString(key, "IP");
    }

    void UseSteam()
    {
        PlayerPrefs.SetString(key, "Steam");
    }
}
