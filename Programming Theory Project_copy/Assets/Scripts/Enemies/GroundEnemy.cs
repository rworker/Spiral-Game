using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundEnemy : Enemy
{
    [SerializeField] private int damageOutput;
    [SerializeField] private float hitDistance;
    //[SerializeField] private Collider attackCollider;


    public override void Attack()
    {
        if (Vector3.Distance(player.gameObject.transform.position, transform.position) <  hitDistance)
        {
            player.TakeDamage(damageOutput);
        }
        base.Attack();
    }
}
