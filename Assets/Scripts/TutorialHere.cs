using System.Collections;
using System.Collections.Generic;
using Unity.Plastic.Antlr3.Runtime.Tree;
using UnityEngine;

public class TutorialHere : MonoBehaviour
{
    [SerializeField]
    string tutor;
    [SerializeField]
    CenterTextManager ctm;
    // Start is called before the first frame update
    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            ctm.ShowText("", tutor);
            gameObject.SetActive(false);
            enabled = false;
        }
    }
}
