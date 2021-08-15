using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerAnimation : MonoBehaviour
{
    [SerializeField] Animator animator;
    private bool alreadyTriggered;

    private void OnTriggerEnter(Collider other) 
    {
        if (!alreadyTriggered && other.gameObject.tag == "Player")
        {
            animator.SetTrigger("Trap"); //triggers the ground behind player to move up and block them
            alreadyTriggered = true;
        }
        
    }
}
