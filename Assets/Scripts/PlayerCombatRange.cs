using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatRange : MonoBehaviour
{
    [SerializeField]
    BoxCollider2D c2D;
    public List<Enemy> EnemysInRange = new List<Enemy>();

    void Start()
    {
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            EnemysInRange.Add(other.GetComponent<Enemy>());
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            EnemysInRange.Remove(EnemysInRange.Find(enemy => enemy == other.GetComponent<Enemy>()));
        }
    }
}
