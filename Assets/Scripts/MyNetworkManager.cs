using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MyNetworkManager : NetworkManager
{
    string characterPicked;

    [SerializeField]
    GameObject kaydenPref;

    [SerializeField]
    GameObject lyraPref;

    public async void StartManager()
    {
        if (!NetworkManagerConfig.IsClient)
        {
            StartHost();
        }

        switch (NetworkManagerConfig.Transport)
        {
            case "IP":
                break;
            case "Steam":
                break;
        }

        StartClient();
        this.networkAddress = string.IsNullOrEmpty(NetworkManagerConfig.IP) ? "127.0.0.1" : NetworkManagerConfig.IP;
    }

    public override void Start()
    {
        base.Start();
        if (GameObject.Find("NetworkManagers (1)") != null)
        {
            Destroy(GameObject.Find("NetworkManagers (1)"));
        }
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        ServerChangeScene("AndrewScene"); // Використовуємо ServerChangeScene для зміни сцени на сервері
        NetworkServer.RegisterHandler<ConnectMessage>(OnCreateCharacter);
    }

    public override void OnClientConnect()
    {
        base.OnClientConnect();

        // Надсилаємо вибір персонажа після підключення
        ConnectMessage characterMessage = new ConnectMessage
        {
            Message = PlayerPrefs.GetString("Character")
        };
        NetworkClient.Send(characterMessage);
    }

    void OnCreateCharacter(NetworkConnectionToClient conn, ConnectMessage message)
    {
        GameObject gameobject = (message.Message == "Kayden") ? Instantiate(kaydenPref) : Instantiate(lyraPref);
        NetworkServer.AddPlayerForConnection(conn, gameobject);
    }
}
