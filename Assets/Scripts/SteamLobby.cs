using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Steamworks;
using Mirror.FizzySteam;
using kcp2k;
using Edgegap;

public class SteamLobby : MonoBehaviour
{
    [SerializeField]
    MyNetworkManager networkManager;
    protected Callback<LobbyCreated_t> lobbyCreated;
    protected Callback<GameLobbyJoinRequested_t> gameLobbyJoinRequested;
    protected Callback<LobbyEnter_t> lobbyEnter;
    const string HostAddressKey = "HostAddress";
    [SerializeField]
    GameObject networkManagerPrefab;
    public void HostLobby()
    {
        networkManager = GameObject.Find("NetworkManagers Steam(Clone)").GetComponent<MyNetworkManager>();
        SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypeFriendsOnly, networkManager.maxConnections);
    }

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        if (GameObject.Find("SteamLobby (1)"))
        {
            Destroy(GameObject.Find("SteamLobby (1)"));
        }
        networkManager = GetComponent<MyNetworkManager>();
        if (!SteamManager.Initialized) { return; }
        lobbyCreated = Callback<LobbyCreated_t>.Create(OnLobbyCreated);
        gameLobbyJoinRequested = Callback<GameLobbyJoinRequested_t>.Create(OnGameLobbyJoinRequested);
        lobbyEnter = Callback<LobbyEnter_t>.Create(OnLobbyEnter);
    }

    private void OnLobbyCreated(LobbyCreated_t callback)
    {
        if (callback.m_eResult != EResult.k_EResultOK)
        {
            Destroy(networkManager.gameObject);
            return;
        }

        networkManager.StartHost();

        SteamMatchmaking.SetLobbyData(new CSteamID(callback.m_ulSteamIDLobby), HostAddressKey, SteamUser.GetSteamID().ToString());
    }

    private void OnGameLobbyJoinRequested(GameLobbyJoinRequested_t callback)
    {
        SteamMatchmaking.JoinLobby(callback.m_steamIDLobby);
    }
    private void OnLobbyEnter(LobbyEnter_t callback)
    {
        if (NetworkServer.active) { return; }
        Destroy(GameObject.Find("NetworkManagers KCP(clone)"));
        networkManager = Instantiate(networkManagerPrefab).GetComponent<MyNetworkManager>();
        string hostAddress = SteamMatchmaking.GetLobbyData(new CSteamID(callback.m_ulSteamIDLobby), HostAddressKey);
        networkManager.networkAddress = hostAddress;
        networkManager.StartClient();
        networkManager.networkAddress = hostAddress;
    }
}
