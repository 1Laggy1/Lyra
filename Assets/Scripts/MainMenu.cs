using System.Collections;
using System.Collections.Generic;
using Mono.CecilX.Cil;
using TMPro;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject Main;
    public GameObject ServerType;
    public GameObject ChooseTypeOfClient;
    public GameObject ChooseCharacter;
    [SerializeField]
    TMP_Text ip_Text;
    GameObject networkManagerGO;
    MyNetworkManager networkManager;
    const string ServerTypeValue = "ServerType";
    const string ClientType = "ClientType";
    const string CharacterType = "CharacterType";
    string serverType;
    string clientType;
    void Start()
    {
        networkManagerGO = GameObject.Find("NetworkManagers");
        networkManager = networkManagerGO.GetComponent<MyNetworkManager>();
    }
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
        NetworkManagerConfig.Transport = "IP";
        PlayerPrefs.SetString(ServerTypeValue, "IP");
        ServerType.SetActive(false);
        ChooseCharacter.SetActive(true);
    }

    public void UseSteam()
    {
        NetworkManagerConfig.Transport = "Steam";
        PlayerPrefs.SetString(ServerTypeValue, "Steam");
        ServerType.SetActive(false);
        ChooseCharacter.SetActive(true);
    }

    public void Host()
    {
        NetworkManagerConfig.IsClient = false;
        PlayerPrefs.SetString(ClientType, "Host");
        clientType = "Host";
        ChooseTypeOfClient.SetActive(false);
        ServerType.SetActive(true);
    }

    public void Client()
    {
        NetworkManagerConfig.IsClient = true;
        PlayerPrefs.SetString(ClientType, "Client");
        clientType = "Client";
        ChooseTypeOfClient.SetActive(false);
        ServerType.SetActive(true);

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
        PlayerPrefs.SetString(CharacterType, "Lyra");
        NetworkManagerConfig.IP = ip_Text.text;
        networkManager.StartManager();
    }

    public void ChooseKayden()
    {
        PlayerPrefs.SetString(CharacterType, "Kayden");
        NetworkManagerConfig.IP = ip_Text.text;
        networkManager.StartManager();
    }
}
