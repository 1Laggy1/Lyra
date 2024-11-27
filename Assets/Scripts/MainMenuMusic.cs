using UnityEngine;

public class MainMenuMusic : MonoBehaviour
{
    [SerializeField]
    AudioSource audioSource;
    public void Start()
    {
        if (PlayerPrefs.GetFloat("Volume_mainmenu") != 0)
            audioSource.volume = PlayerPrefs.GetFloat("Volume_mainmenu");
    }


    public void ChangeVolume()
    {
        if (PlayerPrefs.GetFloat("Volume_mainmenu") != 0)
            audioSource.volume = PlayerPrefs.GetFloat("Volume_mainmenu");
    }
}