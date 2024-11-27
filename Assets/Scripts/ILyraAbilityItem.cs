using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class ILyraAbilityItem : NetworkBehaviour
{
    [SerializeField]
    LyraSpellsAudios lyraSpellsAudios;
    [SerializeField]
    AudioSource audioSource;
    [SerializeField]
    LyraSpellType lyraSpellType;
    public virtual void UseAbility()
    {
        StartAudio();
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
