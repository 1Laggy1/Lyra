using System.Collections;
using System.Collections.Generic;
using Mirror;
using Org.BouncyCastle.Asn1.X509;
using UnityEngine;

public class DialogOnStart : NetworkBehaviour
{
    [SerializeField]
    DialogSO dialog;
    [SerializeField]
    DialogManager dm;

    [SerializeField] private Transform player1;
    [SerializeField] private Transform player2;
    void Start()
    {
        if (!isServer)
        {
            enabled = false;
        }
        StartCoroutine(WaitPlayers());

    }
    IEnumerator WaitPlayers()
    {
        yield return new WaitUntil(() => GameObject.FindGameObjectsWithTag("Player").Length == 2);
        player1 = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<Transform>();
        player2 = GameObject.FindGameObjectsWithTag("Player")[1].GetComponent<Transform>();
        yield return new WaitForSeconds(2);
        this.enabled = false;
        this.enabled = true;
        dm.StartDialogQueueServerOnly(dialog);
    }
}
