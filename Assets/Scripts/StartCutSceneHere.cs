using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartCutSceneHere : CutScene
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            StartAnim();
            gameObject.SetActive(false);
            enabled = false;
        }
    }
}
