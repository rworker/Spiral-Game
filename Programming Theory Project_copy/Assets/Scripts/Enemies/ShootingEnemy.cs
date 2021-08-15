using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingEnemy : Enemy
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float fireRate;
    private float nextFire;
    private Vector3 projectileSpawnPos;
   

    public override void Start() 
    {
       // nextFire = Time.time;

        base.Start();
    }

    public override void Attack()
    {
        //if (Time.time > nextFire)
        //{
        base.Attack();
        Vector3 offset = new Vector3 (0, 3f, 0);
        projectileSpawnPos = transform.position + offset;

        Instantiate (projectilePrefab, projectileSpawnPos, Quaternion.identity);
            //nextFire = Time.time + fireRate;
        //}
    }
}
