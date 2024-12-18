using System.Collections;
using Mirror;
using UnityEngine;

public class WaitPlayers : NetworkBehaviour
{
    GameObject Lyra;
    GameObject Kayden;
    [ClientRpc]
    public void WaitForKayden(GameObject Lyra)
    {
        Lyra = GameObject.Find("Lyra(Clone)");

        Lyra.SetActive(false);
        StartCoroutine(FindKayden());
    }
    IEnumerator FindLyra()
    {
        yield return new WaitUntil(() => GameObject.Find("Lyra(Clone)") != null);
        Kayden.SetActive(true);
    }
    [ClientRpc]
    public void WaitForLyra(GameObject Lyra)
    {
        Kayden = GameObject.Find("Kayden(Clone)");
        Kayden.SetActive(false);
        StartCoroutine(FindLyra());
    }
    IEnumerator FindKayden()
    {
        yield return new WaitUntil(() => GameObject.Find("Kayden(Clone)") != null);
        Lyra.SetActive(true);
    }
}