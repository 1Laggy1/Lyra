using System;
using System.Collections.Generic;
using UnityEngine;

public class WalkableEnemy : Enemy
{
    public float detectionRadius = 5f; // Radius for detecting the player
    private List<Transform> playersTransforms = new List<Transform>();
    private Transform currentPlayer;
    private NPCAI npcAI;

    void Start()
    {
        base.Start(); // Call the base class Start method

        if (!isServer)
        {
            this.enabled = false;
            return;
        }
        npcAI = GetComponent<NPCAI>();
        if (npcAI == null)
        {
            Debug.LogError("NPCAI component not found on WalkableEnemy!");
        }

        // Find the player (assuming the player has a tag "Player")
        List<GameObject> players = new List<GameObject>();
        players.AddRange(GameObject.FindGameObjectsWithTag("Player"));
        foreach (GameObject player in players)
        {
            playersTransforms.Add(player.transform);
        }
        FindClosestPlayer();

    }

    void FixedUpdate()
    {
        if (npcAI != null)
        {
            FindClosestPlayer();
            float distanceToPlayer = Vector2.Distance(transform.position, currentPlayer.position);

            if (distanceToPlayer <= detectionRadius)
            {
                // Move towards the player
                npcAI.WalkNow(currentPlayer.position, true);
            }
            else
            {
                // Stop moving if the player is out of range
                npcAI.StopWalking();
            }
        }
    }
    void FindClosestPlayer()
    {
        if (playersTransforms.Count == 1)
        {
            currentPlayer = playersTransforms[0];
        }
        else if (Vector2.Distance(playersTransforms[0].position, transform.position) > Vector2.Distance(playersTransforms[1].position, transform.position))
        {
            currentPlayer = playersTransforms[0];
        }
        else
        {
            currentPlayer = playersTransforms[1];
        }
    }
}
