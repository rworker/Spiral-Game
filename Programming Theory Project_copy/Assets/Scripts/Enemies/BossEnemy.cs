using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemy : Enemy
{
    [SerializeField] private GameObject magicPrefab;
    [SerializeField] private GameObject axePrefab;
    [SerializeField] private GameObject fireballPrefab;
    [SerializeField] private float fireRate;
    
    [SerializeField] private Transform throwInstantiationPoint;
    [SerializeField] private Transform castPosition;
    private float nextFire;
    private Vector3 projectileSpawnPos;

    [SerializeField] private AudioClip fireballAttackSound;
    [SerializeField] private AudioClip axeAttackSound;
    [SerializeField] private AudioClip magicAttackSound;

    

    private int currentAttackIndex;
    

    public override void Attack()
    {
        if (currentAttackIndex == 0)
        {
            FireballAttack();
        }
        if (currentAttackIndex == 1)
        {
            AxeAttack();
        }

        if (currentAttackIndex == 2)
        {
            MagicAttack();
        }
    }

    public override void AttackAnim()
    {
       // FireballAttack();
        GetRandomAttackAnim();
    }

    private void AxeAttack()
    {
        enemyAudio.PlayOneShot(axeAttackSound);
        Quaternion launchRotation = Quaternion.Euler(0, transform.eulerAngles.y + 270f, 0);
        //if (Time.time > nextFire)
        //{
        Instantiate(axePrefab, throwInstantiationPoint.position, launchRotation);
        //nextFire = Time.time + fireRate;
        //}
    }


    private void MagicAttack()
    {
        enemyAudio.PlayOneShot(magicAttackSound);
        Vector3 offset = new Vector3(0, 3f, 0);
        projectileSpawnPos = transform.position + offset;

        Instantiate(magicPrefab, projectileSpawnPos, Quaternion.identity);
    }

    private void FireballAttack()
    {
        enemyAudio.PlayOneShot(fireballAttackSound);
        Vector3 pos = new Vector3(castPosition.position.x, castPosition.position.y, 0); //technically don't need casterPos object anymore
        Quaternion rotation = Quaternion.Euler(0, 0, 0); //just initial value
        
        Instantiate(fireballPrefab, pos, rotation);
    }


    private void GetRandomAttackAnim()
    {
        /*if (Mathf.Abs(player.transform.position.y - transform.position.y) < 2f)
        {
            animator.SetTrigger("Fireball");
            currentAttackIndex = 0;
            return;
        }*/
        float randomFloat = Random.Range(0, 100);
        animator.ResetTrigger("ExitAttack");
        if (randomFloat > 50)
        {
            currentAttackIndex = 0;
            
            animator.SetTrigger("Fireball");
        }
        else if (randomFloat < 50 && randomFloat > 25)
        {
            currentAttackIndex = 1;
            animator.SetTrigger("Axe");
        }
        else if(randomFloat < 25)
        {
            currentAttackIndex = 2;
            animator.SetTrigger("Magic");
        }
    }
}
