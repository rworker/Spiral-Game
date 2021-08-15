using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHelperScript : MonoBehaviour
{
    private Enemy enemyController;

    // this is an intermediary script used to connect the model's animated controller to the Enemy script on the parent empty game object.
    // this allows me to call functions in the parent Enemy script within animation events in the model animator
    void Start()
    {
        enemyController = GetComponentInParent<Enemy>();
    }

    public void Die()
    {
        enemyController.Die();
    }

    public void Attack()
    {
        enemyController.Attack();
        
    }

    public void DoneAttacking() //used to serve as function for animation event, changes isAttacking to false and allows enemy the option to move again (if it needs to chase player)
    {
        enemyController.DoneAttacking();
    }


}
