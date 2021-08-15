using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadCheck : MonoBehaviour
{
    private Enemy enemy;
    private PlayerController player;
    public Collider headCollider;
    private bool alreadyTriggered = false;


    private void Start() 
    {
        enemy = gameObject.GetComponentInParent<Enemy>();
        player = FindObjectOfType<PlayerController>();

        headCollider = GetComponent<Collider>();
        enemy.OnDead += Enemy_OnDead; //subscribes function
    }

    private void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.CompareTag("Player") && !alreadyTriggered)
        {
            alreadyTriggered = true;
            player.animator.SetTrigger("jumped");
            player.Jump(player.jumpForce); //makes player do small jump upon stomping enemy
            enemy.TakeDamage(5);


            alreadyTriggered = false;

            //logic for player jumping slightly upon bouncing on enemy
        }
    }

    private void Enemy_OnDead(object sender, System.EventArgs e)
    {
        headCollider.enabled = false;
    }

}
