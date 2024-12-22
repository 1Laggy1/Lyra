using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class StartCutSceneHere : CutScene
{
    bool animStarted;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if (!animStarted)
            {
                animStarted = true;
                StartAnim();
            }
            gameObject.SetActive(false);
            enabled = false;
        }
    }
}
