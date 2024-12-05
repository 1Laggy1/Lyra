using System;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseScript : MonoBehaviour
{
    public GameObject PausePanel;

    public void BackToMenu()
    {
        // todo shutting down server if player is host
        SceneManager.LoadScene(0);
    }

    public void Resume()
    {
        PausePanel.SetActive(false);
    }

    void Update()
    {
        if (Input.GetButtonDown("Pause"))
        {
            if (!PausePanel.activeInHierarchy)
            {
                PausePanel.SetActive(true);
            }
            else
            {
                PausePanel.SetActive(false);
            }
        }
    }
}