using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHelperScript : MonoBehaviour
{
    private PlayerController playerController;
    // Start is called before the first frame update
    void Start()
    {
        playerController = GetComponentInParent<PlayerController>();
    }

    public void FireFireball()
    {
        playerController.FireFireball();
        playerController.CheckIfMoving(); //checks if player is moving and exits fireball animation early if they are
    }
}
