using System.Collections;
using System.Collections.Generic;
using Mirror;
using Org.BouncyCastle.Asn1.X509;
using UnityEngine;

public class StartDialogHere : NetworkBehaviour
{
    [SerializeField]
    DialogSO dialog;
    [SerializeField]
    DialogManager dm;

    void Start()
    {
        if (!isServer)
        {
            enabled = false;
        }
        dm = GameObject.FindGameObjectWithTag("DialogManager").GetComponent<DialogManager>();
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && isServer)
        {
            if (isServer)
            {
                dm.StartDialogQueueServerOnly(dialog);
            }
            else
            {
                dm.StartDialogQueueCommand(dialog);
            }
            gameObject.SetActive(false);
        }
    }
}
