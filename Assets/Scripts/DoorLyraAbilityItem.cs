using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class DoorLyraAbilityItem : MonoBehaviour, ILyraAbilityItem
{
    [SerializeField]
    DoorVertical doorVertical;
    public void UseAbility()
    {
        doorVertical.Use();
    }
}
