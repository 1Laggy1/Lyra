using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerCombat : Entity
{
    [SerializeField]
    PlayerCombatRange pcr;

    // Start is called before the first frame update
    void Start()
    {
        return;
    }

    public override void Attack(int fasing)
    {
        if (pcr.EnemysInRange.Count() != 0 && TimeSinceAttack > AttackSpeed)
        {
            TimeSinceAttack = 0;
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

    public void OnCollisionStay2D(Collision2D other)
    {
        if (isLocalPlayer)
        {
            if (other.gameObject.tag == "Enemy")
            {
                other.gameObject.GetComponent<Enemy>().AttackPlayer(this);

            }
        }

    }
}
