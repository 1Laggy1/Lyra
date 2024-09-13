using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class MyNetworkManager : NetworkManager
{
    string characterPicked;

    [SerializeField]
    GameObject KaydenPref;

    [SerializeField]
    GameObject LyraPref;

    public override void OnStartServer()
    {
        base.OnStartServer();

        NetworkServer.RegisterHandler<ConnectMessage>(OnCreateCharacter);
    }

    public override void OnServerConnect(NetworkConnectionToClient conn)
    {
        base.OnServerConnect(conn);
    }

    public override void OnClientConnect()
    {
        base.OnClientConnect();

        // you can send the message here, or wherever else you want
        ConnectMessage characterMessage = new ConnectMessage();
        characterMessage.message = PlayerPrefs.GetString("Character");

        NetworkClient.Send(characterMessage);
    }

    void OnCreateCharacter(NetworkConnectionToClient conn, ConnectMessage message)
    {
        // playerPrefab is the one assigned in the inspector in Network
        // Manager but you can use different prefabs per race for example
        GameObject gameobject;
        if (message.message == "Kayden")
        {
            gameobject = Instantiate(KaydenPref);
        }
        else
        {
            gameobject = Instantiate(LyraPref);
        }

        // call this to use this gameobject as the primary controller
        NetworkServer.AddPlayerForConnection(conn, gameobject);
    }
}
