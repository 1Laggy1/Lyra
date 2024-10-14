using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerCombat : Entity
{
    [SerializeField]
    PlayerCombatRange pcr;
    [SerializeField]
    float attackSpeed;

    [SerializeField]
    float timeSinceAttack;

    // Start is called before the first frame update
    void Start()
    {
        return;
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceAttack += Time.deltaTime;
        return;
    }

    public override void Attack(int fasing)
    {
        if (pcr.EnemysInRange.Count() != 0 && timeSinceAttack > attackSpeed)
        {
            timeSinceAttack = 0;
            List<Enemy> enemies = new List<Enemy>();
            foreach (Enemy e in pcr.EnemysInRange)
            {
                enemies.Add(e);
            }

            foreach (Enemy e in enemies)
            {
                e.Damage(DamageF, fasing);
            }
        }
    }
}
