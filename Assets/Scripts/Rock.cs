using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class Rock : ILyraAbilityItem
{
    [SerializeField]
    float damage;
    [SerializeField]
    public int fasing;
    public List<GameObject> entitiesAttacked = new List<GameObject>();

    public bool isFalling;
    public bool timeToCheck;
    [SerializeField]
    public Rigidbody2D rb;
    [SerializeField]
    public float cooldown = 15;
    [SerializeField]
    public bool respawnable = true;
    public bool canActivate = true;
    public Vector3 startPostion;
    public override void Start()
    {
        base.Start();
        rb.isKinematic = true;
        startPostion = transform.position;
    }
    void OnCollisionEnter2D(Collision2D other)
    {
        if (isServer && (other.gameObject.tag == "Enemy" || other.gameObject.tag == "Player") && isFalling)
        {
            Entity entity = other.gameObject.GetComponent<Entity>();

            if (!entitiesAttacked.Contains(other.gameObject))
            {
                entitiesAttacked.Add(other.gameObject);
                entity.DamageRPC(damage, fasing);
            }
        }

    }
    void OnCollisionStay2D(Collision2D other)
    {
        if (timeToCheck)
        {
            if (other.gameObject.layer == 7)
            {
                isFalling = false;
                canActivate = false;
            }
        }
    }
    public override void UseAbility()
    {
        base.UseAbility();
        Begin();
    }
    public virtual void Begin()
    {
        rb.velocity = Vector3.zero;

    }

}
