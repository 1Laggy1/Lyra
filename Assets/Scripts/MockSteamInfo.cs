using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MockSteamInfo : SteamInfo
{
    public override void Awake()
    {
        OnAwake();
    }

    public override string GetSelfSteamUsername()
    {
        return "Player username test";
    }

    public override int GetFriendsInGame()
    {
        return 10;
    }

    public override Texture2D GetSelfAvatar()
    {
        return Texture2D.blackTexture;
    }
}
