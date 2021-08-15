using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowerEnemy : Enemy
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform throwInstantiationPoint;
    //[SerializeField] private float fireRate;
    //private float nextFire;

    public override void Attack()
    {
        base.Attack();
        Quaternion launchRotation = Quaternion.Euler(0, transform.eulerAngles.y + 270f, 0);
        //if (Time.time > nextFire)
        //{
        Instantiate(projectilePrefab, throwInstantiationPoint.position, launchRotation);
        //nextFire = Time.time + fireRate;
        //}
    }
}
