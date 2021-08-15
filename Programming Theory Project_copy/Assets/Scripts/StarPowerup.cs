using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarPowerup : PowerUp
{
    public override void ApplyPowerup() //activated when player triggers powerup
    {
        if (!playerController.isInvincible && !playerController.hasFirePowerup) //makes sure player doesn't already have a powerup
        {
            playerController.isInvincible = true;
        }

        base.ApplyPowerup(); //applies general powerup logic like deleting the powerup gameobject
    }
}
