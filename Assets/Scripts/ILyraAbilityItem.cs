using System.Collections;
using System.Collections.Generic;
using Mirror;
using Steamworks;
using UnityEngine;

public class ILyraAbilityItem : NetworkBehaviour
{
    [SerializeField]
    LyraSpellsAudios lyraSpellsAudios;
    [SerializeField]
    AudioSource audioSource;
    [SerializeField]
    LyraSpellType lyraSpellType;
    [SerializeField]
    public GameObject LyraItem;
    public virtual void UseAbility()
    {
        StartAudio();
    }
    public virtual void Start()
    {
        Debug.Log("Start");
        StartCoroutine(GetLyraAbility());
    }
    IEnumerator GetLyraAbility()
    {
        Debug.Log("Start finding Lyra");
        yield return new WaitUntil(() => GameObject.Find("Lyra(Clone)") != null);
        GameObject.Find("Lyra(Clone)").GetComponent<LyraAbility>().AbilityActivatedBool += AbilityActivated;
        Debug.Log("Lyra finded");
    }
    void AbilityActivated(bool activated)
    {
        Debug.Log("Lyra ability used");
        if (activated)
        {
            LyraItem.SetActive(true);
        }
        else
        {
            LyraItem.SetActive(false);
        }
    }
    [Command(requiresAuthority = false)]
    public void StartAudio()
    {
        int audioIndex = 0;
        switch (lyraSpellType)
        {
            case LyraSpellType.Easy:
                audioIndex = Random.Range(0, lyraSpellsAudios.EasySpells.Count);
                break;
            case LyraSpellType.Medium:
                audioIndex = Random.Range(0, lyraSpellsAudios.MediumSpells.Count);
                break;
            case LyraSpellType.Heavy:
                audioIndex = Random.Range(0, lyraSpellsAudios.HeavySpells.Count);
                break;
        }
        StartAudioClientRPC(audioIndex);
    }
    [ClientRpc]
    public void StartAudioClientRPC(int audioIndex)
    {
        if (PlayerPrefs.GetFloat("Volume_effects") != 0)
            audioSource.volume = PlayerPrefs.GetFloat("Volume_effects");
        switch (lyraSpellType)
        {
            case LyraSpellType.Easy:
                audioSource.PlayOneShot(lyraSpellsAudios.EasySpells[audioIndex]);
                break;
            case LyraSpellType.Medium:
                audioSource.PlayOneShot(lyraSpellsAudios.MediumSpells[audioIndex]);
                break;
            case LyraSpellType.Heavy:
                audioSource.PlayOneShot(lyraSpellsAudios.HeavySpells[audioIndex]);
                break;
        }
    }
}
