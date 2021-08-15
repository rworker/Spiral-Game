using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballPowerup : PowerUp
{
    public override void ApplyPowerup() //activated when player triggers powerup
    {
        //if (!playerController.isInvincible && !playerController.hasFirePowerup) //makes sure player doesn't already have a powerup
        if (!playerController.hasFirePowerup)
        {
            playerController.hasFirePowerup = true;
            playerController.timeSinceFireball = playerController.fireballFireRate; //initially sets timesicefireball to equal firerate so player can immediately fire upon picking up fireball
        }

        else
        {
            playerScore.AddtoScore(30); //adds a score bonus is player already has the fireball powerup when picking up another
        }

        base.ApplyPowerup(); //applies general powerup logic like deleting the powerup gameobject
    }
}
