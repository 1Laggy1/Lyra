using Steamworks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SteamInfo : MonoBehaviour
{
    public RawImage SteamImage;
    public TMP_Text UsernameText;
    public TMP_Text FriendsInGameText;

    public virtual void Awake()
    {
        if (!SteamManager.Initialized)
        {
            return;
        }

        OnAwake();
    }

    public void OnAwake()
    {
        UsernameText.text = GetSelfSteamUsername();
        SteamImage.texture = GetSelfAvatar();
        FriendsInGameText.text = FriendsInGameText.text + GetFriendsInGame().ToString();
    }

    public virtual string GetSelfSteamUsername()
    {
        return SteamFriends.GetPersonaName();
    }

    public virtual Texture2D GetSelfAvatar()
    {
        int avatarInt = SteamFriends.GetLargeFriendAvatar(SteamUser.GetSteamID());
        if (avatarInt != -1)
        {
            uint width, height;
            SteamUtils.GetImageSize(avatarInt, out width, out height);

            if (width > 0 && height > 0)
            {
                byte[] avatarBytes = new byte[4 * (int)width * (int)height];
                SteamUtils.GetImageRGBA(avatarInt, avatarBytes, avatarBytes.Length);

                Texture2D avatarTexture = new Texture2D((int)width, (int)height, TextureFormat.RGBA32, false);
                avatarTexture.LoadRawTextureData(avatarBytes);
                avatarTexture.Apply();

                return avatarTexture;
            }
        }

        return Texture2D.whiteTexture;
    }

    public virtual int GetFriendsInGame()
    {
        int friendCount = SteamFriends.GetFriendCount(EFriendFlags.k_EFriendFlagImmediate);
        int friendsInGame = 0;

        for (int i = 0; i < friendCount; i++)
        {
            // Отримуємо Steam ID друга
            CSteamID friendID = SteamFriends.GetFriendByIndex(i, EFriendFlags.k_EFriendFlagImmediate);

            // Перевіряємо, чи грає цей друг у гру
            FriendGameInfo_t friendGameInfo;
            if (SteamFriends.GetFriendGamePlayed(friendID, out friendGameInfo))
            {
                // Порівнюємо AppID з поточною грою
                if (friendGameInfo.m_gameID.AppID() == SteamUtils.GetAppID())
                {
                    friendsInGame++;
                }
            }
        }

        return friendsInGame;
    }
}
