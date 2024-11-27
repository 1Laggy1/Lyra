using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class DoorLyraAbilityItem : ILyraAbilityItem
{
    [SerializeField]
    DoorVertical doorVertical;
    public override void UseAbility()
    {
        base.UseAbility();
        doorVertical.Use();
    }
}
