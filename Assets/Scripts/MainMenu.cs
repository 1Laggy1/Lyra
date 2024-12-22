using Mirror;
using TMPro;
using UnityEngine;

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
    public GameObject networkManagerKCPPrefab;
    public GameObject networkManagerSteamPrefab;
    string lastSceneName;
    [SerializeField]
    TMP_Text lastSceneNametxt;

    void Start()
    {
        GameObject nm = GameObject.FindGameObjectWithTag("NetworkManager");
        if (nm != null)
        {
            NetworkManager networkManager = nm.GetComponent<NetworkManager>();

            // Перевірка і зупинка сервера
            if (networkManager.isNetworkActive)
            {
                if (NetworkServer.active)
                {
                    networkManager.StopServer();
                }

                // Перевірка і зупинка хоста
                if (NetworkClient.isConnected && NetworkServer.active)
                {
                    networkManager.StopHost();
                }
                // Перевірка і зупинка клієнта
                else if (NetworkClient.isConnected)
                {
                    networkManager.StopClient();
                }
            }

            // Завершення роботи транспорту
            Transport transport = nm.GetComponent<Transport>();
            if (transport != null)
            {
                transport.Shutdown();
            }

            // Знищення NetworkManager після завершення всіх процесів
            //Destroy(nm);
        }
        lastSceneName = PlayerPrefs.GetString("LastSceneName");
        lastSceneNametxt.text = lastSceneName;
    }


    public void Play()
    {
        NetworkManagerConfig.CurrentSceneLoading = "Level0";
        Main.SetActive(false);
        ChooseTypeOfClient.SetActive(true);
    }
    public void Continue()
    {
        NetworkManagerConfig.CurrentSceneLoading = lastSceneName;
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
        GameObject nm = GameObject.FindGameObjectWithTag("NetworkManager");
        if (nm != null)
        {
            Destroy(nm);
        }
        PlayerPrefs.SetString(CharacterType, "Lyra");
        NetworkManagerConfig.Character = "Lyra";
        NetworkManagerConfig.IP = ip_Text.text;
        if (NetworkManagerConfig.Transport == "IP")
        {
            networkManager = Instantiate(networkManagerKCPPrefab).GetComponent<MyNetworkManager>();
        }
        else
        {
            networkManager = Instantiate(networkManagerSteamPrefab).GetComponent<MyNetworkManager>();
        }

        networkManager.StartManager();
    }

    public void ChooseKayden()
    {
        GameObject nm = GameObject.FindGameObjectWithTag("NetworkManager");
        if (nm != null)
        {
            Destroy(nm);
        }
        PlayerPrefs.SetString(CharacterType, "Kayden");
        NetworkManagerConfig.Character = "Kayden";
        NetworkManagerConfig.IP = ip_Text.text;
        if (NetworkManagerConfig.Transport == "IP")
        {
            networkManager = Instantiate(networkManagerKCPPrefab).GetComponent<MyNetworkManager>();
        }
        else
        {
            networkManager = Instantiate(networkManagerSteamPrefab).GetComponent<MyNetworkManager>();
        }
        networkManager.StartManager();
    }
}
