using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckforEnemies : MonoBehaviour
{

    Enemy enemy;
    //checks to see if player lands on enem

    private void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.tag == "Enemy")
        {
            enemy = other.gameObject.GetComponent<Enemy>();
            enemy.enemyHearts.Damage(5);
        }
    }
}
