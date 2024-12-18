using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpawnOneAttack
{
    public float Time;
    public List<AttackInfo> attackInfos = new List<AttackInfo>();
}



[System.Serializable]
public class AttackInfo
{
    public Vector2 Spawnpoint;
    public GameObject Go;
    public float Amount;
}

[CreateAssetMenu(fileName = "SpawnInTime", menuName = "Custom/SpawnInformation", order = 1)]
public class SpawnInTime : ScriptableObject
{
    public string Name;
    public bool Haos;
    public List<SpawnOneAttack> WhatToSpawn = new List<SpawnOneAttack>();
}
