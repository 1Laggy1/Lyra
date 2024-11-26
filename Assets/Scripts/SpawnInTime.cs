using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FloatGameObjectPair
{
    public float Amount;
    public float Time;
    public Vector2 Spawnpoint;
    public GameObject Go;
}

[CreateAssetMenu(fileName = "SpawnInTime", menuName = "Custom/SpawnInformation", order = 1)]
public class SpawnInTime : ScriptableObject
{
    public bool Haos;
    public List<FloatGameObjectPair> WhatToSpawn = new List<FloatGameObjectPair>();
}
