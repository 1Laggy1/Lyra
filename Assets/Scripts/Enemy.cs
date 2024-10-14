using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    void Start()
    {
        EntityDied += Died;
    }

    void Died(object sender, EventArgs eventArgs)
    {
        Destroy(gameObject);
    }
}
