using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LyraSpellsAudios", menuName = "Custom/LyraSpellsAudios", order = 2)]
public class LyraSpellsAudios : ScriptableObject
{
    public List<AudioClip> EasySpells = new List<AudioClip>();
    public List<AudioClip> MediumSpells = new List<AudioClip>();
    public List<AudioClip> HeavySpells = new List<AudioClip>();
    public GameObject LyraItem;
}
