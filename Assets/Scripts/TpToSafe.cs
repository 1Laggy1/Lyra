using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TpToSafe : MonoBehaviour
{
    [SerializeField]
    Transform tpTo;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && other.GetComponent<PlayerMovement>().isLocalPlayer)
        {
            other.transform.position = tpTo.position;
        }
    }
}
