using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneNameOnStart : MonoBehaviour
{
    CenterTextManager ct;
    [SerializeField]
    public string description;
    // Start is called before the first frame update
    void Start()
    {

        ct = GameObject.FindGameObjectWithTag("CenterTextManager").GetComponent<CenterTextManager>();
        ct.ShowText(SceneManager.GetActiveScene().name, description);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
