using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using kcp2k;
using Mirror;
using Mirror.FizzySteam;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MyNetworkManager : NetworkManager
{
    string characterPicked;

    [SerializeField]
    GameObject kaydenPref;

    [SerializeField]
    GameObject lyraPref;
    [SerializeField]
    SteamLobby steamLobby;
    bool firstClient = true;

    public async void StartManager()
    {
        switch (NetworkManagerConfig.Transport)
        {
            case "IP":
                //transport = GetComponent<KcpTransport>();
                if (!NetworkManagerConfig.IsClient)
                {
                    StartHost();
                }
                else
                {
                    StartClient();
                    this.networkAddress = string.IsNullOrEmpty(NetworkManagerConfig.IP) ? "127.0.0.1" : NetworkManagerConfig.IP;
                }

                break;
            case "Steam":
                //this.transport = gameObject.GetComponent<FizzySteamworks>();
                steamLobby = GameObject.Find("SteamLobby").GetComponent<SteamLobby>();
                steamLobby.HostLobby();
                break;
        }
    }

    public override void Start()
    {
        base.Start();
        if (GameObject.Find("NetworkManagers (1)") != null)
        {
            Destroy(GameObject.Find("NetworkManagers (1)"));
        }
        steamLobby = GameObject.Find("SteamLobby").GetComponent<SteamLobby>();
        NetworkServer.RegisterHandler<ConnectMessage>(OnCreateCharacter);
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        ServerChangeScene("AndrewScene"); // Використовуємо ServerChangeScene для зміни сцени на сервері

    }

    public override void OnClientConnect()
    {
        base.OnClientConnect();

        // Надсилаємо вибір персонажа після підключення
        ConnectMessage characterMessage = new ConnectMessage
        {
            Message = NetworkManagerConfig.Character
        };
        if (NetworkServer.active)
        {
            StartCoroutine(SpawnHostPlayer());
        }
        else
        {
            NetworkClient.Send(characterMessage);
        }

        firstClient = false;
    }

    void OnCreateCharacter(NetworkConnectionToClient conn, ConnectMessage message)
    {
        GameObject gameobject = (message.Message == "Kayden") ? Instantiate(kaydenPref) : Instantiate(lyraPref);
        NetworkServer.AddPlayerForConnection(conn, gameobject);
    }

    IEnumerator SpawnHostPlayer()
    {
        yield return new WaitUntil(() => SceneManager.GetActiveScene().name == "AndrewScene");
        GameObject gameobject = (NetworkManagerConfig.Character == "Kayden") ? Instantiate(kaydenPref) : Instantiate(lyraPref);
        NetworkServer.AddPlayerForConnection(NetworkServer.connections[0], gameobject);
    }
    public override void OnApplicationQuit()
    {
        transport.Shutdown();
        try
        {
            StopClient();
        }
        catch { }
        try
        {
            StopServer();
        }
        catch { }
    }
}
