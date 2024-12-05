using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsScript : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject mmm;
    public Slider MusicVolumeSlider;

    public Slider EventsVolumeSlider;

    public GameObject PreviousPannel;
    public GameObject SettingsPannel;

    void Start()
    {
        mmm = GameObject.Find("MainMenuMusic");
        var musicVolume = PlayerPrefs.GetFloat("Volume_mainmenu");
        var effectsVolume = PlayerPrefs.GetFloat("Volume_effects");
        MusicVolumeSlider.value = musicVolume;
        EventsVolumeSlider.value = effectsVolume;
    }

    public void OnMusicSliderValueChange()
    {
        PlayerPrefs.SetFloat("Volume_mainmenu", MusicVolumeSlider.value);
        if (mmm != null)
        {
        mmm.GetComponent<MainMenuMusic>().ChangeVolume();
        }
    }

    public void OnEventsSliderValueChange()
    {
        PlayerPrefs.SetFloat("Volume_effects", EventsVolumeSlider.value);
    }

    public void BackToPreviousPannnel()
    {
        SettingsPannel.SetActive(false);
        PreviousPannel.SetActive(true);
    }

}
