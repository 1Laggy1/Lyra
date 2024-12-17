using System.Collections;
using System.Collections.Generic;
using Mirror;
using NUnit.Framework.Internal.Commands;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : NetworkBehaviour
{
    bool kaydenHere;
    bool lyraHere;
    [SerializeField]
    string nextSceneName;
    NetworkManager nm;

    void Start()
    {
        if (!isServer)
        {
            this.enabled = false;
            return;
        }

        nm = GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<NetworkManager>();
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == "Kayden(Clone)")
        {
            kaydenHere = true;
            TryChangeScene();
        }
        else if (other.name == "Lyra(Clone)")
        {
            lyraHere = true;
            TryChangeScene();
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.name == "Kayden(Clone)")
        {
            kaydenHere = false;
        }
        else if (other.name == "Lyra(Clone)")
        {
            lyraHere = false;
        }
    }

    void TryChangeScene()
    {
        Debug.Log($"Trying changing scene Lyra: {lyraHere}, Kayden: {kaydenHere}");
        if (kaydenHere && lyraHere)
        {
            PlayerPrefs.SetString("LastSceneName", nextSceneName);
            nm.ServerChangeScene(nextSceneName);
        }
    }
}
