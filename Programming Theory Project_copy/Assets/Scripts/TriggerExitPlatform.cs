using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerExitPlatform : MonoBehaviour
{
    [SerializeField] private BossEnemy boss;
    [SerializeField] Animator animator;

    // Update is called once per frame
    void Update()
    {
        if (boss.enemyHearts.IsDead())
        {
            animator.SetTrigger("BossDefeated");
        }
    }
}
